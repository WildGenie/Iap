using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Iap.Commands;
using Iap.Handlers;
using Iap.Services;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using Iap.Envy;

namespace Iap.Gr
{
   public class TwoButtonsShellGrViewModel:Screen
    {
        private readonly IEventAggregator events;
        private ImageSource bannerBackground;
        private bool isBannerVisible;
        private string arrow;
        private bool openDisclaimer;

        private string RemainingTime = "30";

        private readonly ISendStatsService sender;

        public TwoButtonsShellGrViewModel(IEventAggregator events, ISendStatsService sender)
        {
            this.events = events;
            this.sender = sender;
        }

        protected override void OnViewLoaded(object view)
        {
            this.OpenBanner();
            GlobalCounters.ResetAll();

            ((TwoButtonsShellGrView)view).CloseDisclaimer.Click += CloseDisclaimer_Click;

            base.OnViewLoaded(view);
        }

        private void CloseDisclaimer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.OpenDisclaimer = false;
        }

        public IEventAggregator Events
        {
            get
            {
                return this.events;
            }
        }

        public ImageSource BannerBackground
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
            var imageFileNames =
           Path.Combine(
               Path.GetDirectoryName(
                   this.GetType().Assembly.Location),
               "Media")
           .EnumerateImageFiles()
           .ToArray();
            this.BannerBackground = imageFileNames.Where(x => Path.GetFileNameWithoutExtension(x) == "banner").Select(x => new BitmapImage(new Uri(x))).SingleOrDefault();
            this.Arrow = null;
            this.IsBannerVisible = true;
        }

        public void CloseBanner()
        {
            this.BannerBackground = null;
            this.Arrow = "/Images/AIA_FOR PNG-02.png";
            this.IsBannerVisible = false;
        }

        public void ViewEnglish()
        {
            this.events.PublishOnCurrentThread(new ViewTwoButtonsShellEnCommand());
            this.events.PublishOnCurrentThread(new ViewChangeLanguageCommand(false));
            this.sender.SendAction("ViewEnglish.");
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

        public void ViewAdvertLink()
        {
            this.events.PublishOnCurrentThread(new ViewTwoButtonsAdvertCommand(this.RemainingTime));
            this.sender.SendAction("ViewBannerLink.");
        }

        public void ViewDisclaimer()
        {
            if (!this.OpenDisclaimer)
            {
                this.sender.SendAction("ViewDisclaimer.");
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

        public string DisclaimerGreek
        {
            get
            {
                return GlobalText.DisclaimerGreek;
            }
        }

    }
}
