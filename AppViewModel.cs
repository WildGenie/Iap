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
        IHandle<ViewDynamicGreekShellCommand>
    {
        public IEventAggregator events;
        private bool isGreekSelected;

        private readonly IGetScreenDetailsService parser;

        public AppViewModel(IEventAggregator events, IGetScreenDetailsService parser)
        {
            this.events = events;
            this.parser = parser;
        }

        public ShellViewModel Shell { get; set; }
      //  public IdleInputViewModel IdleInput { get; set; }
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

        public IdleBrowserViewModel IdleBrowser { get; set; }

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
        

        protected override void OnViewLoaded(object view)
        {
            base.ActivateItem(this.ScreenSaver);
            this.ScreenSaver.Parent = this;

            try
            {
                string path = System.IO.Path.Combine(
                              System.IO.Path.GetDirectoryName(
                              this.GetType().Assembly.Location),
                              "Printings");


                System.IO.DirectoryInfo di = new DirectoryInfo(path);

                if (Directory.Exists(path))
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }
            }
            catch { }

            /* EventManager.RegisterClassHandler(
                 typeof(UIElement),
                 Mouse.MouseDownEvent,
                 new MouseButtonEventHandler((s, e) =>
                     this.IdleInput.LastMouseDownEventTicks =
                         TimeProvider.Current.UtcNow.ToLocalTime().Ticks));*/

            EventManager.RegisterClassHandler(
             typeof(ChromiumWebBrowser),
             Mouse.MouseDownEvent,
             new MouseButtonEventHandler((s, e) =>
                 this.IdleBrowser.LastMouseDownEventTicks =
                     TimeProvider.Current.UtcNow.ToLocalTime().Ticks));

            try
            {
                this.buttons = this.parser.GetButtonLinksDetails();

               // this.DynamicEnShell.PopulateButtonLinks(buttons);

            }
            catch(Exception ex)
            {
              //  System.Windows.MessageBox.Show(ex.ToString());
                this.buttons = null;
            }
            base.OnViewLoaded(view);
        }

        public IReadOnlyCollection<ButtonLinkModel> buttons
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
              base.ActivateItem(new ShellGrViewModel(events));
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
            /* if (this.isGreekSelected)
             {
                 base.ActivateItem(new PrintBoardingPassGrViewModel(events,this.boardingPassGrApi));
             }
             else
             {
                 base.ActivateItem(new PrintBoardingPassViewModel(events,this.boardingPassApi));
             }*/
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
            }
            else
            {
                base.ChangeActiveItem(InternetAccess,true);
                InternetAccess.RemainingTime = message.RemainingTime;
                InternetAccess.TimeElapsed = Convert.ToInt32(message.RemainingTime);
            }
        }

        public void Handle(ViewTravelAuthorizationCommand message)
        {
            /*if (this.isGreekSelected)
            {
                base.ActivateItem(new TravelAuthorizationGrViewModel(events,this.travelAuthorizationGrApi));
            }
            else
            {
                base.ActivateItem(new TravelAuthorizationViewModel(events,this.travelAuthorizationApi));
            }*/
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
           /* try
            {
                System.Threading.Thread checkServiceThread = new System.Threading.Thread(() =>
                    this.buttons = this.parser.GetButtonLinksDetails()
                );
                checkServiceThread.Start();
            }
            catch { }*/
            base.ActivateItem(this.ScreenSaver);
        }

        public void Handle(ViewDynamicEnglishShellCommand message)
        {
            if (this.buttons == null)
            {
                this.events.BeginPublishOnUIThread(new ViewEnglishCommand());
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
    }
}
