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

namespace Iap
{
   public class InternetAccessViewModel:Screen
    {
        private readonly IEventAggregator events;
        private string remainingTime;

        private bool openKeyboard;

        public static ChromiumWebBrowser _internetAccessBrowser;

        public InternetAccessViewModel(IEventAggregator events)
        {
            this.events = events;
        }

        protected override void OnViewLoaded(object view)
        {
            _internetAccessBrowser = new ChromiumWebBrowser()
            {
                Address = "http://www.google.com/ncr",
            };

            _internetAccessBrowser.BrowserSettings = new CefSharp.BrowserSettings()
            {
                OffScreenTransparentBackground = false,
            };

            _internetAccessBrowser.Load("http://www.google.com/ncr");

            _internetAccessBrowser.BrowserSettings.FileAccessFromFileUrls = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.WebSecurity = CefState.Disabled;
            _internetAccessBrowser.BrowserSettings.Javascript = CefState.Enabled;

            _internetAccessBrowser.IsBrowserInitializedChanged += _internetAccessBrowser_IsBrowserInitializedChanged;

            var obj = new BoundObject(6,0,"en");
            _internetAccessBrowser.RegisterJsObject("bound", obj);
            _internetAccessBrowser.FrameLoadEnd += obj.OnFrameLoadEnd;

            _internetAccessBrowser.LifeSpanHandler = new LifeSpanHandler();
            _internetAccessBrowser.RequestHandler = new RequestHandler();
            _internetAccessBrowser.MenuHandler = new CustomMenuHandler();
            _internetAccessBrowser.RenderProcessMessageHandler = new CustomRenderProcessHandler();
            _internetAccessBrowser.JsDialogHandler = new CustomJsDialog();

            

            ((InternetAccessView)view).InternetAccessBrowser.Children.Add(_internetAccessBrowser);

            _internetAccessBrowser.TouchDown += _internetAccessBrowser_TouchDown;

            _internetAccessBrowser.TouchMove += _internetAccessBrowser_TouchMove;

            _internetAccessBrowser.MouseDown += _internetAccessBrowser_MouseDown;

            _internetAccessBrowser.RequestContext = new RequestContext();
          //  _internetAccessBrowser.IsManipulationEnabled = true;

            _internetAccessBrowser.ManipulationDelta += _internetAccessBrowser_ManipulationDelta;
            _internetAccessBrowser.Focus();

            this.RemainingTime = "30";

            this.OpenKeyboard = true;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += TimerTick;
            timer.Start();

            base.OnViewLoaded(view);
        }

        private void _internetAccessBrowser_IsBrowserInitializedChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            var browser = sender as ChromiumWebBrowser;
            browser.ExecuteScriptAsync(@"window.print=function() {alert('hello')}");
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
                this.events.PublishOnCurrentThread(new ViewEnglishCommand());
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

        public string RemainingTime
        {
            set
            {
                this.remainingTime = value;
                NotifyOfPropertyChange(() => this.remainingTime);
            }
            get
            {
                return this.remainingTime +"'";
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

            TranslateTransform transform = new TranslateTransform(x, y);

            _internetAccessBrowser.SendMouseWheelEvent((int)_internetAccessBrowser.Width, (int)_internetAccessBrowser.Height, deltax, deltay, CefEventFlags.None);

         /*   _internetAccessBrowser.SendMouseWheelEvent(
                        lastMousePositionX,
                        lastMousePositionY,
                        deltaX: (int)transform.X,
                        deltaY: (int)transform.Y,
                        modifiers: CefEventFlags.None);*/

        }

        private void _internetAccessBrowser_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            lastMousePositionX = (int)e.GetTouchPoint(_internetAccessBrowser).Position.X;
            lastMousePositionY = (int)e.GetTouchPoint(_internetAccessBrowser).Position.Y;
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
