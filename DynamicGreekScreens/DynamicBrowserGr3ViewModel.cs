﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Windows.Media.Imaging;
using CefSharp.Wpf;
using System.Windows.Threading;
using Iap.Unitilities;
using Iap.Models;
using CefSharp;
using Iap.Handlers;
using Iap.Commands;
using System.Windows;
using System.Windows.Media;
using Iap.Bounds;

namespace Iap.DynamicGreekScreens
{
   public class DynamicBrowserGr3ViewModel:Screen
    {
        private readonly IEventAggregator events;

        private BitmapImage leftImage1;
        private BitmapImage leftImage2;
        private BitmapImage leftImage3;

        public static ChromiumWebBrowser _internetAccessBrowser;
        private DynamicBrowserGr3View currentView;

        private bool openKeyboard;
        private string remainingTime;

        private int TimeElapsed = 30;
        private DispatcherTimer timer;

        public IdleInputBrowserViewModel IdleInputBrowser { get; set; }

        public DynamicBrowserGr3ViewModel(IEventAggregator events)
        {
            this.events = events;
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
            currentView = ((DynamicBrowserGr3View)view);

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
            _internetAccessBrowser.RequestHandler = new DynamicBrowserRequestHandler();
            var boundGrObject = new DynamicBrowserBoundObjectGr();
            _internetAccessBrowser.RegisterJsObject("bound", boundGrObject);
            _internetAccessBrowser.FrameLoadEnd += boundGrObject.OnFrameLoadEnd;

            PopulatePanel(currentView);

            ((DynamicBrowserGr3View)view).DynamicBrowser.Children.Add(_internetAccessBrowser);

            _internetAccessBrowser.MouseDown += _internetAccessBrowser_MouseDown;
            _internetAccessBrowser.TouchDown += _internetAccessBrowser_TouchDown;
            _internetAccessBrowser.TouchMove += _internetAccessBrowser_TouchMove;

            _internetAccessBrowser.Focus();

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
                this.events.PublishOnCurrentThread(new ViewDynamicGreekShellCommand());
            }
        }

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

        }

        private void _internetAccessBrowser_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            lastMousePositionX = (int)e.GetTouchPoint(_internetAccessBrowser).Position.X;
            lastMousePositionY = (int)e.GetTouchPoint(_internetAccessBrowser).Position.Y;
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

        private void PopulatePanel(DynamicBrowserGr3View view)
        {
            switch (this.SelectedPosition)
            {
                case "1":


                    view.Row1.Height = new GridLength(293, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);

                    this.LeftImage1 = this.ButtonsDetails[0].SelectedGrImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalGrImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalGrImg;
                    


                    break;

                case "2":

                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(293, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(177, GridUnitType.Auto);
                   

                    this.LeftImage1 = this.ButtonsDetails[0].InternalGrImg;
                    this.LeftImage2 = this.ButtonsDetails[1].SelectedGrImg;
                    this.LeftImage3 = this.ButtonsDetails[2].InternalGrImg;
                    



                    break;
                case "3":

                    view.Row1.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row2.Height = new GridLength(177, GridUnitType.Auto);
                    view.Row3.Height = new GridLength(293, GridUnitType.Auto);
                   

                    this.LeftImage1 = this.ButtonsDetails[0].InternalGrImg;
                    this.LeftImage2 = this.ButtonsDetails[1].InternalGrImg;
                    this.LeftImage3 = this.ButtonsDetails[2].SelectedGrImg;
                    

                    break;
               

            }

            NotifyOfPropertyChange(() => LeftImage1);
            NotifyOfPropertyChange(() => LeftImage2);
            NotifyOfPropertyChange(() => LeftImage3);
          
        }

        public void Back()
        {
            this.OpenKeyboard = false;
            try
            {
                if (_internetAccessBrowser.CanGoBack && this.PreviousSelected == this.SelectedPosition)
                {
                    _internetAccessBrowser.Back();
                }
                else
                {
                    if (_internetAccessBrowser != null)
                    {
                        _internetAccessBrowser.Dispose();
                    }
                    this.events.PublishOnCurrentThread(new ViewDynamicGreekShellCommand());
                }
            }
            catch { }
        }

        public void ViewRedirect1()
        {
            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "1";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            _internetAccessBrowser.Load(this.ButtonsDetails[0].GrUrl);
        }

        public void ViewRedirect2()
        {

            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "2";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            _internetAccessBrowser.Load(this.ButtonsDetails[1].GrUrl);
        }

        public void ViewRedirect3()
        {

            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "3";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            _internetAccessBrowser.Load(this.ButtonsDetails[2].GrUrl);
        }
    }
}