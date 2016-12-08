using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Iap.Models;
using Iap.Commands;
using System.Windows.Media.Imaging;
using CefSharp.Wpf;
using CefSharp;
using System.Windows;
using System.Windows.Threading;
using Iap.Unitilities;
using System.Windows.Media;
using Iap.Handlers;
using Iap.Bounds;
using System.Windows.Input;
using System.Windows.Controls;

namespace Iap.DynamicEnglishScreens
{
   public class DynamicBrowserEnViewModel:Screen
    {
        private readonly IEventAggregator events;
        private readonly string numberOfAvailablePagesToPrint;

        private BitmapImage leftImage1;
        private BitmapImage leftImage2;
        private BitmapImage leftImage3;
        private BitmapImage leftImage4;


        public static ChromiumWebBrowser _internetAccessBrowser;
        private DynamicBrowserEnView currentView;

        private bool openKeyboard;
        private string remainingTime;

        private int TimeElapsed = 30;
        private DispatcherTimer timer;

        

        public DynamicBrowserEnViewModel(IEventAggregator events, string numberOfAvailablePagesToPrint)
        {
            this.events = events;
            this.numberOfAvailablePagesToPrint = numberOfAvailablePagesToPrint;
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
                NotifyOfPropertyChange(() =>LeftImage1);
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

        

       

        public string SelectedPosition
        {
            get;
            set;
        }

        public string PreviousSelected
        {
            get;set;
        }

        protected override void OnViewLoaded(object view)
        {
            currentView = ((DynamicBrowserEnView)view);

           

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
            _internetAccessBrowser.RequestHandler = new CustomRequestHandler("");
            _internetAccessBrowser.DialogHandler = new CustomDialogHandler();
            // var boundEnObject = new DynamicBrowserBoundObjectEn(this.numberOfAvailablePagesToPrint);
            var boundEnObject = new CustomBoundObject(this.numberOfAvailablePagesToPrint);
            _internetAccessBrowser.RegisterJsObject("bound", boundEnObject);
            _internetAccessBrowser.FrameLoadEnd += boundEnObject.OnFrameLoadEnd;

            _internetAccessBrowser.MouseDown += _internetAccessBrowser_MouseDown;
            _internetAccessBrowser.TouchDown += _internetAccessBrowser_TouchDown;
            _internetAccessBrowser.TouchMove += _internetAccessBrowser_TouchMove;

            PopulatePanel(currentView);            

            ((DynamicBrowserEnView)view).DynamicBrowser.Children.Add(_internetAccessBrowser);

           // _internetAccessBrowser.Focus();

            this.RemainingTime = "30";

           // this.OpenKeyboard = true;

            this.TimeElapsed = 30;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 1, 0);
            timer.Tick += TimerTick;
            timer.Start();

            base.OnViewLoaded(view);
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

        public int lastMousePositionX;
        public int lastMousePositionY;

        private TouchDevice windowTouchDevice;
        private System.Windows.Point lastPoint;

        private void _internetAccessBrowser_TouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {
            /* int x = (int)e.GetTouchPoint(_internetAccessBrowser).Position.X;
             int y = (int)e.GetTouchPoint(_internetAccessBrowser).Position.Y;


             int deltax = x - lastMousePositionX;
             int deltay = y - lastMousePositionY;

             TranslateTransform transform = new TranslateTransform(x, y);

             _internetAccessBrowser.SendMouseWheelEvent((int)_internetAccessBrowser.Width, (int)_internetAccessBrowser.Height, deltax, deltay, CefEventFlags.None);
             */
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
            // lastMousePositionX = (int)e.GetTouchPoint(_internetAccessBrowser).Position.X;
            //lastMousePositionY = (int)e.GetTouchPoint(_internetAccessBrowser).Position.Y;

            
                Control control = (Control)sender;
                e.TouchDevice.Capture(control);
                windowTouchDevice = e.TouchDevice;
                var currentTouchPoint = windowTouchDevice.GetTouchPoint(null);


                var locationOnScreen = control.PointToScreen(new System.Windows.Point(currentTouchPoint.Position.X, currentTouchPoint.Position.Y));
                lastPoint = locationOnScreen;
            
        }

        private void PopulatePanel(DynamicBrowserEnView view)
        {
            switch (this.SelectedPosition)
            {
                case "1":
                    

                    view.Row1.Height = new GridLength(293, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(177, GridUnitType.Auto);

                    this.LeftImage1 = this.ButtonsDetails[0].SelectedEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].InternalEngImg;


                    break;

                case "2":

                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(293, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(177, GridUnitType.Auto);

                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].SelectedEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].InternalEngImg;

                    

                    break;
                case "3":

                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(293, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(177, GridUnitType.Auto);

                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].SelectedEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].InternalEngImg;

                    break;
                case "4":
    

                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row4.Height = new GridLength(293, GridUnitType.Auto);


                    this.LeftImage1 = this.ButtonsDetails[0].InternalEngImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalEngImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalEngImg;
                    this.LeftImage4 = this.ButtonsDetails[3].SelectedEngImg;                 

                    break;                    

            }

            NotifyOfPropertyChange(() => LeftImage1);
            NotifyOfPropertyChange(() => LeftImage2);
            NotifyOfPropertyChange(() => LeftImage3);
            NotifyOfPropertyChange(() => LeftImage4);

        }

        public void Back()
        {
            /*  this.OpenKeyboard = false;
              if (_internetAccessBrowser.GetMainFrame().Url.Contains(".pdf"))
              {
                  try
                  {
                      if (_internetAccessBrowser != null)
                      {
                          _internetAccessBrowser.Dispose();
                      }
                      this.events.PublishOnCurrentThread(new ViewDynamicEnglishShellCommand());
                  }

                  catch { }
              }
              else
              {
                  try
                  {
                      if (_internetAccessBrowser.CanGoBack && (this.PreviousSelected == this.SelectedPosition))
                      {
                          _internetAccessBrowser.Back();
                      }
                      else
                      {
                          if (_internetAccessBrowser != null)
                          {
                              _internetAccessBrowser.Dispose();
                          }
                          this.events.PublishOnCurrentThread(new ViewDynamicEnglishShellCommand());
                      }
                  }
                  catch { }
              }*/
            try
            {
                if (_internetAccessBrowser != null)
                {
                    _internetAccessBrowser.Dispose();
                }
                this.events.PublishOnCurrentThread(new ViewDynamicEnglishShellCommand());
            }
            catch {
            }
        }

        public void ViewRedirect1()
        {
            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "1";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            _internetAccessBrowser.Load(this.ButtonsDetails[0].EnUrl);
        }

        public void ViewRedirect2()
        {

            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "2";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            _internetAccessBrowser.Load(this.ButtonsDetails[1].EnUrl);
        }

        public void ViewRedirect3()
        {
            
            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "3";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            _internetAccessBrowser.Load(this.ButtonsDetails[2].EnUrl);
        }

        public void ViewRedirect4()
        {
            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "4";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            _internetAccessBrowser.Load(this.ButtonsDetails[3].EnUrl);
        }
    }
}
