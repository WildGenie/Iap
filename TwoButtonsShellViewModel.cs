﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Iap.Commands;
using Iap.Handlers;

namespace Iap
{
   public class TwoButtonsShellViewModel:Screen
    {
        private readonly IEventAggregator events;

        private string bannerBackground;
        private bool isBannerVisible;
        private string arrow;

        private string RemainingTime = "30";

        public TwoButtonsShellViewModel(IEventAggregator events)
        {
            this.OpenBanner();
            GlobalCounters.ResetAll();
            this.events = events;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
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
        }

        public void ViewInternetAccess()
        {
            this.events.PublishOnBackgroundThread(new ViewInternetAccess2Command(this.RemainingTime));
        }

        public void ViewGreek()
        {
            this.events.PublishOnCurrentThread(new ViewTwoButtonsShellGrCommand());
            this.events.PublishOnCurrentThread(new ViewChangeLanguageCommand(true));
        }

        public void ViewAdvertLink()
        {
            this.events.PublishOnCurrentThread(new ViewTwoButtonsAdvertCommand(this.RemainingTime));
        }
    }
}
