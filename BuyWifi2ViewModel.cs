using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using CefSharp.Wpf;
using System.Windows.Threading;
using Iap.Bounds;
using Iap.Handlers;
using CefSharp;
using Iap.Commands;
using Iap.Services;

namespace Iap
{
   public class BuyWifi2ViewModel:Screen
    {
        private readonly IEventAggregator events;

        private string remainingTime;
        private bool openKeyboard;
        private readonly string numberOfAvailablePagesToPrint;
        private readonly string buyWifiEnApi;
        private readonly ILog log;
        private readonly ISendStatsService sender;

        public static ChromiumWebBrowser _buyWifiBrowser;

        private DispatcherTimer timer;

        public BuyWifi2ViewModel(IEventAggregator events, string numberOfAvailablePagesToPrint, string buyWifiEnApi, ILog log, ISendStatsService sender)
        {
            this.events = events;
            this.numberOfAvailablePagesToPrint = numberOfAvailablePagesToPrint;
            this.buyWifiEnApi = buyWifiEnApi;
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
                Address = this.buyWifiEnApi,
            };

            _buyWifiBrowser.BrowserSettings = new CefSharp.BrowserSettings()
            {
                OffScreenTransparentBackground = false,
            };

            _buyWifiBrowser.Load(this.buyWifiEnApi);

            _buyWifiBrowser.LifeSpanHandler = new LifeSpanHandler();
            // _buyWifiBrowser.RequestHandler = new RequestHandler(Convert.ToInt32(numberOfAvailablePagesToPrint));
            _buyWifiBrowser.RequestHandler = new CustomRequestHandler("");
            _buyWifiBrowser.DialogHandler = new CustomDialogHandler();

            ((BuyWifi2View)view).BuyWifiBrowser.Children.Add(_buyWifiBrowser);

            _buyWifiBrowser.TouchDown += _buyWifiBrowser_TouchDown;

            _buyWifiBrowser.TouchMove += _buyWifiBrowser_TouchMove;

            _buyWifiBrowser.MouseDown += _buyWifiBrowser_MouseDown;

            _buyWifiBrowser.RequestContext = new RequestContext();

            _buyWifiBrowser.Focus();


            var obj = new CustomBoundObject(this.numberOfAvailablePagesToPrint, this.log);
            _buyWifiBrowser.RegisterJsObject("bound", obj);
            _buyWifiBrowser.FrameLoadEnd += obj.OnFrameLoadEnd;


            GlobalCounters.ResetAll();


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
                return this.remainingTime + "'";
            }
        }

        public int lastMousePositionX;
        public int lastMousePositionY;

        private void _buyWifiBrowser_TouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {
            int x = (int)e.GetTouchPoint(_buyWifiBrowser).Position.X;
            int y = (int)e.GetTouchPoint(_buyWifiBrowser).Position.Y;


            int deltax = x - lastMousePositionX;
            int deltay = y - lastMousePositionY;

            _buyWifiBrowser.SendMouseWheelEvent((int)_buyWifiBrowser.Width, (int)_buyWifiBrowser.Height, deltax, deltay, CefEventFlags.None);
        }

        private void _buyWifiBrowser_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            lastMousePositionX = (int)e.GetTouchPoint(_buyWifiBrowser).Position.X;
            lastMousePositionY = (int)e.GetTouchPoint(_buyWifiBrowser).Position.Y;
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
                this.events.PublishOnCurrentThread(new ViewTwoButtonsShellEnCommand());
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

        

        public void ViewInternetAccess()
        {
            this.events.PublishOnCurrentThread(new ViewInternetAccess2Command(this.TimeElapsed.ToString()));
            this.sender.SendAction("ViewInternetAccess.");
        }

       

        public void ViewBuyWifi()
        {
            _buyWifiBrowser.Load(this.buyWifiEnApi);
            this.sender.SendAction("ViewBuyWifi.");
        }
    }
}
