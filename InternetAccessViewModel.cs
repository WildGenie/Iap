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
            _internetAccessBrowser.LifeSpanHandler = new LifeSpanHandler();
            _internetAccessBrowser.RequestHandler = new CustomRequestHandler("",log,sender,this.numberOfAvailablePagesToPrint,events);
            _internetAccessBrowser.MenuHandler = new CustomMenuHandler();
            _internetAccessBrowser.DialogHandler = new CustomDialogHandler();
            
            ((InternetAccessView)view).InternetAccessBrowser.Children.Add(_internetAccessBrowser);

            _internetAccessBrowser.TouchDown += _internetAccessBrowser_TouchDown;

            _internetAccessBrowser.TouchMove += _internetAccessBrowser_TouchMove;

            _internetAccessBrowser.MouseDown += _internetAccessBrowser_MouseDown;

            _internetAccessBrowser.PreviewMouseUp += _internetAccessBrowser_PreviewMouseUp;

            _internetAccessBrowser.RequestContext = new RequestContext();
            

            var boundObject = new CustomBoundObject(this.numberOfAvailablePagesToPrint, this.log, sender,events);
            _internetAccessBrowser.RegisterJsObject("bound", boundObject);
            _internetAccessBrowser.FrameLoadEnd += boundObject.OnFrameLoadEnd;

            _internetAccessBrowser.Focus();


            GlobalCounters.ResetAll();

            
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Tick += TimerTick;
            timer.Start();

            startTime = DateTime.Now;

            base.OnViewLoaded(view);
        }

        private void _internetAccessBrowser_PreviewMouseUp(object sender, MouseButtonEventArgs e)
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
                                    isText=true;
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

            e.Handled = false;
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
               /* string script =
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
                    })();";*/
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
                                    isText=true;
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
            try
            {
                Control control = (Control)sender;

                var currentTouchPoint = windowTouchDevice.GetTouchPoint(null);

                var locationOnScreen = control.PointToScreen(new System.Windows.Point(currentTouchPoint.Position.X, currentTouchPoint.Position.Y));

                var deltaX = locationOnScreen.X - lastPoint.X;
                var deltaY = locationOnScreen.Y - lastPoint.Y;

                lastPoint = locationOnScreen;

                _internetAccessBrowser.SendMouseWheelEvent((int)lastPoint.X, (int)lastPoint.Y, (int)deltaX, (int)deltaY, CefEventFlags.None);
            }
            catch
            {
                
            }
        }

        private void _internetAccessBrowser_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            try
            {
                Control control = (Control)sender;
                e.TouchDevice.Capture(control);
                windowTouchDevice = e.TouchDevice;
                var currentTouchPoint = windowTouchDevice.GetTouchPoint(null);


                var locationOnScreen = control.PointToScreen(new System.Windows.Point(currentTouchPoint.Position.X, currentTouchPoint.Position.Y));
                lastPoint = locationOnScreen;
            }
            catch
            {
                
            }
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
                if (_internetAccessBrowser.CanGoBack)
                {
                    _internetAccessBrowser.Back();
                }
                else
                {
                    try
                    {
                        if (_internetAccessBrowser != null)
                        {
                            _internetAccessBrowser.Dispose();
                        }
                    }
                    catch { }
                    try
                    {
                        this.log.Info("Invoking Action: ViewEndNavigateSession  after " + (30 - this.TimeElapsed).ToString() + " minutes.");
                        this.sender.SendAction("ViewEndNavigateSession after " + (30 - this.TimeElapsed).ToString() + " minutes.");
                    }

                    catch
                    { }
                    this.events.PublishOnCurrentThread(new ViewEnglishCommand());
                }
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
                if (this.ShowBannerUrl)
                {
                    this.log.Info("Invoking Action: ViewClose BannerLink after " + TimeSpended() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose BannerLink after " + TimeSpended() + " time.");
                    }
                    catch { }
                }

                else
                {
                    this.log.Info("Invoking Action: ViewClose InternetAccess after " + TimeSpended() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose InternetAccess after " + TimeSpended() + " time.");
                    }
                    catch { }
                }
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
            try
            {
                this.sender.SendAction("ViewBuyWifi.");
            }
            catch { }
        }

        public void ViewPrintBoardingPass()
        {
            this.events.PublishOnCurrentThread(new ViewPrintBoardingPassCommand(this.TimeElapsed.ToString()));
            try
            {
                this.sender.SendAction("ViewPrintBoardingPass.");
            }
            catch { }
        }

        public void ViewTravelAuthorization()
        {
            this.events.PublishOnCurrentThread(new ViewTravelAuthorizationCommand(this.TimeElapsed.ToString()));
            try
            {
                this.sender.SendAction("ViewTravelAuthorization.");
            }
            catch { }
        }

        public void ViewInternetAccess()
        {
            _internetAccessBrowser.Load(this.internetAccessEnApi);
            try
            {
                this.sender.SendAction("ViewInternetAccess.");
            }
            catch { }
        }
    }
}
