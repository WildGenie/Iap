using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Iap.Commands;

namespace Iap
{
   public class ShellViewModel:Screen
    {
        private readonly IEventAggregator events;
        private string bannerBackground;
        private bool isBannerVisible;

        public ShellViewModel(IEventAggregator events)
        {
            this.events = events;
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

        public void ViewBuyWifi()
        {
            this.events.PublishOnBackgroundThread(new ViewBuyWifiCommand());
        }

        public void ViewPrintBoardingPass()
        {
            this.events.PublishOnBackgroundThread(new ViewPrintBoardingPassCommand());
        }

        public void ViewInternetAccess()
        {
            this.events.PublishOnBackgroundThread(new ViewInternetAccessCommand());
        }

        public void ViewTravelAuthorization()
        {
            this.events.PublishOnBackgroundThread(new ViewTravelAuthorizationCommand());
        }

        public void ViewGreek()
        {
            this.events.PublishOnCurrentThread(new ViewGreekCommand());
            this.events.PublishOnCurrentThread(new ViewChangeLanguageCommand(true));
        }
    }
}
