using System.Windows.Media.Imaging;

namespace Iap.Models
{
    public class BannerModel
    {
        private string title;

        private BitmapImage adImageEN;
        private BitmapImage adImageGR;

        private string adLinkEN;
        private string adLinkGR;

        private uint adDelayTime;

        public BannerModel(
                    string title,
                    BitmapImage adImageEN,
                    BitmapImage adImageGR,
                    string adLinkEN,
                    string adLinkGR,
                    uint adDelayTime
        )
        {
            this.title = title;

            this.adImageEN = adImageEN;
            this.adImageGR = adImageGR;

            this.adLinkEN = adLinkEN;
            this.adLinkGR = adLinkGR;

            this.adDelayTime = adDelayTime;
        }

        public string Title
        {
            get { return this.title; }
        }

        public BitmapImage AdImageEN
        {
            get { return this.adImageEN; }
        }

        public BitmapImage AdImageGR
        {
            get { return this.adImageGR; }
        }

        public string AdLinkEN
        {
            get { return this.adLinkEN; }
        }

        public string AdLinkGR
        {
            get { return this.adLinkGR; }
        }

        public uint AdDelayTime
        {
            get { return this.adDelayTime; }
        }
    }
}
