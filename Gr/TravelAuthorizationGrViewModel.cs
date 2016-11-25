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
using Iap.Handlers;
using Iap.Bounds;
using System.Windows.Input;
using System.Windows.Controls;

namespace Iap.Gr
{
   public class TravelAuthorizationGrViewModel:Screen
    {
        private readonly IEventAggregator events;
        private readonly string travelAuthorizationGrApi;
        private bool openKeyboard;
        private readonly string numberOfAvailablePagesToPrint;

        private string remainingTime;

        public static ChromiumWebBrowser _travelAuthorizationBrowser;

        private int TimeElapsed = 30;
        private DispatcherTimer timer;

        public TravelAuthorizationGrViewModel(IEventAggregator events, string travelAuthorizationGrApi, string numberOfAvailablePagesToPrint)
        {
            this.events = events;
            this.travelAuthorizationGrApi = travelAuthorizationGrApi;
            this.numberOfAvailablePagesToPrint = numberOfAvailablePagesToPrint;
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

            var obj = new BoundObject("el", Convert.ToInt32(numberOfAvailablePagesToPrint));
            _travelAuthorizationBrowser.RegisterJsObject("bound", obj);
            _travelAuthorizationBrowser.FrameLoadEnd += obj.OnFrameLoadEnd;

            ((PrintBoardingPassGrView)view).InternetAccessBrowser.Children.Add(_travelAuthorizationBrowser);

            ((TravelAuthorizationGrView)view).InternetAccessBrowser.Children.Add(_travelAuthorizationBrowser);

            _travelAuthorizationBrowser.TouchDown += _travelAuthorizationBrowser_TouchDown;

            _travelAuthorizationBrowser.TouchMove += _travelAuthorizationBrowser_TouchMove;

            _travelAuthorizationBrowser.MouseDown += _travelAuthorizationBrowser_MouseDown;

            _travelAuthorizationBrowser.RequestContext = new RequestContext();
            _travelAuthorizationBrowser.LifeSpanHandler = new LifeSpanHandler();
            _travelAuthorizationBrowser.RequestHandler = new RequestHandler(Convert.ToInt32(numberOfAvailablePagesToPrint));

            _travelAuthorizationBrowser.Focus();

            this.RemainingTime = "30";

            this.TimeElapsed = 30;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Tick += TimerTick;
            timer.Start();

            base.OnViewLoaded(view);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (TimeElapsed > 1)
            {
                TimeElapsed--;
                this.RemainingTime = TimeElapsed.ToString();
            }
            else
            {
                timer.Stop();
                this.events.PublishOnCurrentThread(new ViewEnglishCommand());
            }
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

                var task = _travelAuthorizationBrowser.EvaluateScriptAsync(script, TimeSpan.FromSeconds(10));
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


        public int lastMousePositionX;
        public int lastMousePositionY;

        private TouchDevice windowTouchDevice;
        private System.Windows.Point lastPoint;

        private void _travelAuthorizationBrowser_TouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {
            /* int x = (int)e.GetTouchPoint(_travelAuthorizationBrowser).Position.X;
             int y = (int)e.GetTouchPoint(_travelAuthorizationBrowser).Position.Y;


             int deltax = x - lastMousePositionX;
             int deltay = y - lastMousePositionY;

             _travelAuthorizationBrowser.SendMouseWheelEvent((int)_travelAuthorizationBrowser.Width, (int)_travelAuthorizationBrowser.Height, deltax, deltay, CefEventFlags.None);*/
            Control control = (Control)sender;

            var currentTouchPoint = windowTouchDevice.GetTouchPoint(null);

            var locationOnScreen = control.PointToScreen(new System.Windows.Point(currentTouchPoint.Position.X, currentTouchPoint.Position.Y));

            var deltaX = locationOnScreen.X - lastPoint.X;
            var deltaY = locationOnScreen.Y - lastPoint.Y;

            lastPoint = locationOnScreen;

            _travelAuthorizationBrowser.SendMouseWheelEvent((int)lastPoint.X, (int)lastPoint.Y, (int)deltaX, (int)deltaY, CefEventFlags.None);
        }

        private void _travelAuthorizationBrowser_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            //lastMousePositionX = (int)e.GetTouchPoint(_travelAuthorizationBrowser).Position.X;
            //lastMousePositionY = (int)e.GetTouchPoint(_travelAuthorizationBrowser).Position.Y;
            Control control = (Control)sender;
            e.TouchDevice.Capture(control);
            windowTouchDevice = e.TouchDevice;
            var currentTouchPoint = windowTouchDevice.GetTouchPoint(null);


            var locationOnScreen = control.PointToScreen(new System.Windows.Point(currentTouchPoint.Position.X, currentTouchPoint.Position.Y));
            lastPoint = locationOnScreen;
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
            timer.Stop();

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

        public void ViewTravelAuthorization()
        {
            _travelAuthorizationBrowser.Load(this.travelAuthorizationGrApi);
        }
    }
}
