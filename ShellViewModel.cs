using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Iap.Commands;
using Iap.Handlers;
using System.Windows.Controls.Primitives;
using Iap.Services;
using System.Windows.Media;
using System.IO;
using Iap.Envy;
using System.Windows.Media.Imaging;

namespace Iap
{
   public class ShellViewModel:Screen
    {
        private readonly IEventAggregator events;
        private ImageSource bannerBackground;
        private bool isBannerVisible;
        private bool bannerActive;
        private string arrow;
        private bool openDisclaimer;

        private string RemainingTime = "30";

        private readonly ISendStatsService sender;

        public ShellViewModel(IEventAggregator events, ISendStatsService sender)
        {
            this.events = events;
            this.sender = sender;
        }


        protected override void OnViewLoaded(object view)
        {
            this.OpenBanner();
            GlobalCounters.ResetAll();
            DeletePdfFiles();
            ((ShellView)view).CloseDisclaimer.Click += CloseDisclaimer_Click;

            base.OnViewLoaded(view);
        }

        public void DeletePdfFiles()
        {
            try
            {
                string path = System.IO.Path.Combine(
                              System.IO.Path.GetDirectoryName(
                              this.GetType().Assembly.Location),
                              "Printings");


                System.IO.DirectoryInfo di = new DirectoryInfo(path);

                if (Directory.Exists(path))
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }
            }
            catch { }
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
                NotifyOfPropertyChange(()=>this.Arrow);
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

        public bool BannerActive
        {
            get { return this.bannerActive; }

            set
            {
                this.bannerActive = value;

                base.NotifyOfPropertyChange(() => this.BannerActive);
            }
        }

        public void OpenBanner()
        {
            var imageFileNames = Path.Combine(Path.GetDirectoryName(
                                        this.GetType().Assembly.Location
            ), "Media").EnumerateImageFiles().ToArray();

            this.BannerBackground = imageFileNames.Where(
                x => Path.GetFileNameWithoutExtension(x) == "banner"
            ).Select(x => new BitmapImage(new Uri(x))).SingleOrDefault();

            if (this.BannerBackground == null)
            {
                this.IsBannerVisible = false;
                this.BannerActive = false;
            }
            else
            {
                this.Arrow = null;
                this.IsBannerVisible = true;
                this.BannerActive = true;
            }
        }

        public void CloseBanner()
        {
            this.BannerBackground = null;
            this.Arrow = "/Images/AIA_FOR PNG-02.png";
            this.IsBannerVisible = false;
        }

        public void ViewBuyWifi()
        {
            this.events.PublishOnBackgroundThread(new ViewBuyWifiCommand(this.RemainingTime));
            try
            {
                this.sender.SendAction("ViewBuyWifi.");
            }
            catch { }
        }

        public void ViewPrintBoardingPass()
        {
            this.events.PublishOnBackgroundThread(new ViewPrintBoardingPassCommand(this.RemainingTime));
            try
            {
                this.sender.SendAction("ViewPrintBoardingPass.");
            }
            catch { }
        }

        public void ViewInternetAccess()
        {
            this.events.PublishOnBackgroundThread(new ViewInternetAccessCommand(this.RemainingTime));
            try
            {
                this.sender.SendAction("ViewInternetAccess.");
            }
            catch { }
        }

        public void ViewTravelAuthorization()
        {
            this.events.PublishOnBackgroundThread(new ViewTravelAuthorizationCommand(this.RemainingTime));
            try
            {
                this.sender.SendAction("ViewTravelAuthorization.");
            }
            catch { }
        }

        public void ViewAdvertLink()
        {
            this.events.PublishOnCurrentThread(new ViewAdvertCommand(this.RemainingTime));
            try
            {
                this.sender.SendAction("ViewBannerLink.");
            }
            catch { }
        }

        public void ViewGreek()
        {
            this.events.PublishOnCurrentThread(new ViewGreekCommand());
            this.events.PublishOnCurrentThread(new ViewChangeLanguageCommand(true));
            try
            {
                this.sender.SendAction("ViewGreek.");
            }
            catch { }
        }

        public void ViewDisclaimer()
        {
            if (!this.OpenDisclaimer)
            {
                this.OpenDisclaimer = true;
                try
                {
                    this.sender.SendAction("ViewDisclaimer.");
                }
                catch { }
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

        public string DisclaimerEnglish
        {
            get
            {
                return GlobalText.getDiscalimerEnglishText();
            }
        }
    }
}
