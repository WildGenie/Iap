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

namespace Iap.DynamicGreekScreens
{
   public class DynamicGrShell7ViewModel:Screen
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
        private BitmapImage image4;
        private BitmapImage image5;
        private BitmapImage image6;
        private BitmapImage image7;

        public DynamicGrShell7ViewModel(IEventAggregator events, ILog log, ISendStatsService sender)
        {
            this.events = events;
            this.log = log;
            this.sender = sender;
        }

        public IEventAggregator Events
        {
            get { return this.events; }
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

        public BitmapImage Image7
        {
            set
            {
                this.image7 = value;
                NotifyOfPropertyChange(() => this.Image7);
            }
            get
            {
                return this.image7;
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
            this.Image3 = populatedList[2].ExternalGrImg;
            this.Image4 = populatedList[3].ExternalGrImg;
            this.Image5 = populatedList[4].ExternalGrImg;
            this.Image6 = populatedList[5].ExternalGrImg;
            this.Image7 = populatedList[6].ExternalGrImg;
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
            GlobalCounters.ResetAll();
            DeletePdfFiles();

            ((DynamicGrShell7View)view).CloseDisclaimer.Click += CloseDisclaimer_Click;

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

        public void ViewRedirect3()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[2].GrUrl, this.ButtonsDetails, "3"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[2].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[2].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect4()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[3].GrUrl, this.ButtonsDetails, "4"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[3].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[3].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect5()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[4].GrUrl, this.ButtonsDetails, "5"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[4].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[4].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect6()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[5].GrUrl, this.ButtonsDetails, "6"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[5].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[5].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect7()
        {
            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[6].GrUrl, this.ButtonsDetails, "7"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[6].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[6].Title + ".");
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
                return GlobalText.getDiscalimerGreekText();
            }
        }
    }
}
