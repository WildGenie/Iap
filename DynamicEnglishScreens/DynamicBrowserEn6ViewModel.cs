using System;
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

namespace Iap.DynamicEnglishScreens
{
   public class DynamicBrowserEn6ViewModel:Screen
    {
        private readonly IEventAggregator events;

        private BitmapImage leftImage1;
        private BitmapImage leftImage2;
        private BitmapImage leftImage3;
        private BitmapImage leftImage4;
        private BitmapImage leftImage5;
        private BitmapImage leftImage6;

        public static ChromiumWebBrowser _internetAccessBrowser;
        private DynamicBrowserEn6View currentView;

        public DynamicBrowserEn6ViewModel(IEventAggregator events)
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

            PopulatePanel(currentView);



            ((DynamicBrowserEn6View)view).DynamicBrowser.Children.Add(_internetAccessBrowser);

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

        }

        public void Back()
        {
            //  this.OpenKeyboard = false;
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

        public void ViewRedirect5()
        {
            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "5";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            _internetAccessBrowser.Load(this.ButtonsDetails[4].EnUrl);
        }

        public void ViewRedirect6()
        {
            this.PreviousSelected = this.SelectedPosition;
            this.SelectedPosition = "6";
            NotifyOfPropertyChange(() => SelectedPosition);
            PopulatePanel(currentView);
            _internetAccessBrowser.Load(this.ButtonsDetails[5].EnUrl);
        }
    }
}
