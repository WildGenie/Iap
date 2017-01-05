using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Iap.Commands;
using CefSharp.Wpf;
using CefSharp;
using System.Windows.Threading;
using Iap.Bounds;
using Iap.Handlers;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using Iap.Unitilities;
using System.Windows.Controls;
using Iap.Services;

namespace Iap
{
   public class InternetAccessViewModel:Screen
    {
        private readonly IEventAggregator events;
        private string remainingTime;
        private readonly string numberOfAvailablePagesToPrint;
        private readonly string internetAccessEnApi;
        private readonly string bannerLinkEnApi;
        private readonly ILog log;
        private readonly ISendStatsService sender;

        private bool openKeyboard;

        public static ChromiumWebBrowser _internetAccessBrowser;

        private DispatcherTimer timer;


       

        public InternetAccessViewModel(IEventAggregator events, string numberOfAvailablePagesToPrint, string internetAccessEnApi,string bannerLinkEnApi, ILog log, ISendStatsService sender)
        {
            this.events = events;
            this.numberOfAvailablePagesToPrint = numberOfAvailablePagesToPrint;
            this.internetAccessEnApi = internetAccessEnApi;
            this.bannerLinkEnApi = bannerLinkEnApi;
            this.log = log;
            this.sender = sender;
        }

        protected override void OnViewLoaded(object view)
        {
            if (!ShowBannerUrl)
            {
                _internetAccessBrowser = new ChromiumWebBrowser()
                {
                    Address = this.internetAccessEnApi,
                };

                _internetAccessBrowser.Load(this.internetAccessEnApi);

                this.OpenKeyboard = true;
            }
            else
            {
                _internetAccessBrowser = new ChromiumWebBrowser()
                {
                    Address = this.bannerLinkEnApi,
                };

                _internetAccessBrowser.Load(this.bannerLinkEnApi);

                this.OpenKeyboard = false;
            }

                _internetAccessBrowser.BrowserSettings = new CefSharp.BrowserSettings()
            {
                OffScreenTransparentBackground = false,
            };

            _internetAccessBrowser.BrowserSettings.FileAccessFromFileUrls = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.WebSecurity = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.Javascript = CefState.Enabled;




            // var obj = new BoundTest(6,0,"en");

            /*  var obj = new BoundObject("en",Convert.ToInt32(numberOfAvailablePagesToPrint));

              _internetAccessBrowser.RegisterJsObject("bound", obj);
              _internetAccessBrowser.FrameLoadEnd += obj.OnFrameLoadEnd;*/

          

            _internetAccessBrowser.LifeSpanHandler = new LifeSpanHandler();
            // _internetAccessBrowser.RequestHandler = new RequestHandler(Convert.ToInt32(numberOfAvailablePagesToPrint));

            _internetAccessBrowser.RequestHandler = new CustomRequestHandler("");

            _internetAccessBrowser.MenuHandler = new CustomMenuHandler();
         //   _internetAccessBrowser.RenderProcessMessageHandler = new CustomRenderProcessHandler();
            _internetAccessBrowser.JsDialogHandler = new CustomJsDialog();

            _internetAccessBrowser.DialogHandler = new CustomDialogHandler();
           // _internetAccessBrowser.FrameLoadEnd += _internetAccessBrowser_FrameLoadEnd;
            

            ((InternetAccessView)view).InternetAccessBrowser.Children.Add(_internetAccessBrowser);

            _internetAccessBrowser.TouchDown += _internetAccessBrowser_TouchDown;

            _internetAccessBrowser.TouchMove += _internetAccessBrowser_TouchMove;

            _internetAccessBrowser.MouseDown += _internetAccessBrowser_MouseDown;

            _internetAccessBrowser.RequestContext = new RequestContext();
            //_internetAccessBrowser.IsManipulationEnabled = true;
            //_internetAccessBrowser.ReleaseAllTouchCaptures();
            //_internetAccessBrowser.ManipulationDelta += _internetAccessBrowser_ManipulationDelta;


            var boundObject = new CustomBoundObject(this.numberOfAvailablePagesToPrint, this.log);
            _internetAccessBrowser.RegisterJsObject("bound", boundObject, true);
            _internetAccessBrowser.FrameLoadEnd += boundObject.OnFrameLoadEnd;

            _internetAccessBrowser.Focus();

            //this.RemainingTime = "30";

            GlobalCounters.ResetAll();

            //this.TimeElapsed = 30;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Tick += TimerTick;
            timer.Start();

            startTime = DateTime.Now;

            base.OnViewLoaded(view);
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

        public bool ShowBannerUrl
        {
            get;
            set;
        }


        private void _internetAccessBrowser_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            if (!e.Handled)
            {
                var point = e.ManipulationOrigin;

                var browser = sender as ChromiumWebBrowser;

                if (browser != null)
                {
                    browser.SendMouseWheelEvent(
                        (int)point.X,
                        (int)point.Y,
                        deltaX: (int)e.DeltaManipulation.Translation.X,
                        deltaY: (int)e.DeltaManipulation.Translation.Y,
                        modifiers: CefEventFlags.None);
                }
            }
        }

