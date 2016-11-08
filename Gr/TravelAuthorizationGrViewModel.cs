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
   public class TravelAuthorizationGrViewModel:Screen
    {
        private readonly IEventAggregator events;
        private readonly string travelAuthorizationGrApi;
        private bool showKeyboard;

        private string remainingTime;

        public static ChromiumWebBrowser _travelAuthorizationBrowser;

        public TravelAuthorizationGrViewModel(IEventAggregator events, string travelAuthorizationGrApi)
        {
            this.events = events;
            this.travelAuthorizationGrApi = travelAuthorizationGrApi;
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
            _travelAuthorizationBrowser = new ChromiumWebBrowser()
            {
                Address = this.travelAuthorizationGrApi,
            };

            _travelAuthorizationBrowser.BrowserSettings = new CefSharp.BrowserSettings()
            {
                OffScreenTransparentBackground = false,
            };

            _travelAuthorizationBrowser.Load(this.travelAuthorizationGrApi);

            ((TravelAuthorizationGrView)view).InternetAccessBrowser.Children.Add(_travelAuthorizationBrowser);

            _travelAuthorizationBrowser.TouchDown += _travelAuthorizationBrowser_TouchDown;

            _travelAuthorizationBrowser.TouchMove += _travelAuthorizationBrowser_TouchMove;

            _travelAuthorizationBrowser.RequestContext = new RequestContext();

            _travelAuthorizationBrowser.Focus();

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
                if (_travelAuthorizationBrowser != null)
                {
                    _travelAuthorizationBrowser.Dispose();
                }
                this.events.PublishOnCurrentThread(new ViewGreekCommand());
            }
        }

        public string RemainingTime
        {
            set
            {
                this.remainingTime = value;
                NotifyOfPropertyChange(() => this.RemainingTime);
            }
            get
            {
                return this.remainingTime + "'";
            }
        }

        public bool ShowKeyboard
        {
            set
            {
                this.showKeyboard = value;
                NotifyOfPropertyChange(() => this.ShowKeyboard);
            }
            get
            {
                return this.showKeyboard;
            }
        }

        public int timeElapsed = 30;

        public int lastMousePositionX;
        public int lastMousePositionY;

        private void _travelAuthorizationBrowser_TouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {
            int x = (int)e.GetTouchPoint(_travelAuthorizationBrowser).Position.X;
            int y = (int)e.GetTouchPoint(_travelAuthorizationBrowser).Position.Y;


            int deltax = x - lastMousePositionX;
            int deltay = y - lastMousePositionY;

            _travelAuthorizationBrowser.SendMouseWheelEvent((int)_travelAuthorizationBrowser.Width, (int)_travelAuthorizationBrowser.Height, deltax, deltay, CefEventFlags.None);
        }

        private void _travelAuthorizationBrowser_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            lastMousePositionX = (int)e.GetTouchPoint(_travelAuthorizationBrowser).Position.X;
            lastMousePositionY = (int)e.GetTouchPoint(_travelAuthorizationBrowser).Position.Y;
        }

        public void Back()
        {
            if (_travelAuthorizationBrowser != null)
            {
                _travelAuthorizationBrowser.Dispose();
            }
            this.events.PublishOnCurrentThread(new ViewEnglishCommand());
        }

        protected override void OnDeactivate(bool close)
        {
            try
            {
                if (_travelAuthorizationBrowser != null)
                {
                    _travelAuthorizationBrowser.Dispose();
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

        public void ViewInternetAccess()
        {
            this.events.PublishOnCurrentThread(new ViewInternetAccessCommand());
        }
    }
}
