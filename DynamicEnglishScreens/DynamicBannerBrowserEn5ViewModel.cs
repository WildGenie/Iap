using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Windows.Media.Imaging;
using CefSharp.Wpf;
using System.Windows.Threading;
using CefSharp;
using Iap.Handlers;
using Iap.Bounds;
using Iap.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Iap.Models;
using Iap.Services;

namespace Iap.DynamicEnglishScreens
{
   public class DynamicBannerBrowserEn5ViewModel:Screen
    {
        private readonly IEventAggregator events;
        private readonly string numberOfAvailablePagesToPrint;
        private readonly string bannerLinkEnApi;
        private readonly ILog log;
        private readonly ISendStatsService sender;

        private BitmapImage leftImage1;
        private BitmapImage leftImage2;
        private BitmapImage leftImage3;
        private BitmapImage leftImage4;
        private BitmapImage leftImage5;
        private BitmapImage leftImage6;
        private BitmapImage leftImage7;
        private BitmapImage leftImage8;

        public static ChromiumWebBrowser _internetAccessBrowser;
        private DynamicBannerBrowserEn5View currentView;

        private bool openKeyboard;
        private string remainingTime;

        private int TimeElapsed = 30;
        private DispatcherTimer timer;

        public DynamicBannerBrowserEn5ViewModel(
                        IEventAggregator events,
                        string numberOfAvailablePagesToPrint,
                        string bannerLinkEnApi,
                        ILog log,
                        ISendStatsService sender
        )
        {
            this.events = events;
            this.numberOfAvailablePagesToPrint = numberOfAvailablePagesToPrint;
            this.bannerLinkEnApi = bannerLinkEnApi;
            this.log = log;
            this.sender = sender;
        }

        public IEventAggregator Events
        {
            get { return this.events; }
        }

        public List<ButtonLinkModel> ButtonsDetails
        {
            get;
            set;
        }

        public string AdLinkEN
            { get; set; }

        public string HomeUrl
        {
            get;
            set;
        }

        public BitmapImage LeftImage1
        {
            set
            {
                this.leftImage1 = value;
                NotifyOfPropertyChange(() => LeftImage1);
            }
            get
            {
                return this.leftImage1;
            }
        }

        public BitmapImage LeftImage2
        {
            set
            {
                this.leftImage2 = value;
                NotifyOfPropertyChange(() => LeftImage2);
            }
            get
            {
                return this.leftImage2;
            }
        }

        public BitmapImage LeftImage3
        {
            set
            {
                this.leftImage3 = value;
                NotifyOfPropertyChange(() => LeftImage3);
            }
            get
            {
                return this.leftImage3;
            }
        }

        public BitmapImage LeftImage4
        {
            set
            {
                this.leftImage4 = value;
                NotifyOfPropertyChange(() => LeftImage4);
            }
            get
            {
                return this.leftImage4;
            }
        }

        public BitmapImage LeftImage5
        {
            set
            {
                this.leftImage5 = value;
                NotifyOfPropertyChange(() => LeftImage5);
            }
            get
            {
                return this.leftImage5;
            }
        }

        public BitmapImage LeftImage6
        {
            set
            {
                this.leftImage6 = value;
                NotifyOfPropertyChange(() => LeftImage6);
            }
            get
            {
                return this.leftImage6;
            }
        }

        public BitmapImage LeftImage7
        {
            set
            {
                this.leftImage7 = value;
                NotifyOfPropertyChange(() => LeftImage7);
            }
            get
            {
                return this.leftImage7;
            }
        }

        public BitmapImage LeftImage8
        {
            set
            {
                this.leftImage8 = value;
                NotifyOfPropertyChange(() => LeftImage8);
            }
            get
            {
                return this.leftImage8;
            }
        }

        public string SelectedPosition
        {
            get;
            set;
        }

