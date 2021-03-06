﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Windows.Media.Imaging;
using CefSharp.Wpf;
using Iap.Models;
using CefSharp;
using Iap.Commands;
using System.Windows;
using System.Windows.Threading;
using Iap.Unitilities;
using System.Windows.Media;
using Iap.Handlers;
using Iap.Bounds;
using System.Windows.Input;
using System.Windows.Controls;
using Iap.Services;

namespace Iap.DynamicEnglishScreens
{
   public class DynamicBrowserEn6ViewModel:Screen
    {
        private readonly IEventAggregator events;
        private readonly string numberOfAvailablePagesToPrint;
        private readonly ILog log;
        private readonly ISendStatsService sender;

        private BitmapImage leftImage1;
        private BitmapImage leftImage2;
        private BitmapImage leftImage3;
        private BitmapImage leftImage4;
        private BitmapImage leftImage5;
        private BitmapImage leftImage6;

        public static ChromiumWebBrowser _internetAccessBrowser;
        private DynamicBrowserEn6View currentView;

        private bool openKeyboard;
        private string remainingTime;

        private int TimeElapsed = 30;
        private DispatcherTimer timer;

       

        public DynamicBrowserEn6ViewModel(IEventAggregator events, string numberOfAvailablePagesToPrint, ILog log, ISendStatsService sender)
        {
            this.events = events;
            this.numberOfAvailablePagesToPrint = numberOfAvailablePagesToPrint;
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
            currentView = ((DynamicBrowserEn6View)view);

            _internetAccessBrowser = new ChromiumWebBrowser()
            {
                Address = this.HomeUrl
            };

            _internetAccessBrowser.BrowserSettings = new CefSharp.BrowserSettings()
            {
                OffScreenTransparentBackground = false,
            };

            _internetAccessBrowser.Load(this.HomeUrl);

            _internetAccessBrowser.BrowserSettings.FileAccessFromFileUrls = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.WebSecurity = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.Javascript = CefState.Enabled;


            _internetAccessBrowser.RequestContext = new RequestContext();
            _internetAccessBrowser.LifeSpanHandler = new LifeSpanHandler();
            // _internetAccessBrowser.RequestHandler = new DynamicBrowserRequestHandler();
            _internetAccessBrowser.RequestHandler = new CustomRequestHandler("",log,sender,this.numberOfAvailablePagesToPrint,events);
            _internetAccessBrowser.DialogHandler = new CustomDialogHandler();

            _internetAccessBrowser.MenuHandler = new CustomMenuHandler();

            // var boundEnObject = new DynamicBrowserBoundObjectEn(this.numberOfAvailablePagesToPrint);
            var boundEnObject = new CustomBoundObject(this.numberOfAvailablePagesToPrint, this.log,sender,events);
            _internetAccessBrowser.RegisterJsObject("bound", boundEnObject);
            _internetAccessBrowser.FrameLoadEnd += boundEnObject.OnFrameLoadEnd;

            PopulatePanel(currentView);

            if (this.SelectedPosition == "8" || this.SelectedPosition == "7" || this.SelectedPosition == "6" || this.SelectedPosition == "5")
            {
                currentView.scroller.ScrollToEnd();
            }

            currentView.scroller.PreviewMouseDown += Scroller_PreviewMouseDown;
            currentView.scroller.PreviewMouseMove += Scroller_PreviewMouseMove;
            currentView.scroller.PreviewMouseUp += Scroller_PreviewMouseUp;

            ((DynamicBrowserEn6View)view).DynamicBrowser.Children.Add(_internetAccessBrowser);

            _internetAccessBrowser.MouseDown += _internetAccessBrowser_MouseDown;
            _internetAccessBrowser.TouchDown += _internetAccessBrowser_TouchDown;
            _internetAccessBrowser.TouchMove += _internetAccessBrowser_TouchMove;

            _internetAccessBrowser.PreviewMouseUp += _internetAccessBrowser_PreviewMouseUp;

            _internetAccessBrowser.Focus();

            this.RemainingTime = "30";

            //this.OpenKeyboard = true;

            this.TimeElapsed = 30;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Tick += TimerTick;
            timer.Start();

            startTime = DateTime.Now;
            UnitStartTime = DateTime.Now;

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

        private Point scrollTarget;

        private Point scrollStartPoint;

        private Point scrollStartOffset;

        private bool drag;

        private void Scroller_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ScrollViewer scrollViewer = sender as ScrollViewer;
                if (drag)
                {
                    e.Handled = true;
                }

                drag = false;
            }
            catch { }
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

