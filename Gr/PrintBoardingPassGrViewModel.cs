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
   public class PrintBoardingPassGrViewModel:Screen
    {
        private IEventAggregator events;
        private readonly string boardingPassGrApi;
        private bool openKeyboard;
        private readonly string numberOfAvailablePagesToPrint;
        private readonly ILog log;
        private readonly ISendStatsService sender;

        private string remainingTime;

        public static ChromiumWebBrowser _printBoardingPassBrowser;

        
        private DispatcherTimer timer;

        public PrintBoardingPassGrViewModel(IEventAggregator events, string boardingPassGrApi, string numberOfAvailablePagesToPrint, ILog log, ISendStatsService sender)
        {
            this.events = events;
            this.boardingPassGrApi = boardingPassGrApi;
            this.numberOfAvailablePagesToPrint = numberOfAvailablePagesToPrint;
            this.log = log;
            this.sender = sender;
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

            var obj = new CustomBoundObjectEl(this.numberOfAvailablePagesToPrint,this.log,sender);

            _printBoardingPassBrowser.RegisterJsObject("bound", obj);
            _printBoardingPassBrowser.FrameLoadEnd += obj.OnFrameLoadEnd;

            ((PrintBoardingPassGrView)view).InternetAccessBrowser.Children.Add(_printBoardingPassBrowser);

            _printBoardingPassBrowser.LifeSpanHandler = new LifeSpanHandler();
            _printBoardingPassBrowser.RequestHandler = new CustomRequestHandler("",log,sender,this.numberOfAvailablePagesToPrint);
          //  _printBoardingPassBrowser.RequestHandler = new RequestHandler(Convert.ToInt32(this.numberOfAvailablePagesToPrint));
            _printBoardingPassBrowser.RequestContext = new RequestContext();
            _printBoardingPassBrowser.DialogHandler = new CustomDialogHandler();
            _printBoardingPassBrowser.MenuHandler = new CustomMenuHandler();

            _printBoardingPassBrowser.TouchDown += _printBoardingPassBrowser_TouchDown;

            _printBoardingPassBrowser.TouchMove += _printBoardingPassBrowser_TouchMove;

            _printBoardingPassBrowser.MouseDown += _printBoardingPassBrowser_MouseDown;
          
            _printBoardingPassBrowser.Focus();


            
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

        private void _printBoardingPassBrowser_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

                var task = _printBoardingPassBrowser.EvaluateScriptAsync(script, TimeSpan.FromSeconds(10));
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

        private void _printBoardingPassBrowser_TouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {
            try
            {
                Control control = (Control)sender;

                var currentTouchPoint = windowTouchDevice.GetTouchPoint(null);

                var locationOnScreen = control.PointToScreen(new System.Windows.Point(currentTouchPoint.Position.X, currentTouchPoint.Position.Y));

                var deltaX = locationOnScreen.X - lastPoint.X;
                var deltaY = locationOnScreen.Y - lastPoint.Y;

                lastPoint = locationOnScreen;

                _printBoardingPassBrowser.SendMouseWheelEvent((int)lastPoint.X, (int)lastPoint.Y, (int)deltaX, (int)deltaY, CefEventFlags.None);
            }
            catch { }
        }

        private void _printBoardingPassBrowser_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
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
            catch { }
        }

        public void Back()
        {
            this.OpenKeyboard = false;
            try
            {
                try
                {
                    if (_printBoardingPassBrowser != null)
                    {
                        _printBoardingPassBrowser.Dispose();
                    }
                }
                catch { }

                this.events.PublishOnCurrentThread(new ViewGreekCommand());
            }

            catch { }

            try
            {
                this.log.Info("Invoking Action: ViewEndNavigateSession  after " + (30 - this.TimeElapsed).ToString() + " minutes.");
                this.sender.SendAction("ViewEndNavigateSession after " + (30 - this.TimeElapsed).ToString() + " minutes.");
            }

            catch
            { }
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
                this.log.Info("Invoking Action: ViewClose PrintBoardingPass after " + TimeSpended() + " time.");
                this.sender.SendAction("ViewClose PrintBoardingPass after " + TimeSpended() + " time.");
            }
            catch { }
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
            this.events.PublishOnCurrentThread(new ViewBuyWifiCommand(this.TimeElapsed.ToString()));
            try
            {
                this.sender.SendAction("ViewBuyWifi.");
            }
            catch { }
        }

        public void ViewInternetAccess()
        {
            this.events.PublishOnCurrentThread(new ViewInternetAccessCommand(this.TimeElapsed.ToString()));
            try
            {
                this.sender.SendAction("ViewInternetAccess.");
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

        public void ViewPrintBoardingPass()
        {
            _printBoardingPassBrowser.Load(this.boardingPassGrApi);
            try
            {
                this.sender.SendAction("ViewPrintBoardingPass.");
            }
            catch { }
        }
    }
}
