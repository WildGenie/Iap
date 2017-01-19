using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Iap.Services
{
    public class LicenceProviderService : ILicenceProviderService
    {
        private readonly string statusLicencesApi;
        private readonly string sendPCDataApi;
        private readonly string checkLicenceApi;

        public LicenceProviderService(string statusLicencesApi, string sendPCDataApi, string checkLicenceApi)
        {
            this.statusLicencesApi = statusLicencesApi;
            this.sendPCDataApi = sendPCDataApi;
            this.checkLicenceApi = checkLicenceApi;
        }

        public bool hasAlreadyKey()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var path = Path.Combine(directory, "iapSettings.txt");
            if(File.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string RetrieveTypeFromRegistry()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var path = Path.Combine(directory, "iapSettings.txt");
            if(File.Exists(path))
            {
                string line = File.ReadAllLines(path).Where(x => x.ToString().StartsWith("Type=")).FirstOrDefault();
                string type = line.Replace("Type=", "");
                return type;
            }
            else
            {
                return "null";
            }
        }

        public bool writeKeyToRegistry(string type, string id)
        {
            try
            {
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                var path = Path.Combine(directory, "iapSettings.txt");
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("Type=" + type);
                        sw.WriteLine("ID=" + id);
                    }
                }
                return true;
            }
            catch { return false; }
        }

        public void deleteFromRegistry()
        {
            try
            {
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                var path = Path.Combine(directory, "iapSettings.txt");
                if(File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch
            {

            }
        }


        public string getUniquePcId()
        {
            try
            {
                ManagementObjectSearcher searcher = new
         ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

                string serialNo = "";

                foreach (ManagementObject wmi_HD in searcher.Get())
                {

                    if (wmi_HD["SerialNumber"] != null)
                        serialNo += wmi_HD["SerialNumber"].ToString();
                }

                return serialNo.Replace(" ", "");
            }
            catch
            {
                return "";
            }
        }

        public string getProcessorID()
        {
            try
            {
                string cpuID = string.Empty;
                ManagementClass mc = new ManagementClass("win32_processor");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    if (cpuID == "")
                    {
                        cpuID = mo.Properties["processorID"].Value.ToString();

                    }
                }

                return cpuID+"-"+System.Environment.MachineName;
            }
            catch
            {
                return "";
            }
        }

       

        public void ShowUniqueIds()
        {
            string procID = this.getProcessorID();
            string diskID = this.getUniquePcId();
        }

        public string checkLicencesStatus()
        {
            using (HttpClient client = new HttpClient())
            {
                var response =
                   client
                   .GetAsync(
                   this.statusLicencesApi,
               HttpCompletionOption.ResponseHeadersRead).Result;
                return response.Content.ReadAsStringAsync().Result;

            }
        }

        public async Task<string> sendPcData(string type, CancellationToken ct)
        {
              try
              {
                  string procID = this.getProcessorID();
                  string hdID = this.getUniquePcId();
                  var httpClient = new HttpClient();


                  var parameters = new Dictionary<string, string>();
                  parameters["type"] = type;
                  parameters["prid"] = procID;
                  parameters["hdid"] = hdID;

                  var response = await httpClient.PostAsync(this.sendPCDataApi, new FormUrlEncodedContent(parameters), ct);
                  var contents = await response.Content.ReadAsStringAsync();
                  return contents.ToString();
              }
              catch(Exception ex)
              {
                  System.Windows.MessageBox.Show(ex.ToString());
                  return "no";
              }
        }

        public async Task<string> checkPcLicence(CancellationTokenSource ct)
        {
            ct.CancelAfter(TimeSpan.FromSeconds(15));
            try
            {
                string hdID = this.getUniquePcId();
                var parameters = new Dictionary<string, string>();
                parameters["hdid"] = hdID;
               
                var httpClient = new HttpClient();
                var response = await httpClient.PostAsync(this.checkLicenceApi,
                    new FormUrlEncodedContent(parameters), ct.Token);
                var contents = await response.Content.ReadAsStringAsync();
                return  contents.ToString();
            }
            catch(TaskCanceledException)
            {
                return "error";
            }

            catch
            {
                return "0";
            }
        }
    }
}
