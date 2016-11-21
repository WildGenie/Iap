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

namespace Iap.DynamicEnglishScreens
{
   public class DynamicBrowserEnViewModel:Screen
    {
        private readonly IEventAggregator events;

        private BitmapImage leftImage1;
        private BitmapImage leftImage2;
        private BitmapImage leftImage3;
        private BitmapImage leftImage4;


        public static ChromiumWebBrowser _internetAccessBrowser;
        private DynamicBrowserEnView currentView;

        public DynamicBrowserEnViewModel(IEventAggregator events)
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

            PopulatePanel(currentView);

            

            ((DynamicBrowserEnView)view).DynamicBrowser.Children.Add(_internetAccessBrowser);

            base.OnViewLoaded(view);
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
          //  this.OpenKeyboard = false;
            try
            {
                if (_internetAccessBrowser.CanGoBack && this.PreviousSelected==this.SelectedPosition)
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
