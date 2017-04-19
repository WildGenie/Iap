using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Iap.Commands;
using Iap.Gr;
using System.Windows;
using System.Windows.Input;
using Iap.Unitilities;
using Iap.Services;
using Iap.Models;
using Iap.DynamicEnglishScreens;
using Iap.DynamicGreekScreens;
using CefSharp.Wpf;
using CefSharp;
using System.IO;
using System.Threading;
using Microsoft.Win32;

namespace Iap
{
   public class AppViewModel:Conductor<Screen>,
        IHandle<ViewGreekCommand>,
        IHandle<ViewEnglishCommand>,
        IHandle<ViewInternetAccessCommand>,
        IHandle<ViewBuyWifiCommand>,
        IHandle<ViewPrintBoardingPassCommand>,
        IHandle<ViewTravelAuthorizationCommand>,
        IHandle<ViewChangeLanguageCommand>,
        IHandle<ViewSrceenSaverCommand>,
        IHandle<ViewDynamicEnglishShellCommand>,
        IHandle<ViewRedirectToBrowserCommand>,
        IHandle<ViewDynamicGreekShellCommand>,
        IHandle<ViewAdvertCommand>,
        IHandle<ViewDynamicBannerEnCommand>,
        IHandle<ViewTwoButtonsShellEnCommand>,
        IHandle<ViewBuyWifi2Command>,
        IHandle<ViewInternetAccess2Command>,
        IHandle<ViewTwoButtonsShellGrCommand>,
        IHandle<ViewTwoButtonsAdvertCommand>,
        IHandle<ViewFirstRegistrationCommand>,
        IHandle<ViewShutDownCommand>,
        IHandle<ViewInstallationCommand>,
        IHandle<ViewStartPrintProgressCommand>
    {
        public IEventAggregator events;
        private bool isGreekSelected;
        private readonly ILog log;
        private readonly IWindowManager windowManager;

        private readonly IGetScreenDetailsService parser;
        private readonly IBannerService bannerService;

        private readonly ILicenceProviderService licenceProvider;

        public AppViewModel(IEventAggregator events,
                            IGetScreenDetailsService parser,
                            IBannerService bannerService,
                            ILicenceProviderService licenceProvider,
                            ILog log,
                            IWindowManager windowManager)
        {
            this.events = events;
            this.parser = parser;
            this.bannerService = bannerService;
            this.licenceProvider = licenceProvider;
            this.log = log;
            this.windowManager = windowManager;
        }

        public ShellViewModel Shell { get; set; }
    
        public ShellGrViewModel ShellGr {get;set;}

        public PrintBoardingPassViewModel PrintBoardingPass { get; set; }
        public PrintBoardingPassGrViewModel PrintBoardingPassGr { get; set; }
        public TravelAuthorizationViewModel TravelAuthorization { get; set; }
        public TravelAuthorizationGrViewModel TravelAuthorizationGr { get; set; }
        public InternetAccessGrViewModel InternetAccessGr { get; set; }
        public InternetAccessViewModel InternetAccess { get; set; }
        public BuyWifiGrViewModel BuyWifiGr { get; set; }
        public BuyWifiViewModel BuyWifi { get; set; }

        public ScreenSaverViewModel ScreenSaver { get; set; }

        public DynamicEnShellViewModel DynamicEnShell { get; set; }
        public DynamicEnShell2ViewModel DynamicEnShell2 { get; set; }
        public DynamicEnShell3ViewModel DynamicEnShell3 { get; set; }
        public DynamicEnShell5ViewModel DynamicEnShell5 { get; set; }
        public DynamicEnShell6ViewModel DynamicEnShell6 { get; set; }
        public DynamicEnShell7ViewModel DynamicEnShell7 { get; set; }
        public DynamicEnShell8ViewModel DynamicEnShell8 { get; set; }
        public DynamicBrowserEnViewModel DynamicBrowserEn { get; set; }
        public DynamicBrowserEn2ViewModel DynamicBrowserEn2 { get; set; }
        public DynamicBrowserEn3ViewModel DynamicBrowserEn3 { get; set; }
        public DynamicBrowserEn5ViewModel DynamicBrowserEn5 { get; set; }
        public DynamicBrowserEn6ViewModel DynamicBrowserEn6 { get; set; }
        public DynamicBrowserEn7ViewModel DynamicBrowserEn7 { get; set; }
        public DynamicBrowserEn8ViewModel DynamicBrowserEn8 { get; set; }
        

        public DynamicGrShellViewModel  DynamicGrShell { get; set; }
        public DynamicGrShell2ViewModel DynamicGrShell2 { get; set; }
        public DynamicGrShell3ViewModel DynamicGrShell3 { get; set; }
        public DynamicGrShell5ViewModel DynamicGrShell5 { get; set;}
        public DynamicGrShell6ViewModel DynamicGrShell6 { get; set; }
        public DynamicGrShell7ViewModel DynamicGrShell7 { get; set; }
        public DynamicGrShell8ViewModel DynamicGrShell8 { get; set; }
        public DynamicBrowserGrViewModel DynamicBrowserGr { get; set; }
        public DynamicBrowserGr2ViewModel DynamicBrowserGr2 { get; set; }
        public DynamicBrowserGr3ViewModel DynamicBrowserGr3 { get; set; }
        public DynamicBrowserGr5ViewModel DynamicBrowserGr5 { get; set;}
        public DynamicBrowserGr6ViewModel DynamicBrowserGr6 { get; set; }
        public DynamicBrowserGr7ViewModel DynamicBrowserGr7 { get; set; }
        public DynamicBrowserGr8ViewModel DynamicBrowserGr8 { get; set; }

        public DynamicBannerBrowserEn5ViewModel DynamicBannerBrowserEn5 { get; set; }
        public DynamicBannerBrowserGr5ViewModel DynamicBannerBrowserGr5 { get; set; }


        public TwoButtonsShellViewModel TwoButtonsShell { get; set; }
        public TwoButtonsShellGrViewModel TwoButtonsShellGr { get; set; }
        public BuyWifi2ViewModel BuyWifi2 { get; set; }
        public InternetAccess2ViewModel InternetAccess2 { get; set; }
        public BuyWifiGr2ViewModel BuyWifiGr2 { get; set; }
        public InternetAccessGr2ViewModel InternetAccessGr2 { get; set; }

        public InstallingViewModel Installing { get; set; }

        public IdleTimeViewModel IdleTime { get; set; }


        public SelectVersionViewModel SelectVersion { get; set; }

        public LoadingViewModel Loading { get; set; }

        public PrintWaitViewModel PrintWait { get; set; }

        private string KioskType { get; set; }

        private AppView mainView;


        protected async override void OnViewLoaded(object view)
        {
            try
            {
                TaskbarManager.HideTaskbar();
            }
            catch { }

            mainView = ((AppView)view);
            CancellationTokenSource ct = new CancellationTokenSource();

            base.ActivateItem(this.Loading);

                if (!this.licenceProvider.hasAlreadyKey())
                {
                    base.ActivateItem(this.SelectVersion);
                    this.SelectVersion.Parent = this;
                }

                else
                {
                string checkLicence = await this.licenceProvider.checkPcLicence(ct);

                this.log.Info("Invoking Action: Return from checkLicence is" + checkLicence);

                if (checkLicence == "1")
                        {
                            this.HandlerAndSettings();
                            base.ActivateItem(this.ScreenSaver);
                            this.ScreenSaver.Parent = this;
                        }
                    else if(checkLicence=="error")

                {
                    this.log.Info("Invoking Action: View An error occured. Return from checkLicence is: " + checkLicence);
                           // System.Windows.MessageBox.Show("An error occured. Please check net connection or contact the administrator");
                            System.Windows.Application.Current.Shutdown();
                 }

                else if(checkLicence=="0")
                        {
                    this.log.Info("Invoking Action: View no valid licence. Return from checkLicence is: " + checkLicence);
                          //  System.Windows.MessageBox.Show("you have not a a used licence");
                            System.Windows.Application.Current.Shutdown();
                       }
                    else
                {
                    this.log.Info("Invoking Action: View An error occured. Return from checkLicence is: " + checkLicence);
                    //System.Windows.MessageBox.Show("An error occured. Please check net connection or contact the administrator");
                    System.Windows.Application.Current.Shutdown();
                }
                }

            base.OnViewLoaded(view);
        }

        public IReadOnlyCollection<ButtonLinkModel> buttons
        {
            get;
            set;
        }

        public IReadOnlyCollection<BannerModel> BannerImages
            { get; set; }

        public IReadOnlyCollection<ButtonLinkModel> tempButtons
        {
            get;
            set;
        }

        protected override void OnDeactivate(bool close)
        {
            System.Windows.Application.Current.Shutdown();
            base.OnDeactivate(close);
        }

        public void Handle(ViewGreekCommand message)
        {
              base.ActivateItem(this.ShellGr);
        }

        public void Handle(ViewEnglishCommand message)
        {
            base.ActivateItem(this.Shell);
        }

        public void Handle(ViewBuyWifiCommand message)
        {
            if (this.isGreekSelected)
            {
                base.ChangeActiveItem(BuyWifiGr,true);
                BuyWifiGr.RemainingTime = message.RemaingTime;
                BuyWifiGr.TimeElapsed = Convert.ToInt32(message.RemaingTime);
            }
            else
            {
                base.ChangeActiveItem(BuyWifi,true);
                BuyWifi.RemainingTime = message.RemaingTime;
                BuyWifi.TimeElapsed = Convert.ToInt32(message.RemaingTime);
            }
        }

        public void Handle(ViewPrintBoardingPassCommand message)
        {
          
            if (this.isGreekSelected)
            {
                base.ChangeActiveItem(PrintBoardingPassGr, true);
                PrintBoardingPassGr.RemainingTime = message.RemainingTime;
                PrintBoardingPassGr.TimeElapsed = Convert.ToInt32(message.RemainingTime);
            }
            else
            {
                base.ChangeActiveItem(PrintBoardingPass, true);
                PrintBoardingPass.RemainingTime = message.RemainingTime;
                PrintBoardingPass.TimeElapsed = Convert.ToInt32(message.RemainingTime);
            }
        }

        public void Handle(ViewInternetAccessCommand message)
        {
            if (this.isGreekSelected)
            {
                base.ChangeActiveItem(InternetAccessGr,true);
                InternetAccessGr.RemainingTime = message.RemainingTime;
                InternetAccessGr.TimeElapsed = Convert.ToInt32(message.RemainingTime);
                InternetAccessGr.ShowBannerUrl = false;
            }
            else
            {
                base.ChangeActiveItem(InternetAccess,true);
                InternetAccess.RemainingTime = message.RemainingTime;
                InternetAccess.TimeElapsed = Convert.ToInt32(message.RemainingTime);
                InternetAccess.ShowBannerUrl = false;
            }
        }

        public void Handle(ViewTravelAuthorizationCommand message)
        {
            
            if (this.isGreekSelected)
            {
                base.ChangeActiveItem(this.TravelAuthorizationGr, true);
                TravelAuthorizationGr.RemainingTime = message.RemaingTime;
                TravelAuthorizationGr.TimeElapsed = Convert.ToInt32(message.RemaingTime);
            }
            else
            {
                base.ChangeActiveItem(this.TravelAuthorization, true);
                TravelAuthorization.RemainingTime = message.RemaingTime;
                TravelAuthorization.TimeElapsed = Convert.ToInt32(message.RemaingTime);
            }
        }

        public void Handle(ViewChangeLanguageCommand message)
        {
            this.isGreekSelected = message.GreekSelected;
        }

        public void Handle(ViewSrceenSaverCommand message)
        {
            try
            {
                this.tempButtons = this.buttons;
                this.buttons = this.parser.GetButtonLinksDetails(this.KioskType);

            }
            catch 
            {
                this.buttons = tempButtons;
            }

            base.ActivateItem(this.ScreenSaver);
        }

        public void Handle(ViewDynamicEnglishShellCommand message)
        {
            string storeTypeFromRegistry = this.licenceProvider.RetrieveTypeFromRegistry();

            if (this.buttons == null)
            {
                if (storeTypeFromRegistry == "WCI")
                {
                    this.events.BeginPublishOnUIThread(new ViewEnglishCommand());
                }
                else
                {
                    this.events.PublishOnUIThread(new ViewTwoButtonsShellEnCommand());
                }
            }
            else
            {

                if (this.buttons.Count == 1)
                {
                    if (storeTypeFromRegistry == "WCI")
                    {
                        this.events.BeginPublishOnUIThread(new ViewEnglishCommand());
                    }
                    else
                    {
                        this.events.PublishOnUIThread(new ViewTwoButtonsShellEnCommand());
                    }
                }

                else
                {

                    List<ButtonLinkModel> ButtonsDetails = this.buttons.ToList();


                    switch (ButtonsDetails.Count)
                    {
                        case 2:
                            this.DynamicEnShell2.PopulateButtonLinks(ButtonsDetails);
                            base.ActivateItem(this.DynamicEnShell2);
                            break;
                        case 3:
                            this.DynamicEnShell3.PopulateButtonLinks(ButtonsDetails);
                            base.ActivateItem(this.DynamicEnShell3);
                            break;
                        case 4:
                            this.DynamicEnShell.PopulateButtonLinks(ButtonsDetails);
                            base.ActivateItem(this.DynamicEnShell);
                            break;
                        case 5:
                            this.DynamicEnShell5.PopulateButtonLinks(ButtonsDetails);
                            base.ActivateItem(this.DynamicEnShell5);
                            break;
                        case 6:
                            this.DynamicEnShell6.PopulateButtonLinks(ButtonsDetails);
                            base.ActivateItem(this.DynamicEnShell6);
                            break;
                        case 7:
                            this.DynamicEnShell7.PopulateButtonLinks(ButtonsDetails);
                            base.ActivateItem(this.DynamicEnShell7);
                            break;
                        case 8:
                            this.DynamicEnShell8.PopulateButtonLinks(ButtonsDetails);
                            base.ActivateItem(this.DynamicEnShell8);
                            break;
                    }
                }
            }
        }

        public void Handle(ViewRedirectToBrowserCommand message)
        {
            List<ButtonLinkModel> buttonDetails = this.buttons.ToList();

            switch(buttonDetails.Count)
            {
                case 2:
                    if(this.isGreekSelected)
                    {
                        base.ChangeActiveItem(this.DynamicBrowserGr2, true);
                        this.DynamicBrowserGr2.ButtonsDetails = message.ButtonsDetails;
                        this.DynamicBrowserGr2.HomeUrl = message.Url;
                        this.DynamicBrowserGr2.SelectedPosition = message.Position;
                        this.DynamicBrowserGr2.PreviousSelected = message.Position;
                    }
                    else
                    {
                        base.ChangeActiveItem(this.DynamicBrowserEn2, true);
                        this.DynamicBrowserEn2.ButtonsDetails = message.ButtonsDetails;
                        this.DynamicBrowserEn2.HomeUrl = message.Url;
                        this.DynamicBrowserEn2.SelectedPosition = message.Position;
                        this.DynamicBrowserEn2.PreviousSelected = message.Position;
                    }

                    break;

                case 3:
                    if (this.isGreekSelected)
                    {
                        base.ChangeActiveItem(this.DynamicBrowserGr3, true);
                        this.DynamicBrowserGr3.ButtonsDetails = message.ButtonsDetails;
                        this.DynamicBrowserGr3.HomeUrl = message.Url;
                        this.DynamicBrowserGr3.SelectedPosition = message.Position;
                        this.DynamicBrowserGr3.PreviousSelected = message.Position;
                    }
                    else
                    {
                        base.ChangeActiveItem(this.DynamicBrowserEn3, true);
                        this.DynamicBrowserEn3.ButtonsDetails = message.ButtonsDetails;
                        this.DynamicBrowserEn3.HomeUrl = message.Url;
                        this.DynamicBrowserEn3.SelectedPosition = message.Position;
                        this.DynamicBrowserEn3.SelectedPosition = message.Position;
                    }

                    break;

                case 4:
                    if (this.isGreekSelected)
                    {
                        base.ChangeActiveItem(this.DynamicBrowserGr, true);
                        this.DynamicBrowserGr.ButtonsDetails = message.ButtonsDetails;
                        this.DynamicBrowserGr.HomeUrl = message.Url;
                        this.DynamicBrowserGr.SelectedPosition = message.Position;
                        this.DynamicBrowserGr.PreviousSelected = message.Position;
                    }
                    else
                    {
                        base.ChangeActiveItem(this.DynamicBrowserEn, true);
                        this.DynamicBrowserEn.ButtonsDetails = message.ButtonsDetails;
                        this.DynamicBrowserEn.HomeUrl = message.Url;
                        this.DynamicBrowserEn.SelectedPosition = message.Position;
                        this.DynamicBrowserEn.PreviousSelected = message.Position;
                    }

                    break;

                case 5:
                    if (this.isGreekSelected)
                    {
                        base.ChangeActiveItem(this.DynamicBrowserGr5, true);
                        this.DynamicBrowserGr5.ButtonsDetails = message.ButtonsDetails;
                        this.DynamicBrowserGr5.HomeUrl = message.Url;
                        this.DynamicBrowserGr5.SelectedPosition = message.Position;
                        this.DynamicBrowserGr5.PreviousSelected = message.Position;
                    }
                    else
                    {
                        base.ChangeActiveItem(this.DynamicBrowserEn5, true);
                        this.DynamicBrowserEn5.ButtonsDetails = message.ButtonsDetails;
                        this.DynamicBrowserEn5.HomeUrl = message.Url;
                        this.DynamicBrowserEn5.SelectedPosition = message.Position;
                        this.DynamicBrowserEn5.PreviousSelected = message.Position;
                    }

                    break;

                case 6:
                    if (this.isGreekSelected)
                    {
                        base.ChangeActiveItem(this.DynamicBrowserGr6, true);
                        this.DynamicBrowserGr6.ButtonsDetails = message.ButtonsDetails;
                        this.DynamicBrowserGr6.HomeUrl = message.Url;
                        this.DynamicBrowserGr6.SelectedPosition = message.Position;
                        this.DynamicBrowserGr6.PreviousSelected = message.Position;
                    }
                    else
                    {
                        base.ChangeActiveItem(this.DynamicBrowserEn6, true);
                        this.DynamicBrowserEn6.ButtonsDetails = message.ButtonsDetails;
                        this.DynamicBrowserEn6.HomeUrl = message.Url;
                        this.DynamicBrowserEn6.SelectedPosition = message.Position;
                        this.DynamicBrowserEn6.PreviousSelected = message.Position;
                    }

                    break;

                case 7:
                    if (this.isGreekSelected)
                    {
                        base.ChangeActiveItem(this.DynamicBrowserGr7, true);
                        this.DynamicBrowserGr7.ButtonsDetails = message.ButtonsDetails;
                        this.DynamicBrowserGr7.HomeUrl = message.Url;
                        this.DynamicBrowserGr7.SelectedPosition = message.Position;
                        this.DynamicBrowserGr7.PreviousSelected = message.Position;
                    }
                    else
                    {
                        base.ChangeActiveItem(this.DynamicBrowserEn7, true);
                        this.DynamicBrowserEn7.ButtonsDetails = message.ButtonsDetails;
                        this.DynamicBrowserEn7.HomeUrl = message.Url;
                        this.DynamicBrowserEn7.SelectedPosition = message.Position;
                        this.DynamicBrowserEn7.PreviousSelected = message.Position;
                    }

                    break;

                case 8:
                    if (this.isGreekSelected)
                    {
                        base.ChangeActiveItem(this.DynamicBrowserGr8, true);
                        this.DynamicBrowserGr8.ButtonsDetails = message.ButtonsDetails;
                        this.DynamicBrowserGr8.HomeUrl = message.Url;
                        this.DynamicBrowserGr8.SelectedPosition = message.Position;
                        this.DynamicBrowserGr8.PreviousSelected = message.Position;
                    }
                    else
                    {
                        base.ChangeActiveItem(this.DynamicBrowserEn8, true);
                        this.DynamicBrowserEn8.ButtonsDetails = message.ButtonsDetails;
                        this.DynamicBrowserEn8.HomeUrl = message.Url;
                        this.DynamicBrowserEn8.SelectedPosition = message.Position;
                        this.DynamicBrowserEn8.PreviousSelected = message.Position;
                    }

                    break;
            }

        }

        public void Handle(ViewDynamicGreekShellCommand message)
        {
            List<ButtonLinkModel> ButtonsDetails = this.buttons.ToList();

            switch(ButtonsDetails.Count)
            {
                case 2:
                    this.DynamicGrShell2.PopulateButtonLinks(ButtonsDetails);
                    base.ActivateItem(this.DynamicGrShell2);
                    break;
                case 3:
                    this.DynamicGrShell3.PopulateButtonLinks(ButtonsDetails);
                    base.ActivateItem(this.DynamicGrShell3);
                    break;
                case 4:
                    this.DynamicGrShell.PopulateButtonLinks(ButtonsDetails);
                    base.ActivateItem(this.DynamicGrShell);
                    break;
                case 5:
                    this.DynamicGrShell5.PopulateButtonLinks(ButtonsDetails);
                    base.ActivateItem(this.DynamicGrShell5);
                    break;
                case 6:
                    this.DynamicGrShell6.PopulateButtonLinks(ButtonsDetails);
                    base.ActivateItem(this.DynamicGrShell6);
                    break;
                case 7:
                    this.DynamicGrShell7.PopulateButtonLinks(ButtonsDetails);
                    base.ActivateItem(this.DynamicGrShell7);
                    break;
                case 8:
                    this.DynamicGrShell8.PopulateButtonLinks(ButtonsDetails);
                    base.ActivateItem(this.DynamicGrShell8);
                    break;
            }
        }

        public void Handle(ViewAdvertCommand message)
        {
            if (this.isGreekSelected)
            {
                base.ChangeActiveItem(InternetAccessGr, true);
                InternetAccessGr.RemainingTime = message.RemainingTime;
                InternetAccessGr.TimeElapsed = Convert.ToInt32(message.RemainingTime);
                InternetAccessGr.ShowBannerUrl = true;
                InternetAccessGr.OpenKeyboard = false;
            }
            else
            {
                base.ChangeActiveItem(InternetAccess, true);
                InternetAccess.RemainingTime = message.RemainingTime;
                InternetAccess.TimeElapsed = Convert.ToInt32(message.RemainingTime);
                InternetAccess.ShowBannerUrl = true;
                InternetAccess.OpenKeyboard = false;
            }
        }

        public void Handle(ViewDynamicBannerEnCommand message)
        {
            List<ButtonLinkModel> buttonDetails = this.buttons.ToList();

            if (this.isGreekSelected)
            {
                base.ChangeActiveItem(this.DynamicBannerBrowserGr5, true);
                this.DynamicBannerBrowserGr5.SelectedPosition = "banner";
                this.DynamicBannerBrowserGr5.ButtonsDetails = message.ButtonDetails;
            }
            else
            {
                base.ChangeActiveItem(this.DynamicBannerBrowserEn5, true);
                this.DynamicBannerBrowserEn5.SelectedPosition = "banner";
                this.DynamicBannerBrowserEn5.ButtonsDetails = message.ButtonDetails;
            }
            
        }

        public void Handle(ViewTwoButtonsShellEnCommand message)
        {
            base.ActivateItem(TwoButtonsShell);
        }

        public void Handle(ViewBuyWifi2Command message)
        {
            if (this.isGreekSelected)
            {
                base.ChangeActiveItem(BuyWifiGr2, true);
                BuyWifiGr2.RemainingTime = message.RemaingTime;
                BuyWifiGr2.TimeElapsed = Convert.ToInt32(message.RemaingTime);
            }
            else
            {
                base.ChangeActiveItem(BuyWifi2, true);
                BuyWifi2.RemainingTime = message.RemaingTime;
                BuyWifi2.TimeElapsed = Convert.ToInt32(message.RemaingTime);
            }
        }

        public void Handle(ViewInternetAccess2Command message)
        {
            if (this.isGreekSelected)
            {
                base.ChangeActiveItem(InternetAccessGr2, true);
                InternetAccessGr2.RemainingTime = message.RemainingTime;
                InternetAccessGr2.TimeElapsed = Convert.ToInt32(message.RemainingTime);
                InternetAccessGr2.ShowBannerUrl = false;
            }
            else
            {
                base.ChangeActiveItem(InternetAccess2, true);
                InternetAccess2.RemainingTime = message.RemainingTime;
                InternetAccess2.TimeElapsed = Convert.ToInt32(message.RemainingTime);
                InternetAccess2.ShowBannerUrl = false;
            }
        }

        public void Handle(ViewTwoButtonsShellGrCommand message)
        {
            base.ActivateItem(this.TwoButtonsShellGr);
        }

        public void Handle(ViewTwoButtonsAdvertCommand message)
        {
            if (this.isGreekSelected)
            {
                base.ChangeActiveItem(InternetAccessGr2, true);
                InternetAccessGr2.RemainingTime = message.RemainingTime;
                InternetAccessGr2.TimeElapsed = Convert.ToInt32(message.RemainingTime);
                InternetAccessGr2.ShowBannerUrl = true;
                InternetAccessGr2.OpenKeyboard = false;
            }
            else
            {
                base.ChangeActiveItem(InternetAccess2, true);
                InternetAccess2.RemainingTime = message.RemainingTime;
                InternetAccess2.TimeElapsed = Convert.ToInt32(message.RemainingTime);
                InternetAccess2.ShowBannerUrl = true;
                InternetAccess2.OpenKeyboard = false;
            }
        }

        public void Handle(ViewFirstRegistrationCommand message)
        {
            this.KioskType = message.KioskType;
             base.ActivateItem(this.ScreenSaver);
            this.ScreenSaver.Parent = this;
            this.HandlerAndSettings();
        }

        private string RetrieveIDFromRegistry()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var path = Path.Combine(directory, "iapSettings.txt");
            if (File.Exists(path))
            {
                string line = File.ReadAllLines(path).Where(x => x.ToString().StartsWith("ID=")).FirstOrDefault();
                string type = line.Replace("ID=", "");
                return type;
            }
            else
            {
                return "null";
            }
        }

