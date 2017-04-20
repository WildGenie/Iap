using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows.Media.Imaging;

using Newtonsoft.Json.Linq;

using Iap.Models;

namespace Iap.Services
{
    public class BannerService : IBannerService
    {
        private readonly string bannerApi;

        public BannerService(string bannerApi)
        {
            this.bannerApi = bannerApi;
        }

        public IReadOnlyCollection<BannerModel> GetBannerContent()
        {
            ServicePointManager.ServerCertificateValidationCallback
                += (sender, certificate, chainData, sslPolicyErrors)
                    => true;

            try
            {
                HttpClient client = new HttpClient();

                client.Timeout = TimeSpan.FromSeconds(5);

                HttpResponseMessage result = client.GetAsync(
                            bannerApi,
                            HttpCompletionOption.ResponseContentRead
                ).Result;

                result.EnsureSuccessStatusCode();

                string data =
                    result.Content.ReadAsStringAsync().Result;

                WebClient contentClient = new WebClient();

                return BannerService.RetrieveData(contentClient,
                                                  data).ToList();
            }
            catch
            {
                return null;
            }
        }

        private static IEnumerable<BannerModel> RetrieveData(
                                    WebClient client, string data
        )
        {
            JToken result = JToken.Parse(data);

            foreach (JToken element in result)
            {
                JToken contents = JToken.Parse(element.ToString());

                string title = contents["Title"].ToString();

                string link = contents["AdvertImage"].ToString();

                BitmapImage adImageEN = null;

                if (!string.IsNullOrEmpty(link))
                {
                    adImageEN = BannerService.DownloadImage(client,
                                                            link);
                    if (adImageEN == null)
                    {
                        yield return null;
                    }
                }

                link = contents["AdvertImageGr"].ToString();

                BitmapImage adImageGR = null;

                if (!string.IsNullOrEmpty(link))
                {
                    adImageGR = BannerService.DownloadImage(client,
                                                            link);
                    if (adImageGR == null)
                    {
                        yield return null;
                    }
                }

                string adLinkEN = contents["AdvertUrl"].ToString();

                if (adLinkEN == "[]")
                {
                    adLinkEN = string.Empty;
                }

                string adLinkGR = contents["AdvertUrlGr"].ToString();

                if (adLinkGR == "[]")
                {
                    adLinkGR = string.Empty;
                }

                uint adDelayTime;

                if (!uint.TryParse(contents["Advertdelay"].ToString(),
                                   out adDelayTime))
                {
                    yield return null;
                }

                yield return new BannerModel(title, adImageEN,
                                             adImageGR, adLinkEN,
                                             adLinkGR, adDelayTime);
            }
        }

        private static BitmapImage DownloadImage(WebClient client,
                                                 string link)
        {
            MemoryStream stream = null;

            try
            {
                byte[ ] buffer = client.DownloadData(link);

                BitmapImage image = new BitmapImage();

                image.BeginInit();

                image.CacheOption = BitmapCacheOption.OnLoad;

                stream = new MemoryStream(buffer);

                image.StreamSource = stream;
                image.EndInit();

                return image;
            }
            catch
            {
                if (stream != null)
                {
                    stream.Close();
                }

                return null;
            }
        }
    }
}
