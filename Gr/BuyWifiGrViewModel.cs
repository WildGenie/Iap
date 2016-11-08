using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using CefSharp;
using CefSharp.Wpf;
using Iap.Commands;
using System.Windows.Threading;

namespace Iap.Gr
{
   public class BuyWifiGrViewModel:Screen
    {
        private readonly IEventAggregator events;

        private string remainingTime;

        public static ChromiumWebBrowser _buyWifiBrowser;

        public BuyWifiGrViewModel(IEventAggregator events)
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

        protected override void OnViewLoaded(object view)
        {
            _buyWifiBrowser = new ChromiumWebBrowser()
            {
                Address = "http://www.google.com",
            };

            _buyWifiBrowser.BrowserSettings = new CefSharp.BrowserSettings()
            {
                OffScreenTransparentBackground = false,
            };

            _buyWifiBrowser.Load("http://www.google.com");

            ((BuyWifiGrView)view).BuyWifiBrowser.Children.Add(_buyWifiBrowser);

            _buyWifiBrowser.TouchDown += _buyWifiBrowser_TouchDown;

            _buyWifiBrowser.TouchMove += _buyWifiBrowser_TouchMove;

            _buyWifiBrowser.RequestContext = new RequestContext();

            _buyWifiBrowser.Focus();

            this.RemainingTime = "30";

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            base.OnViewLoaded(view);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timeElapsed--;
            this.RemainingTime = timeElapsed.ToString();
            if (timeElapsed == 0)
            {
                this.RemainingTime = "0";
                timer.Stop();
                System.Threading.Thread.Sleep(1000);
                timer.Tick -= Timer_Tick;
                if (_buyWifiBrowser != null)
                {
                    _buyWifiBrowser.Dispose();
                }
                this.events.PublishOnCurrentThread(new ViewGreekCommand());
            }
        }

        public string RemainingTime
        {
            set
            {
                this.remainingTime = value;
                NotifyOfPropertyChange(() => this.remainingTime);
            }
            get
            {
                return this.remainingTime + "'";
            }
        }

        public int timeElapsed = 30;

        public int lastMousePositionX;
        public int lastMousePositionY;

        private void _buyWifiBrowser_TouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {
            int x = (int)e.GetTouchPoint(_buyWifiBrowser).Position.X;
            int y = (int)e.GetTouchPoint(_buyWifiBrowser).Position.Y;


            int deltax = x - lastMousePositionX;
            int deltay = y - lastMousePositionY;

            _buyWifiBrowser.SendMouseWheelEvent((int)_buyWifiBrowser.Width, (int)_buyWifiBrowser.Height, deltax, deltay, CefEventFlags.None);
        }

        private void _buyWifiBrowser_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            lastMousePositionX = (int)e.GetTouchPoint(_buyWifiBrowser).Position.X;
            lastMousePositionY = (int)e.GetTouchPoint(_buyWifiBrowser).Position.Y;
        }

        public void Back()
        {
            if (_buyWifiBrowser != null)
            {
                _buyWifiBrowser.Dispose();
            }
            this.events.PublishOnCurrentThread(new ViewGreekCommand());
        }

        protected override void OnDeactivate(bool close)
        {
            try
            {
                if (_buyWifiBrowser != null)
                {
                    _buyWifiBrowser.Dispose();
                }
            }
            catch { }
            base.OnDeactivate(close);
        }

        public void ViewPrintBoardingPass()
        {
            this.events.PublishOnCurrentThread(new ViewPrintBoardingPassCommand());
        }

        public void ViewInternetAccess()
        {
            this.events.PublishOnCurrentThread(new ViewInternetAccessCommand());
        }

        public void ViewTravelAuthorization()
        {
            this.events.PublishOnCurrentThread(new ViewTravelAuthorizationCommand());
        }
    }
}
