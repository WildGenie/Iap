﻿using System;
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
   public class DynamicEnShell3ViewModel:Screen
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
        private BitmapImage image3;

        private Timer bannerNotifier;
        private int activeBannerImage;

        private readonly object bannerSyncLock = new object();

        public DynamicEnShell3ViewModel(IEventAggregator events, ILog log, ISendStatsService sender)
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
            if (this.BannerImages == null)
            {
                var imageFileNames = Path.Combine(
                    Path.GetDirectoryName(
                        this.GetType().Assembly.Location
                    ), "Media"
                ).EnumerateImageFiles().ToArray();

                this.BannerBackground = imageFileNames.Where(
                    x => Path.GetFileNameWithoutExtension(x) == "banner"
                ).Select(x => new BitmapImage(new Uri(x))).SingleOrDefault();
            }
            else
            {
                this.BannerBackground =
                    this.BannerImages[this.activeBannerImage].AdImageEN;
            }

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
            GlobalCounters.ResetAll();
            DeletePdfFiles();

            ((DynamicEnShell3View)view).CloseDisclaimer.Click +=
                                            CloseDisclaimer_Click;

            this.activeBannerImage = 0;

            this.bannerNotifier = new Timer(
                this.SetActiveBannerImage, null, 0, Timeout.Infinite
            );

            this.bannerNotifier.Change(3000, 3000);

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

        public void ViewAdvertLink()
        {
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
            }
        }
    }
}