        public int lastMousePositionX;
        public int lastMousePositionY;

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
            catch { }
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
            catch { }
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
          /*  try
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

        private void PopulatePanel(DynamicBrowserEn6View view)
        {
            switch (this.SelectedPosition)
            {
                case "1":


                    view.Row1.Height = new GridLength(293, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row5.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row6.Height = new GridLength(177, GridUnitType.Auto);

                    this.LeftImage1 = this.ButtonsDetails[0].SelectedEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].InternalEngImg;
                    this.LeftImage5 = this.ButtonsDetails[4].InternalEngImg;
                    this.LeftImage6 = this.ButtonsDetails[5].InternalEngImg;

                    break;

                case "2":

                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(293, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row5.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row6.Height = new GridLength(177, GridUnitType.Auto);

                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].SelectedEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].InternalEngImg;
                    this.LeftImage5 = this.ButtonsDetails[4].InternalEngImg;
                    this.LeftImage6 = this.ButtonsDetails[5].InternalEngImg;

                    break;
                case "3":

                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(293, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row5.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row6.Height = new GridLength(177, GridUnitType.Auto);

                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].SelectedEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].InternalEngImg;
                    this.LeftImage5 = this.ButtonsDetails[4].InternalEngImg;
                    this.LeftImage6 = this.ButtonsDetails[5].InternalEngImg;

                    break;
                case "4":


                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(293, GridUnitType.Auto);
                    view.Row5.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row6.Height = new GridLength(177, GridUnitType.Auto);

                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].SelectedEngImg;
                    this.LeftImage5 = this.ButtonsDetails[4].InternalEngImg;
                    this.LeftImage6 = this.ButtonsDetails[5].InternalEngImg;

                    break;

                case "5":

                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row5.Height = new GridLength(293, GridUnitType.Auto);
                    view.Row6.Height = new GridLength(177, GridUnitType.Auto);

                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].InternalEngImg;
                    this.LeftImage5 = this.ButtonsDetails[4].SelectedEngImg;
                    this.LeftImage6 = this.ButtonsDetails[5].InternalEngImg;

                    break;

                case "6":
                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row5.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row6.Height = new GridLength(293, GridUnitType.Auto);

                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].InternalEngImg;
                    this.LeftImage5 = this.ButtonsDetails[4].InternalEngImg;
                    this.LeftImage6 = this.ButtonsDetails[5].SelectedEngImg;

                    break;
            }

            NotifyOfPropertyChange(() => LeftImage1);
            NotifyOfPropertyChange(() => LeftImage2);
            NotifyOfPropertyChange(() => LeftImage3);
            NotifyOfPropertyChange(() => LeftImage4);
            NotifyOfPropertyChange(() => LeftImage5);
            NotifyOfPropertyChange(() => LeftImage6);
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

        private void InitializeBrowserAgain(string url)
        {
            _internetAccessBrowser = new ChromiumWebBrowser()
            {
                Address = url
            };


            _internetAccessBrowser.BrowserSettings = new CefSharp.BrowserSettings()
            {
                OffScreenTransparentBackground = false,
            };

            _internetAccessBrowser.Load(url);

            _internetAccessBrowser.BrowserSettings.FileAccessFromFileUrls = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.WebSecurity = CefState.Enabled;
            _internetAccessBrowser.BrowserSettings.Javascript = CefState.Enabled;


            _internetAccessBrowser.RequestContext = new RequestContext();
            _internetAccessBrowser.LifeSpanHandler = new LifeSpanHandler();

            _internetAccessBrowser.RequestHandler = new CustomRequestHandler("", log, sender, this.numberOfAvailablePagesToPrint, events);
            _internetAccessBrowser.DialogHandler = new CustomDialogHandler();

            _internetAccessBrowser.MenuHandler = new CustomMenuHandler();


            _internetAccessBrowser.MouseDown += _internetAccessBrowser_MouseDown;
            _internetAccessBrowser.TouchDown += _internetAccessBrowser_TouchDown;
            _internetAccessBrowser.TouchMove += _internetAccessBrowser_TouchMove;

            _internetAccessBrowser.PreviewMouseUp += _internetAccessBrowser_PreviewMouseUp;

            currentView.DynamicBrowser.Children.Add(_internetAccessBrowser);

            _internetAccessBrowser.Focus();

            var boundEnObject = new CustomBoundObject(this.numberOfAvailablePagesToPrint, this.log, sender, events);
            _internetAccessBrowser.RegisterJsObject("bound", boundEnObject);
            _internetAccessBrowser.FrameLoadEnd += boundEnObject.OnFrameLoadEnd;
        }

        public void ViewRedirect1()
        {
            try
            {
                this.log.Info("Invoking Action: ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                this.sender.SendAction("ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                this.UnitStartTime = DateTime.Now;
            }
            catch { }

            this.log.Info("Invoking Action: View" + this.ButtonsDetails[0].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[0].Title + ".");
            }
            catch { }
            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "1";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            // _internetAccessBrowser.Load(this.ButtonsDetails[0].EnUrl);
            try
            {
                _internetAccessBrowser.Dispose();
            }
            catch { }
            InitializeBrowserAgain(this.ButtonsDetails[0].EnUrl);
        }

        public void ViewRedirect2()
        {
            try
            {
                this.log.Info("Invoking Action: ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                this.sender.SendAction("ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                this.UnitStartTime = DateTime.Now;
            }
            catch { }

            this.log.Info("Invoking Action: View" + this.ButtonsDetails[1].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[1].Title + ".");
            }
            catch { }
            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "2";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            //_internetAccessBrowser.Load(this.ButtonsDetails[1].EnUrl);
            try
            {
                _internetAccessBrowser.Dispose();
            }
            catch { }
            InitializeBrowserAgain(this.ButtonsDetails[1].EnUrl);
        }

        public void ViewRedirect3()
        {
            try
            {
                this.log.Info("Invoking Action: ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                this.sender.SendAction("ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                this.UnitStartTime = DateTime.Now;
            }
            catch { }

            this.log.Info("Invoking Action: View" + this.ButtonsDetails[2].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[2].Title + ".");
            }
            catch { }
            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "3";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            //   _internetAccessBrowser.Load(this.ButtonsDetails[2].EnUrl);
            try
            {
                _internetAccessBrowser.Dispose();
            }
            catch { }
            InitializeBrowserAgain(this.ButtonsDetails[2].EnUrl);
        }

        public void ViewRedirect4()
        {
            try
            {
                this.log.Info("Invoking Action: ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                this.sender.SendAction("ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                this.UnitStartTime = DateTime.Now;
            }
            catch { }

            this.log.Info("Invoking Action: View" + this.ButtonsDetails[3].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[3].Title + ".");
            }
            catch { }
            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "4";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            //_internetAccessBrowser.Load(this.ButtonsDetails[3].EnUrl);
            try
            {
                _internetAccessBrowser.Dispose();
            }
            catch { }
            InitializeBrowserAgain(this.ButtonsDetails[3].EnUrl);
        }

        public void ViewRedirect5()
        {
            try
            {
                this.log.Info("Invoking Action: ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                this.sender.SendAction("ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                this.UnitStartTime = DateTime.Now;
            }
            catch { }

            this.log.Info("Invoking Action: View" + this.ButtonsDetails[4].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[4].Title + ".");
            }
            catch { }
            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "5";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            // _internetAccessBrowser.Load(this.ButtonsDetails[4].EnUrl);
            try
            {
                _internetAccessBrowser.Dispose();
            }
            catch { }
            InitializeBrowserAgain(this.ButtonsDetails[4].EnUrl);
        }

        public void ViewRedirect6()
        {
            try
            {
                this.log.Info("Invoking Action: ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                this.sender.SendAction("ViewClose " + this.ButtonsDetails[Int32.Parse(this.SelectedPosition) - 1].Title + " after " + CalculateUnitSession() + " time.");
                this.UnitStartTime = DateTime.Now;
            }
            catch { }

            this.log.Info("Invoking Action: View" + this.ButtonsDetails[5].Title + ".");
            try
            {
                this.sender.SendAction("View" + this.ButtonsDetails[5].Title + ".");
            }
            catch { }
            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "6";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            // _internetAccessBrowser.Load(this.ButtonsDetails[5].EnUrl);
            try
            {
                _internetAccessBrowser.Dispose();
            }
            catch { }
            InitializeBrowserAgain(this.ButtonsDetails[5].EnUrl);
        }
    }
}
