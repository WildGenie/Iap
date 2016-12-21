using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Iap.Commands;
using Iap.Handlers;
using Iap.Services;

namespace Iap
{
   public class TwoButtonsShellViewModel:Screen
    {
        private readonly IEventAggregator events;

        private string bannerBackground;
        private bool isBannerVisible;
        private string arrow;
        private bool openDisclaimer;

        private string RemainingTime = "30";

        private readonly ISendStatsService sender;

        public TwoButtonsShellViewModel(IEventAggregator events, ISendStatsService sender)
        {
            this.events = events;
            this.sender = sender;
        }

        protected override void OnViewLoaded(object view)
        {
            this.OpenBanner();
            GlobalCounters.ResetAll();

            ((TwoButtonsShellView)view).CloseDisclaimer.Click += CloseDisclaimer_Click;

            base.OnViewLoaded(view);
        }

        private void CloseDisclaimer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenDisclaimer = false;
        }

        private IEventAggregator Events
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

        public void ViewBuyWifi()
        {
            this.events.PublishOnBackgroundThread(new ViewBuyWifi2Command(this.RemainingTime));
            this.sender.SendAction("ViewBuyWifi.");
        }

        public void ViewInternetAccess()
        {
            this.events.PublishOnBackgroundThread(new ViewInternetAccess2Command(this.RemainingTime));
            this.sender.SendAction("ViewInternetAccess.");
        }

        public void ViewGreek()
        {
            this.events.PublishOnCurrentThread(new ViewTwoButtonsShellGrCommand());
            this.events.PublishOnCurrentThread(new ViewChangeLanguageCommand(true));
            this.sender.SendAction("ViewGreek.");
        }

        public void ViewAdvertLink()
        {
            this.events.PublishOnCurrentThread(new ViewTwoButtonsAdvertCommand(this.RemainingTime));
            this.sender.SendAction("ViewBannerLink.");
        }

        public void ViewDisclaimer()
        {
            if (!this.OpenDisclaimer)
            {
                this.OpenDisclaimer = true;
            }
        }

        public bool OpenDisclaimer
        {
            set
            {
                this.openDisclaimer = value;
                NotifyOfPropertyChange(() => this.OpenDisclaimer);
            }
            get { return this.openDisclaimer; }
        }

        public void CloseDisclaimer()
        {
            this.OpenDisclaimer = false;
        }
    }
}
