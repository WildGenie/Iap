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
   public class InternetAccessGrViewModel:Screen
    {
        private readonly IEventAggregator events;

        private string remainingTime;

        public static ChromiumWebBrowser _internetAccessBrowser;

        public InternetAccessGrViewModel(IEventAggregator events)
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
            _internetAccessBrowser = new ChromiumWebBrowser()
            {
                Address = "http://www.google.com",
            };

            _internetAccessBrowser.BrowserSettings = new CefSharp.BrowserSettings()
            {
                OffScreenTransparentBackground = false,
            };

            _internetAccessBrowser.Load("http://www.google.com");

            ((InternetAccessGrView)view).InternetAccessBrowser.Children.Add(_internetAccessBrowser);

            _internetAccessBrowser.TouchDown += _internetAccessBrowser_TouchDown;

            _internetAccessBrowser.TouchMove += _internetAccessBrowser_TouchMove;

            _internetAccessBrowser.RequestContext = new RequestContext();

            _internetAccessBrowser.Focus();

            this.RemainingTime = "30";

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += TimerTick;
            timer.Start();

            base.OnViewLoaded(view);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timeElapsed--;
            this.RemainingTime = timeElapsed.ToString();
            if (timeElapsed == 0)
            {
                this.RemainingTime = "0";
                timer.Stop();
                System.Threading.Thread.Sleep(1000);
                timer.Tick -= TimerTick;
                if (_internetAccessBrowser != null)
                {
                    _internetAccessBrowser.Dispose();
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

        private void _internetAccessBrowser_TouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {
            int x = (int)e.GetTouchPoint(_internetAccessBrowser).Position.X;
            int y = (int)e.GetTouchPoint(_internetAccessBrowser).Position.Y;


            int deltax = x - lastMousePositionX;
            int deltay = y - lastMousePositionY;

            _internetAccessBrowser.SendMouseWheelEvent((int)_internetAccessBrowser.Width, (int)_internetAccessBrowser.Height, deltax, deltay, CefEventFlags.None);
        }

        private void _internetAccessBrowser_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            lastMousePositionX = (int)e.GetTouchPoint(_internetAccessBrowser).Position.X;
            lastMousePositionY = (int)e.GetTouchPoint(_internetAccessBrowser).Position.Y;
        }

        public void Back()
        {
            if (_internetAccessBrowser != null)
            {
                _internetAccessBrowser.Dispose();
            }
            this.events.PublishOnCurrentThread(new ViewGreekCommand());
        }

        protected override void OnDeactivate(bool close)
        {
            try
            {
                if (_internetAccessBrowser != null)
                {
                    _internetAccessBrowser.Dispose();
                }
            }
            catch { }
            base.OnDeactivate(close);
        }

        public void ViewBuyWifi()
        {
            this.events.PublishOnCurrentThread(new ViewBuyWifiCommand());
        }

        public void ViewPrintBoardingPass()
        {
            this.events.PublishOnCurrentThread(new ViewPrintBoardingPassCommand());
        }

        public void ViewTravelAuthorization()
        {
            this.events.PublishOnCurrentThread(new ViewTravelAuthorizationCommand());
        }
    }
}
