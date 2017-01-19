using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace Iap.Services
{
    public class SendStatsService : ISendStatsService
    {
        private readonly string sendActionsApi;

        public SendStatsService(string sendActionsApi)
        {
            this.sendActionsApi = sendActionsApi;
           // this.kioskID = RetrieveIDFromRegistry();
        }

        private string RetrieveIDFromRegistry()
        {
            /* string key = "Kiosk";
             RegistryKey keyToRetr = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + key);
             if (keyToRetr != null)
             {
                 return keyToRetr.GetValue("ID").ToString();
             }
             else
             {
                 return "null";
             }*/
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var path = Path.Combine(directory, "iapSettings.txt");
            if (File.Exists(path))
            {
                // string line = File.ReadLines(path).Skip(1).Take(1).First();
                string line = File.ReadAllLines(path).Where(x => x.ToString().StartsWith("ID=")).FirstOrDefault();
                string type = line.Replace("ID=", "");
                return type;
            }
            else
            {
                return "null";
            }
        }

        private string kioskID;

       

        public  void SendAction(string action)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(20));
            SendActionAsync(action,cts.Token);
        }

        private async void SendActionAsync(string action, CancellationToken ct)
        {

            // string kioskID = this.RetrieveIDFromRegistry();
            try
            {
              /*  var response =
                    new HttpClient()
                    .GetAsync(
                    this.sendActionsApi,
                HttpCompletionOption.ResponseHeadersRead, ct).Result;*/

                var httpClient = new HttpClient();
                var parameters = new Dictionary<string, string>();
                parameters["action"] = action;
                parameters["id"] = Iap.Handlers.GlobalCounters.kioskID;
               await httpClient.PostAsync(this.sendActionsApi, new FormUrlEncodedContent(parameters), ct);
            }
            catch { }
        }
    }
}
