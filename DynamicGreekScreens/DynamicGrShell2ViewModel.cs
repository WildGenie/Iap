using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Windows.Media.Imaging;
using Iap.Models;
using Iap.Commands;
using Iap.Services;
using System.Windows.Media;
using System.IO;
using Iap.Envy;

namespace Iap.DynamicGreekScreens
{
   public class DynamicGrShell2ViewModel:Screen
    {
        private readonly IEventAggregator events;
        private readonly ILog log;
        private ImageSource bannerBackground;
        private bool isBannerVisible;
        private string arrow;
        private bool openDisclaimer;

        private readonly ISendStatsService sender;

        private BitmapImage image1;
        private BitmapImage image2;

        public DynamicGrShell2ViewModel(IEventAggregator events, ILog log, ISendStatsService sender)
        {
            this.events = events;
            this.log = log;
            this.sender = sender;
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
            this.Image1 = populatedList[0].ExternalGrImg;
            this.Image2 = populatedList[1].ExternalGrImg;
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

        protected override void OnViewLoaded(object view)
        {
            this.OpenBanner();

            ((DynamicGrShell2View)view).CloseDisclaimer.Click += CloseDisclaimer_Click;

            base.OnViewLoaded(view);
        }

        private void CloseDisclaimer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.CloseDisclaimer();
        }

        public void ViewRedirect1()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[0].GrUrl, this.ButtonsDetails, "1"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[0].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[0].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect2()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[1].GrUrl, this.ButtonsDetails, "2"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[1].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[1].Title + ".");
            }
            catch { }
        }

        public void ViewAdvertLink()
        {
            this.events.PublishOnBackgroundThread(new ViewDynamicBannerEnCommand(this.ButtonsDetails));
            try
            {
                this.sender.SendAction("ViewBannerLink.");
            }
            catch { }
            this.log.Info("Invoking Action: ViewBannerLink");
        }

        public void ViewEnglish()
        {
            this.events.PublishOnCurrentThread(new ViewDynamicEnglishShellCommand());
            this.events.PublishOnCurrentThread(new ViewChangeLanguageCommand(false));
            try
            {
                this.sender.SendAction("ViewEnglish.");
            }
            catch { }
        }

        public void ViewDisclaimer()
        {
            if (!this.OpenDisclaimer)
            {
                try
                {
                    this.sender.SendAction("ViewDisclaimer.");
                }
                catch { }
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
