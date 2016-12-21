using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Iap.Services
{
    public class LicenceProviderService : ILicenceProviderService
    {
        private readonly string statusLicencesApi;
        private readonly string sendPCDataApi;

        public LicenceProviderService(string statusLicencesApi, string sendPCDataApi)
        {
            this.statusLicencesApi = statusLicencesApi;
            this.sendPCDataApi = sendPCDataApi;
        }

        public bool hasAlreadyKey()
        {
            try
            {
                string key = "Kiosk";
                RegistryKey keyToRetr = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + key);
                if (keyToRetr != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public string RetrieveTypeFromRegistry()
        {
            string key = "Kiosk";
            RegistryKey keyToRetr = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + key);
            if (keyToRetr != null)
            {
                return keyToRetr.GetValue("Type").ToString();
            }
            else
            {
                return "null";
            }
        }

        public bool writeKeyToRegistry(string type)
        {
            try
            {
                string key = "Kiosk";
                    RegistryKey keyToRetr = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + key);
                    if (keyToRetr == null)
                    {
                        RegistryKey Regkey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\" + key);
                        Regkey.SetValue("Type", type);
                    }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void deleteFromRegistry()
        {

            try
            {
                string key = "kiosk";
                Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\" + key);
            }
            catch { }
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
                        //Remark gets only the first CPU ID
                        cpuID = mo.Properties["processorID"].Value.ToString();

                    }
                }

                return cpuID;
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

            System.Windows.MessageBox.Show(procID + "---" + diskID);
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
    }
}
