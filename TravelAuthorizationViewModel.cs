using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using CefSharp;
using CefSharp.Wpf;
using System.Windows.Threading;
using Iap.Commands;
using Iap.Handlers;

namespace Iap
{
   public class TravelAuthorizationViewModel:Screen
    {
        private readonly IEventAggregator events;
        private readonly string travelAuthorizationEnApi;
        private bool openKeyboard;

        private string remainingTime;

        public static ChromiumWebBrowser _travelAuthorizationBrowser;

        public TravelAuthorizationViewModel(IEventAggregator events,string travelAuthorizationEnApi)
        {
            this.events = events;
            this.travelAuthorizationEnApi = travelAuthorizationEnApi;
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
                Address = this.travelAuthorizationEnApi,
            };

            _travelAuthorizationBrowser.BrowserSettings = new CefSharp.BrowserSettings()
            {
                OffScreenTransparentBackground = false,
            };

            _travelAuthorizationBrowser.Load(this.travelAuthorizationEnApi);

            ((TravelAuthorizationView)view).InternetAccessBrowser.Children.Add(_travelAuthorizationBrowser);

            _travelAuthorizationBrowser.TouchDown += _travelAuthorizationBrowser_TouchDown;

            _travelAuthorizationBrowser.TouchMove += _travelAuthorizationBrowser_TouchMove;

            _travelAuthorizationBrowser.MouseDown += _travelAuthorizationBrowser_MouseDown;

            _travelAuthorizationBrowser.RequestContext = new RequestContext();

            _travelAuthorizationBrowser.LifeSpanHandler = new LifeSpanHandler();
            _travelAuthorizationBrowser.RequestHandler = new RequestHandler();

            _travelAuthorizationBrowser.Focus();

            this.RemainingTime = "30";

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += TimerTick;
            timer.Start();

            base.OnViewLoaded(view);
        }

        private void _travelAuthorizationBrowser_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                string script =
                                @"(function ()
                    {
                        var isText = false;
                        var activeElement = document.activeElement;
                        if (activeElement) {
                            if (activeElement.tagName.toLowerCase() === 'textarea') {                              
                                isText = true;
                            } else {
                                if (activeElement.tagName.toLowerCase() === 'input') {
                                    if (activeElement.hasAttribute('type')) {
                                        var inputType = activeElement.getAttribute('type').toLowerCase();
                                        if (inputType === 'text' || inputType === 'email' || inputType === 'password' || inputType === 'tel' || inputType === 'number' || inputType === 'range' || inputType === 'search' || inputType === 'url') {                                          
                                        isText = true;
                                        }
                                    }
                                }
                            }
                        }
                        return isText;
                    })();";

                var task =_travelAuthorizationBrowser.EvaluateScriptAsync(script, TimeSpan.FromSeconds(10));
                task.Wait();

                var response = task.Result;

                var result = response.Success ? (response.Result ?? "null") : response.Message;


                if (Convert.ToBoolean(result) == true)
                {
                    OpenKeyboard = true;
                    NotifyOfPropertyChange(() => this.OpenKeyboard);
                }

                else
                {
                    OpenKeyboard = false;
                    NotifyOfPropertyChange(() => this.OpenKeyboard);
                }
            }
            catch
            {

            }

            e.Handled = true;
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
                this.events.PublishOnCurrentThread(new ViewEnglishCommand());
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

        public bool OpenKeyboard
        {
            set
            {
                this.openKeyboard = value;
                NotifyOfPropertyChange(() => this.OpenKeyboard);
            }
            get
            {
                return this.openKeyboard;
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
            this.OpenKeyboard = false;
            try
            {
                if (_travelAuthorizationBrowser.CanGoBack)
                {
                    _travelAuthorizationBrowser.Back();
                }
                else
                {
                    if (_travelAuthorizationBrowser != null)
                    {
                        _travelAuthorizationBrowser.Dispose();
                    }
                    this.events.PublishOnCurrentThread(new ViewEnglishCommand());
                }
            }
            catch { }
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
            catch
            { }
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

        public void ViewTravelAuthorization()
        {
            _travelAuthorizationBrowser.Load(this.travelAuthorizationEnApi);
        }
    }
}
