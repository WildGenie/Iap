using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
            this.kioskID = RetrieveIDFromRegistry();
        }

        private string RetrieveIDFromRegistry()
        {
            string key = "Kiosk";
            RegistryKey keyToRetr = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + key);
            if (keyToRetr != null)
            {
                return keyToRetr.GetValue("ID").ToString();
            }
            else
            {
                return "null";
            }
        }

        private string kioskID;

        public void SendAction(string action)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(20));
            SendActionAsync(action,cts.Token);
        }

        private void SendActionAsync(string action, CancellationToken ct)
        {

           // string kioskID = this.RetrieveIDFromRegistry();

            var response =
                new HttpClient()
                .GetAsync(
                this.sendActionsApi,
            HttpCompletionOption.ResponseHeadersRead, ct).Result;

            var httpClient = new HttpClient();
            var parameters = new Dictionary<string, string>();
            parameters["action"] = action;
            parameters["id"] = kioskID;
            httpClient.PostAsync(this.sendActionsApi, new FormUrlEncodedContent(parameters), ct);
        }
    }
}
