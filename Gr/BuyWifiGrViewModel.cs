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
using Iap.Services;

namespace Iap.Gr
{
   public class BuyWifiGrViewModel:Screen
    {
        private readonly IEventAggregator events;

        private string remainingTime;
        private bool openKeyboard;
        private readonly string numberOfAvailablePagesToPrint;
        private readonly string buyWifiGrApi;
        private readonly ILog log;
        private readonly ISendStatsService sender;

        public static ChromiumWebBrowser _buyWifiBrowser;

       
        private DispatcherTimer timer;

        public BuyWifiGrViewModel(IEventAggregator events, string numberOfAvailablePagesToPrint, string buyWifiGrApi, ILog log, ISendStatsService sender)
        {
            this.events = events;
            this.numberOfAvailablePagesToPrint = numberOfAvailablePagesToPrint;
            this.buyWifiGrApi = buyWifiGrApi;
            this.log = log;
            this.sender = sender;
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
                Address = this.buyWifiGrApi,
            };

            _buyWifiBrowser.BrowserSettings = new CefSharp.BrowserSettings()
            {
                OffScreenTransparentBackground = false,
            };

            _buyWifiBrowser.Load(this.buyWifiGrApi);

            var obj = new CustomBoundObjectEl(this.numberOfAvailablePagesToPrint,this.log);

           _buyWifiBrowser.RegisterJsObject("bound", obj);
            _buyWifiBrowser.FrameLoadEnd += obj.OnFrameLoadEnd;

            ((BuyWifiGrView)view).BuyWifiBrowser.Children.Add(_buyWifiBrowser);

            _buyWifiBrowser.TouchDown += _buyWifiBrowser_TouchDown;

            _buyWifiBrowser.TouchMove += _buyWifiBrowser_TouchMove;

            _buyWifiBrowser.MouseDown += _buyWifiBrowser_MouseDown;

            _buyWifiBrowser.RequestContext = new RequestContext();
            _buyWifiBrowser.LifeSpanHandler = new LifeSpanHandler();
            // _buyWifiBrowser.RequestHandler = new RequestHandler(Convert.ToInt32(numberOfAvailablePagesToPrint));
            _buyWifiBrowser.RequestHandler = new CustomRequestHandler("");
            _buyWifiBrowser.DialogHandler = new CustomDialogHandler();
            _buyWifiBrowser.Focus();

            
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Tick += TimerTick;
            timer.Start();

            startTime = DateTime.Now;
        }

        public int TimeElapsed
        {
            get;
            set;
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

        private void _buyWifiBrowser_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

                var task = _buyWifiBrowser.EvaluateScriptAsync(script, TimeSpan.FromSeconds(10));
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

        private void _buyWifiBrowser_TouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {
            /* int x = (int)e.GetTouchPoint(_buyWifiBrowser).Position.X;
             int y = (int)e.GetTouchPoint(_buyWifiBrowser).Position.Y;


             int deltax = x - lastMousePositionX;
             int deltay = y - lastMousePositionY;

             _buyWifiBrowser.SendMouseWheelEvent((int)_buyWifiBrowser.Width, (int)_buyWifiBrowser.Height, deltax, deltay, CefEventFlags.None);*/
            Control control = (Control)sender;

            var currentTouchPoint = windowTouchDevice.GetTouchPoint(null);

            var locationOnScreen = control.PointToScreen(new System.Windows.Point(currentTouchPoint.Position.X, currentTouchPoint.Position.Y));

            var deltaX = locationOnScreen.X - lastPoint.X;
            var deltaY = locationOnScreen.Y - lastPoint.Y;

            lastPoint = locationOnScreen;

            _buyWifiBrowser.SendMouseWheelEvent((int)lastPoint.X, (int)lastPoint.Y, (int)deltaX, (int)deltaY, CefEventFlags.None);
        }

        private void _buyWifiBrowser_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            // lastMousePositionX = (int)e.GetTouchPoint(_buyWifiBrowser).Position.X;
            // lastMousePositionY = (int)e.GetTouchPoint(_buyWifiBrowser).Position.Y;
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
                try
                {
                    if (_buyWifiBrowser != null)
                    {
                        _buyWifiBrowser.Dispose();
                    }
                }
                catch { }
                this.events.PublishOnCurrentThread(new ViewGreekCommand());
            }
            catch { }
        }

        private string TimeHasSpent()
        {
            int timeSpent = 30 - TimeElapsed;

            return timeSpent.ToString();
        }

        private DateTime startTime;

        private string TimeSpended()
        {
            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime.Subtract(startTime);
            return duration.ToString(@"hh\:mm\:ss");
        }


        protected override void OnDeactivate(bool close)
        {
            timer.Stop();
            try
            {
                this.log.Info("Invoking Action: ViewEndSession after " + TimeSpended() + " time.");
                this.sender.SendAction("ViewEndSession after " + TimeSpended() + " time.");
            }
            catch { }
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
            this.events.PublishOnCurrentThread(new ViewPrintBoardingPassCommand(this.TimeElapsed.ToString()));
            this.sender.SendAction("ViewPrintBoardingPass.");
        }

        public void ViewInternetAccess()
        {
            this.events.PublishOnCurrentThread(new ViewInternetAccessCommand(this.TimeElapsed.ToString()));
            this.sender.SendAction("ViewInternetAccess.");
        }

        public void ViewTravelAuthorization()
        {
            this.events.PublishOnCurrentThread(new ViewTravelAuthorizationCommand(this.TimeElapsed.ToString()));
            this.sender.SendAction("ViewTravelAuthorization.");
        }

        public void ViewBuyWifi()
        {
            _buyWifiBrowser.Load(this.buyWifiGrApi);
            this.sender.SendAction("ViewBuyWifi.");
        }
    }
}
