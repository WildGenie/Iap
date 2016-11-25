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

namespace Iap
{
   public class InternetAccessViewModel:Screen
    {
        private readonly IEventAggregator events;
        private string remainingTime;
        private readonly string numberOfAvailablePagesToPrint;

        private bool openKeyboard;

        public static ChromiumWebBrowser _internetAccessBrowser;



        private int TimeElapsed = 30;
        private DispatcherTimer timer;


       

        public InternetAccessViewModel(IEventAggregator events, string numberOfAvailablePagesToPrint)
        {
            this.events = events;
            this.numberOfAvailablePagesToPrint = numberOfAvailablePagesToPrint;
        }

        protected override void OnViewLoaded(object view)
        {
            _internetAccessBrowser = new ChromiumWebBrowser()
            {
                Address = "https://www.google.co.uk/",
            };

            _internetAccessBrowser.BrowserSettings = new CefSharp.BrowserSettings()
            {
                OffScreenTransparentBackground = false,
            };

            _internetAccessBrowser.Load("https://www.google.co.uk/");

            _internetAccessBrowser.BrowserSettings.FileAccessFromFileUrls = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.WebSecurity = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.Javascript = CefState.Enabled;
           

           

           // var obj = new BoundTest(6,0,"en");

            var obj = new BoundObject("en",Convert.ToInt32(numberOfAvailablePagesToPrint));

            _internetAccessBrowser.RegisterJsObject("bound", obj);
            _internetAccessBrowser.FrameLoadEnd += obj.OnFrameLoadEnd;

            _internetAccessBrowser.LifeSpanHandler = new LifeSpanHandler();
            _internetAccessBrowser.RequestHandler = new RequestHandler(Convert.ToInt32(numberOfAvailablePagesToPrint));
            _internetAccessBrowser.MenuHandler = new CustomMenuHandler();
         //   _internetAccessBrowser.RenderProcessMessageHandler = new CustomRenderProcessHandler();
            _internetAccessBrowser.JsDialogHandler = new CustomJsDialog();


           // _internetAccessBrowser.FrameLoadEnd += _internetAccessBrowser_FrameLoadEnd;
            

            ((InternetAccessView)view).InternetAccessBrowser.Children.Add(_internetAccessBrowser);

            _internetAccessBrowser.TouchDown += _internetAccessBrowser_TouchDown;

            _internetAccessBrowser.TouchMove += _internetAccessBrowser_TouchMove;

            _internetAccessBrowser.MouseDown += _internetAccessBrowser_MouseDown;

            _internetAccessBrowser.RequestContext = new RequestContext();
            //_internetAccessBrowser.IsManipulationEnabled = true;
            //_internetAccessBrowser.ReleaseAllTouchCaptures();
            //_internetAccessBrowser.ManipulationDelta += _internetAccessBrowser_ManipulationDelta;
            _internetAccessBrowser.Focus();

            this.RemainingTime = "30";

            this.OpenKeyboard = true;

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

        private void _internetAccessBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            var browser = sender as ChromiumWebBrowser;

            var script = @"var beforePrint = function(){

                alert('before');
            };

            var afterPrint = function() {
                alert('after');
            };

            if (window.matchMedia)
            {
                var mediaQueryList = window.matchMedia('print');
                mediaQueryList.addListener(function(mql) {
                    if (mql.matches)
                    {
                        beforePrint();
                    }
                    else
                    {
                        afterPrint();
                    }
                });
            }

            window.onbeforeprint = beforePrint;
            window.onafterprint = afterPrint;";

            browser.ExecuteScriptAsync(script);
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
                if (_internetAccessBrowser.CanGoBack)
                {
                    _internetAccessBrowser.Back();
                }
                else
                {
                    if (_internetAccessBrowser != null)
                    {
                        _internetAccessBrowser.Dispose();
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

        public void ViewInternetAccess()
        {
            _internetAccessBrowser.Load("http://google.com/ncr");
        }
    }
}