        public string PreviousSelected
        {
            get; set;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            currentView = ((DynamicBannerBrowserEn5View)view);

            _internetAccessBrowser = new ChromiumWebBrowser()
            {
                Address = this.bannerLinkEnApi
            };

            _internetAccessBrowser.BrowserSettings = new CefSharp.BrowserSettings()
            {
                OffScreenTransparentBackground = false,
            };

            if (string.IsNullOrEmpty(this.AdLinkEN))
            {
                _internetAccessBrowser.Load(this.bannerLinkEnApi);
            }
            else
            {
                _internetAccessBrowser.Load(this.AdLinkEN);
            }

            _internetAccessBrowser.BrowserSettings.FileAccessFromFileUrls = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.WebSecurity = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.Javascript = CefState.Enabled;


            _internetAccessBrowser.RequestContext = new RequestContext();
            _internetAccessBrowser.LifeSpanHandler = new LifeSpanHandler();
            
            _internetAccessBrowser.RequestHandler = new CustomRequestHandler("",log,sender,this.numberOfAvailablePagesToPrint,events);
            _internetAccessBrowser.DialogHandler = new CustomDialogHandler();
            
            var boundEnObject = new CustomBoundObject(this.numberOfAvailablePagesToPrint,log,sender,events);
            _internetAccessBrowser.RegisterJsObject("bound", boundEnObject);
            _internetAccessBrowser.FrameLoadEnd += boundEnObject.OnFrameLoadEnd;

            _internetAccessBrowser.MouseDown += _internetAccessBrowser_MouseDown;
            _internetAccessBrowser.TouchDown += _internetAccessBrowser_TouchDown;
            _internetAccessBrowser.TouchMove += _internetAccessBrowser_TouchMove;

            currentView.scroller.PreviewMouseDown += Scroller_PreviewMouseDown;
            currentView.scroller.PreviewMouseMove += Scroller_PreviewMouseMove;
            currentView.scroller.PreviewMouseUp += Scroller_PreviewMouseUp;

            _internetAccessBrowser.PreviewMouseUp += _internetAccessBrowser_PreviewMouseUp;

            ((DynamicBannerBrowserEn5View)view).DynamicBrowser.Children.Add(_internetAccessBrowser);


            PopulatePanel(currentView);

            _internetAccessBrowser.Focus();

            this.RemainingTime = "30";

            

            this.TimeElapsed = 30;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Tick += TimerTick;
            timer.Start();

            startTime = DateTime.Now;
            UnitStartTime = DateTime.Now;
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
                                if (activeElement.tagName.toLowerCase() === 'iframe') {
                                    var iframe = document.getElementById(activeElement.id);
                                    try{
                                    var doc = iframe.contentDocument? iframe.contentDocument: iframe.contentWindow.document;
                                        }
                                     catch(err)
                                        {
                                            isText=true; 
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

            e.Handled = false;
        }

        private TouchDevice windowTouchDevice;
        private System.Windows.Point lastPoint;

        private void _internetAccessBrowser_TouchMove(object sender, TouchEventArgs e)
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
            catch { }
        }

        private void _internetAccessBrowser_TouchDown(object sender, TouchEventArgs e)
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

        private Point scrollTarget;

        private Point scrollStartPoint;

        private Point scrollStartOffset;

        private bool drag;

        private void Scroller_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            if (drag)
            {
                e.Handled = true;
            }

            drag = false;
        }

        private void Scroller_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                ScrollViewer scrollViewer = sender as ScrollViewer;

                Point currentPoint = e.GetPosition(scrollViewer);

                Point delta = new Point(scrollStartPoint.X - currentPoint.X,
                    scrollStartPoint.Y - currentPoint.Y);

                scrollTarget.X = scrollStartOffset.X + delta.X;
                scrollTarget.Y = scrollStartOffset.Y + delta.Y;

                scrollViewer.ScrollToHorizontalOffset(scrollTarget.X);
                scrollViewer.ScrollToVerticalOffset(scrollTarget.Y);

