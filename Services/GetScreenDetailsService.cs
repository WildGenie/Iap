using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iap.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Imaging;
using System.Net;
using System.IO;

namespace Iap.Services
{
    public class GetScreenDetailsService : IGetScreenDetailsService
    {

        private readonly string getScreenDetailsApi;
        private readonly string getIapScreensApi;

        private string storeType = System.Configuration.ConfigurationManager.AppSettings["storeType"];


        public GetScreenDetailsService(string getScreenDetailsApi, string getIapScreensApi)
        {
            this.getScreenDetailsApi = getScreenDetailsApi;
            this.getIapScreensApi = getIapScreensApi;
        }

        public IReadOnlyCollection<ButtonLinkModel> GetButtonLinksDetails(string kioskType)
        {

            string url;
            if(kioskType=="WCI")
            {
                url = getScreenDetailsApi;
            }
            else if(kioskType=="IAP")
            {
                url = getIapScreensApi;
            }

            else
            {
                url = getScreenDetailsApi;
            }

            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);

            var response = client
               .GetAsync(
                   url,
                   HttpCompletionOption.ResponseHeadersRead)
               .Result;

            response.EnsureSuccessStatusCode();

            var json = response.Content.ReadAsStringAsync().Result;
           
            return GetScreenDetailsService.Map(json).ToList();

        }

        private static IEnumerable<ButtonLinkModel> Map(string response)
        {
           
            var json = JToken.Parse(response);

            foreach (var result in json)
            {

                var contents = JToken.Parse(result.ToString());

                string title = String.Empty;
                BitmapImage externalImgEn = null;
                BitmapImage externalImgGr = null;
                BitmapImage internalImgEn = null;
                BitmapImage internalImgGr = null;
                BitmapImage selectedImgEn = null;
                BitmapImage selectedImgGr = null;
                string englishUrl = String.Empty;
                string greekUrl = String.Empty;

                title = contents["Title"].ToString();
                externalImgEn = getBitmapImage(contents["Image"].ToString());
                externalImgGr = getBitmapImage(contents["OutpuImageGreek"].ToString());
                internalImgEn = getBitmapImage(contents["InternalImgEn"].ToString());
                internalImgGr = getBitmapImage(contents["InternalImgGr"].ToString());
                selectedImgEn = getBitmapImage(contents["SelectedImgEn"].ToString());
                selectedImgGr = getBitmapImage(contents["SelectedImgGr"].ToString());
                englishUrl = contents["EnglishUrl"].ToString();
                greekUrl = contents["GreekUrl"].ToString();

                yield return new ButtonLinkModel(title,externalImgEn, externalImgGr, internalImgEn, internalImgGr,
                    selectedImgEn, selectedImgGr, englishUrl, greekUrl);
            }
        }


        public static BitmapImage getBitmapImage(string imageUrl)
        {

            var buffer = new WebClient().DownloadData(imageUrl);
            var bitmap = new BitmapImage();

            using (var stream = new MemoryStream(buffer))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }
            return bitmap;
        }
    }
}