        private void HandlerAndSettings()
        {
            Handlers.GlobalCounters.kioskID = RetrieveIDFromRegistry();

            this.IdleTime.StartNotifier();

            EventManager.RegisterClassHandler(
                 typeof(UIElement),
                 Mouse.MouseDownEvent,
                 new MouseButtonEventHandler((s, e) =>
                     this.IdleTime.LastMouseDownEventTime = DateTime.Now));


            EventManager.RegisterClassHandler(
             typeof(ChromiumWebBrowser),
             Mouse.MouseDownEvent,
             new MouseButtonEventHandler((s, e) =>
                 this.IdleTime.LastMouseDownEventTime = DateTime.Now));

            try
            {
                this.buttons = this.parser.GetButtonLinksDetails(this.KioskType);
            }
            catch
            {
                this.buttons = null;
            }

            this.BannerImages = this.bannerService.GetBannerContent();
        }

        public void Handle(ViewShutDownCommand message)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public  void Handle(ViewInstallationCommand message)
        {
            base.ActivateItem(this.Installing);
            this.Installing.CheckAndValidate(message.Type);
        }

        public void Handle(ViewStartPrintProgressCommand message)
        {
            mainView.Focus();
            windowManager.ShowWindow(this.PrintWait);
            try
            {
                TaskbarManager.HideTaskbar();
            }
            catch { }
        }
    }
}
