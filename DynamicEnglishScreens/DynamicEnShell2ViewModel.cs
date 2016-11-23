using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Windows.Media.Imaging;
using Iap.Models;
using Iap.Commands;

namespace Iap.DynamicEnglishScreens
{
   public class DynamicEnShell2ViewModel:Screen
    {
        private readonly IEventAggregator events;
        private string bannerBackground;
        private bool isBannerVisible;

        private BitmapImage image1;
        private BitmapImage image2;
        private BitmapImage image3;
        private BitmapImage image4;

        public DynamicEnShell2ViewModel(IEventAggregator events)
        {
            this.events = events;
        }

        public BitmapImage Image1
        {
            set
            {
                this.image1 = value;
                NotifyOfPropertyChange(() => this.Image1);
            }
            get
            {
                return this.image1;
            }
        }

        public BitmapImage Image2
        {
            set
            {
                this.image2 = value;
                NotifyOfPropertyChange(() => this.Image2);
            }

            get
            {
                return this.image2;
            }
        }

        public List<ButtonLinkModel> ButtonsDetails
        {
            get;
            set;
        }

        public void PopulateButtonLinks(List<ButtonLinkModel> populatedList)
        {
            this.ButtonsDetails = populatedList;
            this.Image1 = populatedList[0].ExternalEngImg;
            this.Image2 = populatedList[1].ExternalEngImg;
        }

        public IEventAggregator Events
        {
            get
            {
                return this.events;
            }
        }

        public string BannerBackground
        {
            set
            {
                this.bannerBackground = value;
                NotifyOfPropertyChange(() => this.BannerBackground);
            }
            get
            {
                return this.bannerBackground;
            }
        }

        public bool IsBannerVisible
        {
            set
            {
                this.isBannerVisible = value;
                NotifyOfPropertyChange(() => this.IsBannerVisible);
            }
            get
            {
                return this.isBannerVisible;
            }
        }

        public void OpenBanner()
        {
            this.BannerBackground = "/Images/AIA_FOR PNG-11.png";
            this.IsBannerVisible = true;
        }

        public void CloseBanner()
        {
            this.BannerBackground = "";
            this.IsBannerVisible = false;
        }

        protected override void OnViewLoaded(object view)
        {
            this.OpenBanner();
            base.OnViewLoaded(view);
        }

        public void ViewRedirect1()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[0].EnUrl, this.ButtonsDetails, "1"));
        }

        public void ViewRedirect2()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[1].EnUrl, this.ButtonsDetails, "2"));
        }

        public void ViewGreek()
        {
            this.events.PublishOnCurrentThread(new ViewDynamicGreekShellCommand());
            this.events.PublishOnCurrentThread(new ViewChangeLanguageCommand(true));
        }
    }
}