                var moveTo = currentPoint.Y - scrollStartPoint.Y;
                if (Math.Abs(moveTo) > 1)
                {
                    drag = true;
                }
            }
            catch { }
        }

        private void Scroller_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ScrollViewer scrollViewer = sender as ScrollViewer;
                drag = false;
                scrollStartPoint = e.GetPosition(scrollViewer);
                scrollStartOffset.X = scrollViewer.HorizontalOffset;
                scrollStartOffset.Y = scrollViewer.VerticalOffset;
            }
            catch { }
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
                return this.remainingTime + "'";
            }
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
                this.events.PublishOnCurrentThread(new ViewDynamicEnglishShellCommand());
            }
        }

        private DateTime startTime;

        private string TimeSpended()
        {
            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime.Subtract(startTime);
            return duration.Minutes.ToString();
        }

        private DateTime UnitStartTime;

        private string CalculateUnitSession()
        {
            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime.Subtract(UnitStartTime);
            return duration.ToString(@"hh\:mm\:ss");
        }

        protected override void OnDeactivate(bool close)
        {
            timer.Stop();
           /* try
            {
                this.log.Info("Invoking Action: ViewEndSession after " + TimeSpended() + " time.");
                this.sender.SendAction("ViewEndSession after " + TimeSpended() + " time.");
            }
            catch { }*/
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

        private void PopulatePanel(DynamicBannerBrowserEn5View view)
        {

            switch (this.ButtonsDetails.Count)
            {

                case 2:
                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    
                    view.Row3.IsEnabled = false;
                    view.Row4.IsEnabled = false;
                    view.Row5.IsEnabled = false;
                    view.Row6.IsEnabled = false;
                    view.Row7.IsEnabled = false;
                    view.Row8.IsEnabled = false;

                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;

                    break;

                case 3:
                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                    
                    view.Row4.IsEnabled = false;
                    view.Row5.IsEnabled = false;
                    view.Row6.IsEnabled = false;
                    view.Row7.IsEnabled = false;
                    view.Row8.IsEnabled = false;

                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalEngImg;

                    break;

                case 4:

                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(177, GridUnitType.Auto);
                    
                    view.Row5.IsEnabled = false;
                    view.Row6.IsEnabled = false;
                    view.Row7.IsEnabled = false;
                    view.Row8.IsEnabled = false;

                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].InternalEngImg;
                   
                    break;

                case 5:
                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row5.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row6.IsEnabled = false;
                    view.Row7.IsEnabled = false;
                    view.Row8.IsEnabled = false;

                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].InternalEngImg;
                    this.LeftImage5 = this.ButtonsDetails[4].InternalEngImg;

                    break;

                case 6:
                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row5.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row6.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row7.IsEnabled = false;
                    view.Row8.IsEnabled = false;

                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].InternalEngImg;
                    this.LeftImage5 = this.ButtonsDetails[4].InternalEngImg;
                    this.LeftImage6 = this.ButtonsDetails[5].InternalEngImg;
                    break;

                case 7:
                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row5.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row6.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row7.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row8.IsEnabled = false;

                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].InternalEngImg;
                    this.LeftImage5 = this.ButtonsDetails[4].InternalEngImg;
                    this.LeftImage6 = this.ButtonsDetails[5].InternalEngImg;
                    this.LeftImage7 = this.ButtonsDetails[6].InternalEngImg;
                    break;

                case 8:
                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row5.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row6.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row7.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row8.Height = new GridLength(177, GridUnitType.Auto);

                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].InternalEngImg;
                    this.LeftImage5 = this.ButtonsDetails[4].InternalEngImg;
                    this.LeftImage6 = this.ButtonsDetails[5].InternalEngImg;
                    this.LeftImage7 = this.ButtonsDetails[6].InternalEngImg;
                    this.LeftImage8 = this.ButtonsDetails[7].InternalEngImg;
                    break;
            }

            NotifyOfPropertyChange(() => LeftImage1);
            NotifyOfPropertyChange(() => LeftImage2);
            NotifyOfPropertyChange(() => LeftImage3);
            NotifyOfPropertyChange(() => LeftImage4);
            NotifyOfPropertyChange(() => LeftImage5);
            NotifyOfPropertyChange(() => LeftImage6);
            NotifyOfPropertyChange(() => LeftImage7);
            NotifyOfPropertyChange(() => LeftImage8);
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
                        this.log.Info("Invoking Action: ViewEndNavigateSession after " + TimeSpended() + " minutes.");
                        this.sender.SendAction("ViewEndNavigateSession after " + TimeSpended() + " minutes.");
                    }

                    catch
                    { }
                    this.events.PublishOnCurrentThread(new ViewDynamicEnglishShellCommand());
                }
            }
            catch { }
        }

        private string TimeHasSpent()
        {
            int timeSpent = 30 - TimeElapsed;

            return timeSpent.ToString();
        }


        public void ViewRedirect1()
        {
            try
            {
                if (this.SelectedPosition == "banner")
                {
                    this.log.Info("Invoking Action: ViewClose BannerLink  after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose BannerLink after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                else
                {
                    this.log.Info("Invoking Action: ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                this.UnitStartTime = DateTime.Now;
            }
            catch { }

            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[0].EnUrl, this.ButtonsDetails, "1"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[0].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[0].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect2()
        {
            try
            {
                if (this.SelectedPosition == "banner")
                {
                    this.log.Info("Invoking Action: ViewClose BannerLink  after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose BannerLink after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                else
                {
                    this.log.Info("Invoking Action: ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                this.UnitStartTime = DateTime.Now;
            }
            catch { }

            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[1].EnUrl, this.ButtonsDetails, "2"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[1].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[1].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect3()
        {
            try
            {
                if (this.SelectedPosition == "banner")
                {
                    this.log.Info("Invoking Action: ViewClose BannerLink  after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose BannerLink after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                else
                {
                    this.log.Info("Invoking Action: ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                this.UnitStartTime = DateTime.Now;
            }
            catch { }

            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[2].EnUrl, this.ButtonsDetails, "3"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[2].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[2].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect4()
        {
            try
            {
                if (this.SelectedPosition == "banner")
                {
                    this.log.Info("Invoking Action: ViewClose BannerLink  after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose BannerLink after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                else
                {
                    this.log.Info("Invoking Action: ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                this.UnitStartTime = DateTime.Now;
            }
            catch { }

            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[3].EnUrl, this.ButtonsDetails, "4"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[3].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[3].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect5()
        {
            try
            {
                if (this.SelectedPosition == "banner")
                {
                    this.log.Info("Invoking Action: ViewClose BannerLink  after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose BannerLink after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                else
                {
                    this.log.Info("Invoking Action: ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                this.UnitStartTime = DateTime.Now;
            }
            catch { }

            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[4].EnUrl, this.ButtonsDetails, "5"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[4].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[4].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect6()
        {
            try
            {
                if (this.SelectedPosition == "banner")
                {
                    this.log.Info("Invoking Action: ViewClose BannerLink  after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose BannerLink after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                else
                {
                    this.log.Info("Invoking Action: ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                this.UnitStartTime = DateTime.Now;
            }
            catch { }

            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[5].EnUrl, this.ButtonsDetails, "6"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[5].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[5].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect7()
        {
            try
            {
                if (this.SelectedPosition == "banner")
                {
                    this.log.Info("Invoking Action: ViewClose BannerLink  after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose BannerLink after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                else
                {
                    this.log.Info("Invoking Action: ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                this.UnitStartTime = DateTime.Now;
            }
            catch { }

            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[6].EnUrl, this.ButtonsDetails, "7"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[6].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[6].Title + ".");
            }
            catch { }
        }

        public void ViewRedirect8()
        {
            try
            {
                if (this.SelectedPosition == "banner")
                {
                    this.log.Info("Invoking Action: ViewClose BannerLink  after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose BannerLink after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                else
                {
                    this.log.Info("Invoking Action: ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    try
                    {
                        this.sender.SendAction("ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                    }
                    catch { }
                }
                this.UnitStartTime = DateTime.Now;
            }
            catch { }

            this.events.PublishOnBackgroundThread(new ViewRedirectToBrowserCommand("", this.ButtonsDetails[7].EnUrl, this.ButtonsDetails, "8"));
            this.log.Info("Invoking Action: View" + this.ButtonsDetails[7].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[7].Title + ".");
            }
            catch { }
        }
    }
}
