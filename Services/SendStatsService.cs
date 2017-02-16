using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        }

        private string RetrieveIDFromRegistry()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var path = Path.Combine(directory, "iapSettings.txt");
            if (File.Exists(path))
            {
                string line = File.ReadAllLines(path).Where(x => x.ToString().StartsWith("ID=")).FirstOrDefault();
                string type = line.Replace("ID=", "");
                return type;
            }
            else
            {
                return "null";
            }
        } 

        public  void SendAction(string action)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(20));
            SendActionAsync(action,cts.Token);
        }

        private async void SendActionAsync(string action, CancellationToken ct)
        {
            try
            {
                ServicePointManager
       .ServerCertificateValidationCallback +=
       (sender, cert, chain, sslPolicyErrors) => true;

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
