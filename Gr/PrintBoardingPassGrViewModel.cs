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
   public class PrintBoardingPassGrViewModel:Screen
    {
        private IEventAggregator events;
        private readonly string boardingPassGrApi;
        private bool showKeyboard;

        private string remainingTime;

        public static ChromiumWebBrowser _printBoardingPassBrowser;

        public PrintBoardingPassGrViewModel(IEventAggregator events, string boardingPassGrApi)
        {
            this.events = events;
            this.boardingPassGrApi = boardingPassGrApi;
        }

        public IEventAggregator Events
        {
            get {
                return this.events;
            }
        }

        protected override void OnViewLoaded(object view)
        {
            _printBoardingPassBrowser = new ChromiumWebBrowser()
            {
                Address = this.boardingPassGrApi,
            };

            _printBoardingPassBrowser.BrowserSettings = new CefSharp.BrowserSettings()
            {
                OffScreenTransparentBackground = false,
            };

            _printBoardingPassBrowser.Load(this.boardingPassGrApi);

            ((PrintBoardingPassGrView)view).InternetAccessBrowser.Children.Add(_printBoardingPassBrowser);

            _printBoardingPassBrowser.TouchDown += _printBoardingPassBrowser_TouchDown;

            _printBoardingPassBrowser.TouchMove += _printBoardingPassBrowser_TouchMove;

            _printBoardingPassBrowser.RequestContext = new RequestContext();

            _printBoardingPassBrowser.Focus();

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
                if (_printBoardingPassBrowser != null)
                {
                    _printBoardingPassBrowser.Dispose();
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

        private void _printBoardingPassBrowser_TouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {
            int x = (int)e.GetTouchPoint(_printBoardingPassBrowser).Position.X;
            int y = (int)e.GetTouchPoint(_printBoardingPassBrowser).Position.Y;


            int deltax = x - lastMousePositionX;
            int deltay = y - lastMousePositionY;

            _printBoardingPassBrowser.SendMouseWheelEvent((int)_printBoardingPassBrowser.Width, (int)_printBoardingPassBrowser.Height, deltax, deltay, CefEventFlags.None);
        }

        private void _printBoardingPassBrowser_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            lastMousePositionX = (int)e.GetTouchPoint(_printBoardingPassBrowser).Position.X;
            lastMousePositionY = (int)e.GetTouchPoint(_printBoardingPassBrowser).Position.Y;
        }

        public void Back()
        {
            if (_printBoardingPassBrowser != null)
            {
                _printBoardingPassBrowser.Dispose();
            }
            this.events.PublishOnCurrentThread(new ViewGreekCommand());
        }

        protected override void OnDeactivate(bool close)
        {
            try
            {
                if (_printBoardingPassBrowser != null)
                {
                    _printBoardingPassBrowser.Dispose();
                }
            }
            catch { }
            base.OnDeactivate(close);
        }

        public void ViewBuyWifi()
        {
            this.events.PublishOnCurrentThread(new ViewBuyWifiCommand());
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
