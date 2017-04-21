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
using Iap.Handlers;
using System.Threading;

namespace Iap.DynamicEnglishScreens
{
   public class DynamicEnShell6ViewModel:Screen
    {
        private readonly IEventAggregator events;
        private readonly ILog log;
        private ImageSource bannerBackground;
        private bool isBannerVisible;
        private bool bannerActive;
        private string arrow;
        private bool openDisclaimer;

        private BitmapImage image1;
        private BitmapImage image2;
        private BitmapImage image3;
        private BitmapImage image4;
        private BitmapImage image5;
        private BitmapImage image6;

        private readonly ISendStatsService sender;

        private Timer bannerNotifier;
        private int activeBannerImage;

        private readonly object bannerSyncLock = new object();

        public DynamicEnShell6ViewModel(IEventAggregator events, ILog log, ISendStatsService sender)
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

        public BitmapImage Image3
        {
            set
            {
                this.image3 = value;
                NotifyOfPropertyChange(() => this.Image3);
            }

            get
            {
                return this.image3;
            }
        }

        public BitmapImage Image4
        {
            set
            {
                this.image4 = value;
                NotifyOfPropertyChange(() => this.Image4);
            }

            get
            {
                return this.image4;
            }
        }

        public BitmapImage Image5
        {
            set
            {
                this.image5 = value;
                NotifyOfPropertyChange(() => this.Image5);
            }

            get
            {
                return this.image5;
            }
        }

        public BitmapImage Image6
        {
            set
            {
                this.image6 = value;
                NotifyOfPropertyChange(() => this.Image6);
            }

            get
            {
                return this.image6;
            }
        }


        public List<ButtonLinkModel> ButtonsDetails
        {
            get;
            set;
        }

        public List<BannerModel> BannerImages
            { get; set; }

        public void PopulateButtonLinks(List<ButtonLinkModel> populatedList)
        {
            this.ButtonsDetails = populatedList;
            this.Image1 = populatedList[0].ExternalEngImg;
            this.Image2 = populatedList[1].ExternalEngImg;
            this.Image3 = populatedList[2].ExternalEngImg;
            this.Image4 = populatedList[3].ExternalEngImg;
            this.Image5 = populatedList[4].ExternalEngImg;
            this.Image6 = populatedList[5].ExternalEngImg;
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
            if (this.BannerImages == null)
            {
                var imageFileNames = Path.Combine(
                    Path.GetDirectoryName(
                        this.GetType().Assembly.Location
                    ), "Media"
                ).EnumerateImageFiles().ToArray();

                this.BannerBackground = imageFileNames.Where(
                    x => Path.GetFileNameWithoutExtension(x) == "banner_EN"
                ).Select(x => new BitmapImage(new Uri(x))).SingleOrDefault();
            }
            else
            {
                this.BannerBackground =
                    this.BannerImages[this.activeBannerImage].AdImageEN;
            }

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

        protected override void OnViewLoaded(object view)
        {
            this.OpenBanner();
            GlobalCounters.ResetAll();
            DeletePdfFiles();

            ((DynamicEnShell6View)view).CloseDisclaimer.Click +=
                                            CloseDisclaimer_Click;

            this.activeBannerImage = 0;

            this.bannerNotifier = new Timer(
                this.SetActiveBannerImage, null, 0, Timeout.Infinite
            );

            this.bannerNotifier.Change(0, Timeout.Infinite);

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
            this.CloseDisclaimer();
        }

        public void ViewRedirect1()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[0].EnUrl, this.ButtonsDetails, "1"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[0].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[0].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect2()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[1].EnUrl, this.ButtonsDetails, "2"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[1].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[1].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect3()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[2].EnUrl, this.ButtonsDetails, "3"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[2].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[2].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect4()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[3].EnUrl, this.ButtonsDetails, "4"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[3].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[3].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect5()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[4].EnUrl, this.ButtonsDetails, "5"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[4].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[4].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect6()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[5].EnUrl, this.ButtonsDetails, "6"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[5].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[5].Title + ".");
            }
            catch { }
        }

        public void ViewAdvertLink()
        {
            if ((this.BannerImages == null) ||
                (this.BannerImages[this.activeBannerImage].AdLinkEN == string.Empty))
            {
                return;
            }

            this.events.PublishOnBackgroundThread(
                    new ViewDynamicBannerEnCommand(
                                this.ButtonsDetails,
                                this.BannerImages[this.activeBannerImage].AdLinkEN
                    )
            );

            try
            {
                this.sender.SendAction("ViewBannerLink.");
            }
            catch { }
            this.log.Info("Invoking Action: ViewBannerLink");
        }

        public void ViewGreek()
        {
            this.events.PublishOnCurrentThread(new ViewDynamicGreekShellCommand());
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

        public string DisclaimerEnglish
        {
            get
            {
                return GlobalText.getDiscalimerEnglishText();
            }
        }

        private void SetActiveBannerImage(object state)
        {
            lock (this.bannerSyncLock)
            {
                if (++this.activeBannerImage ==
                                this.BannerImages.Count)
                {
                    this.activeBannerImage = 0;
                }

                BannerModel data =
                    this.BannerImages[this.activeBannerImage];

                if (data.AdImageEN == null)
                {
                    this.bannerNotifier.Change(0, Timeout.Infinite);
                }
                else
                {
                    this.BannerBackground = data.AdImageEN;

                    this.bannerNotifier.Change(
                        (data.AdDelayTime * 1000), Timeout.Infinite
                    );
                }
            }
        }
    }
}