        private void _internetAccessBrowser_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

                var task = _internetAccessBrowser.EvaluateScriptAsync(script, TimeSpan.FromSeconds(10));
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

        public string RemainingTime
        {
            set
            {
                this.remainingTime = value;
                NotifyOfPropertyChange(() => this.RemainingTime);
            }
            get
            {
                return this.remainingTime +"'";
            }
        }


       

        private TouchDevice windowTouchDevice;
        private System.Windows.Point lastPoint;
        private void _internetAccessBrowser_TouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {

            Control control = (Control)sender;

            var currentTouchPoint = windowTouchDevice.GetTouchPoint(null);

            var locationOnScreen = control.PointToScreen(new System.Windows.Point(currentTouchPoint.Position.X, currentTouchPoint.Position.Y));

            var deltaX = locationOnScreen.X - lastPoint.X;
            var deltaY = locationOnScreen.Y - lastPoint.Y;

            lastPoint = locationOnScreen;

            _internetAccessBrowser.SendMouseWheelEvent((int)lastPoint.X, (int)lastPoint.Y, (int)deltaX, (int)deltaY, CefEventFlags.None);
        }

        private void _internetAccessBrowser_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            Control control = (Control)sender;
            e.TouchDevice.Capture(control);
            windowTouchDevice = e.TouchDevice;
            var currentTouchPoint = windowTouchDevice.GetTouchPoint(null);


            var locationOnScreen = control.PointToScreen(new System.Windows.Point(currentTouchPoint.Position.X, currentTouchPoint.Position.Y));
            lastPoint = locationOnScreen;
        }

        public IEventAggregator Events
        {
            get
            {
                return this.events;
            }
        }

        public void Back()
        {
            this.OpenKeyboard = false;
            try
            {
                try
                {
                    if (_internetAccessBrowser != null)
                    {
                        _internetAccessBrowser.Dispose();
                    }
                }
                catch { }

                this.events.PublishOnCurrentThread(new ViewEnglishCommand());
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
            this.ShowBannerUrl = false;
            timer.Stop();
            try
            {
                this.log.Info("Invoking Action: ViewEndSession after " + TimeSpended() + " time.");
                this.sender.SendAction("ViewEndSession after " + TimeSpended() + " time.");
            }
            catch { }
            try
            {
                if (_internetAccessBrowser != null)
                {
                    _internetAccessBrowser.Dispose();
                }
            }
            catch
            {
                
            }
            base.OnDeactivate(close);
        }

        public void ViewBuyWifi()
        {
           this.events.PublishOnCurrentThread(new ViewBuyWifiCommand(this.TimeElapsed.ToString()));
            this.sender.SendAction("ViewBuyWifi.");
        }

        public void ViewPrintBoardingPass()
        {
            this.events.PublishOnCurrentThread(new ViewPrintBoardingPassCommand(this.TimeElapsed.ToString()));
            this.sender.SendAction("ViewPrintBoardingPass.");
        }

        public void ViewTravelAuthorization()
        {
            this.events.PublishOnCurrentThread(new ViewTravelAuthorizationCommand(this.TimeElapsed.ToString()));
            this.sender.SendAction("ViewTravelAuthorization.");
        }

        public void ViewInternetAccess()
        {
            _internetAccessBrowser.Load(this.internetAccessEnApi);
            this.sender.SendAction("ViewInternetAccess.");
        }
    }
}
