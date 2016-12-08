using System;
using Caliburn.Micro;
using Iap.Commands;
using System.Windows.Controls;
using Iap.Handlers;

namespace Iap.Gr
{
   public class ShellGrViewModel:Screen
    {
        private readonly IEventAggregator events;
        private string bannerBackground;
        private bool isBannerVisible;
        private string arrow;

        private string RemainingTime = "30";

        public ShellGrViewModel(IEventAggregator events)
        {
            this.events = events;
        }

        protected override void OnViewLoaded(object view)
        {
            this.OpenBanner();
            // grView = ((ShellGrViewModel)view).grView;
            GlobalCounters.ResetAll();
            base.OnViewLoaded(view);
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

        public string Arrow
        {
            set
            {
                this.arrow = value;
                NotifyOfPropertyChange(() => this.Arrow);
            }
            get
            {
                return this.arrow;
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
            this.BannerBackground = "/Images/AIA_FOR_20PNG-1111111.png";
            this.Arrow = null;
            this.IsBannerVisible = true;
        }

        public void CloseBanner()
        {
            this.BannerBackground = "";
            this.Arrow = "/Images/AIA_FOR PNG-02.png";
            this.IsBannerVisible = false;
        }

        public void ViewEnglish()
        {
            this.events.PublishOnCurrentThread(new ViewEnglishCommand());
            this.events.PublishOnCurrentThread(new ViewChangeLanguageCommand(false));
        }

        public void ViewBuyWifi()
        {
            this.events.PublishOnBackgroundThread(new ViewBuyWifiCommand(this.RemainingTime));
        }

        public void ViewPrintBoardingPass()
        {
            this.events.PublishOnBackgroundThread(new ViewPrintBoardingPassCommand(this.RemainingTime));
        }

        public void ViewInternetAccess()
        {
            this.events.PublishOnBackgroundThread(new ViewInternetAccessCommand(this.RemainingTime));
        }

        public void ViewTravelAuthorization()
        {
            this.events.PublishOnBackgroundThread(new ViewTravelAuthorizationCommand(this.RemainingTime));
        }
    }
}
