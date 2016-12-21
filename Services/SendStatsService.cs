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
        }

        public void SendAction(string action)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(20));
            SendActionAsync(action,cts.Token);
        }

        private void SendActionAsync(string action, CancellationToken ct)
        {
            var response =
                new HttpClient()
                .GetAsync(
                this.sendActionsApi,
            HttpCompletionOption.ResponseHeadersRead, ct).Result;

            var httpClient = new HttpClient();
            var parameters = new Dictionary<string, string>();
            parameters["action"] = action;
            httpClient.PostAsync(this.sendActionsApi, new FormUrlEncodedContent(parameters), ct);
        }
    }
}
