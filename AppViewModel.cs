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
        public ShellGrViewModel ShellGr {get;set;}

        public PrintBoardingPassViewModel PrintBoardingPass { get; set; }
        public PrintBoardingPassGrViewModel PrintBoardingPassGr { get; set; }
        public TravelAuthorizationViewModel TravelAuthorization { get; set; }
        public TravelAuthorizationGrViewModel TravelAuthorizationGr { get; set; }
        public IdleInputViewModel IdleInput { get; set; }
        public InternetAccessGrViewModel InternetAccessGr { get; set; }
        public InternetAccessViewModel InternetAccess { get; set; }
        public BuyWifiGrViewModel BuyWifiGr { get; set; }
        public BuyWifiViewModel BuyWifi { get; set; }

        public ScreenSaverViewModel ScreenSaver { get; set; }


        public DynamicEnShellViewModel DynamicEnShell { get; set; }
        public DynamicBrowserEnViewModel DynamicBrowserEn { get; set; }
        public DynamicBrowserEn6ViewModel DynamicBrowserEn6 { get; set; }
        public DynamicEnShell6ViewModel DynamicEnShell6 { get; set; }
        public DynamicEnShell5ViewModel DynamicEnShell5 { get; set; }
        public DynamicEnShell7ViewModel DynamicEnShell7 { get; set; }
        public DynamicEnShell8ViewModel DynamicEnShell8 { get; set; }
        public DynamicBrowserEn5ViewModel DynamicBrowserEn5 { get; set; }
        public DynamicBrowserEn7ViewModel DynamicBrowserEn7 { get; set; }
        public DynamicBrowserEn8ViewModel DynamicBrowserEn8 { get; set; }

        public DynamicGrShellViewModel  DynamicGrShell { get; set; }
        public DynamicGrShell5ViewModel DynamicGrShell5 { get; set;}
        public DynamicGrShell6ViewModel DynamicGrShell6 { get; set; }
        public DynamicGrShell7ViewModel DynamicGrShell7 { get; set; }
        public DynamicGrShell8ViewModel DynamicGrShell8 { get; set; }
        public DynamicBrowserGrViewModel DynamicBrowserGr { get; set; }
        public DynamicBrowserGr5ViewModel DynamicBrowserGr5 { get; set;}
        public DynamicBrowserGr6ViewModel DynamicBrowserGr6 { get; set; }
        public DynamicBrowserGr7ViewModel DynamicBrowserGr7 { get; set; }
        public DynamicBrowserGr8ViewModel DynamicBrowserGr8 { get; set; }

        protected override void OnViewLoaded(object view)
        {
            base.ActivateItem(this.ScreenSaver);
            this.ScreenSaver.Parent = this;

            EventManager.RegisterClassHandler(
                typeof(UIElement),
                Mouse.MouseDownEvent,
                new MouseButtonEventHandler((s, e) =>
                    this.IdleInput.LastMouseDownEventTicks =
                        TimeProvider.Current.UtcNow.ToLocalTime().Ticks));

            try
            {
                this.buttons = this.parser.GetButtonLinksDetails();

               // this.DynamicEnShell.PopulateButtonLinks(buttons);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
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
            }
            else
            {
                base.ChangeActiveItem(BuyWifi,true);
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
            }
            else
            {
                base.ChangeActiveItem(PrintBoardingPass, true);
            }
        }

        public void Handle(ViewInternetAccessCommand message)
        {
            if (this.isGreekSelected)
            {
                base.ChangeActiveItem(InternetAccessGr,true);
            }
            else
            {
                base.ChangeActiveItem(InternetAccess,true);
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
            }
            else
            {
                base.ChangeActiveItem(this.TravelAuthorization, true);
            }
        }

        public void Handle(ViewChangeLanguageCommand message)
        {
            this.isGreekSelected = message.GreekSelected;
        }

        public void Handle(ViewSrceenSaverCommand message)
        {
            base.ActivateItem(this.ScreenSaver);
        }

        public void Handle(ViewDynamicEnglishShellCommand message)
        {
            List<ButtonLinkModel> ButtonsDetails = this.buttons.ToList();
            if (ButtonsDetails.Count == 4)
            {
                this.DynamicEnShell.PopulateButtonLinks(ButtonsDetails);
                base.ActivateItem(this.DynamicEnShell);
            }

            else if (ButtonsDetails.Count == 5)
            {
                this.DynamicEnShell5.PopulateButtonLinks(ButtonsDetails);
                base.ActivateItem(this.DynamicEnShell5);
            }

            else if (ButtonsDetails.Count == 6)
            {
                this.DynamicEnShell6.PopulateButtonLinks(ButtonsDetails);
                base.ActivateItem(this.DynamicEnShell6);
            }

            else if (ButtonsDetails.Count == 7)
            {
                this.DynamicEnShell7.PopulateButtonLinks(ButtonsDetails);
                base.ActivateItem(this.DynamicEnShell7);
            }

            else if (ButtonsDetails.Count == 8)
            {
                this.DynamicEnShell8.PopulateButtonLinks(ButtonsDetails);
                base.ActivateItem(this.DynamicEnShell8);
            }
            //this.DynamicEnShell.Parent = this;
        }

        public void Handle(ViewRedirectToBrowserCommand message)
        {
            List<ButtonLinkModel> buttonDetails = this.buttons.ToList();
            if (buttonDetails.Count == 4)
            {
                if (this.isGreekSelected)
                {
                    base.ChangeActiveItem(this.DynamicBrowserGr, true);
                    this.DynamicBrowserGr.ButtonsDetails = message.ButtonsDetails;
                    this.DynamicBrowserGr.HomeUrl = message.Url;
                    this.DynamicBrowserGr.SelectedPosition = message.Position;
                }
                else
                {
                    base.ChangeActiveItem(this.DynamicBrowserEn, true);
                    this.DynamicBrowserEn.ButtonsDetails = message.ButtonsDetails;
                    this.DynamicBrowserEn.HomeUrl = message.Url;
                    this.DynamicBrowserEn.SelectedPosition = message.Position;
                }
            }

            else if (buttonDetails.Count == 5)
            {
                if (this.isGreekSelected)
                {
                    base.ChangeActiveItem(this.DynamicBrowserGr5, true);
                    this.DynamicBrowserGr5.ButtonsDetails = message.ButtonsDetails;
                    this.DynamicBrowserGr5.HomeUrl = message.Url;
                    this.DynamicBrowserGr5.SelectedPosition = message.Position;
                }
                else
                {
                    base.ChangeActiveItem(this.DynamicBrowserEn5, true);
                    this.DynamicBrowserEn5.ButtonsDetails = message.ButtonsDetails;
                    this.DynamicBrowserEn5.HomeUrl = message.Url;
                    this.DynamicBrowserEn5.SelectedPosition = message.Position;
                }
            }

            else if (buttonDetails.Count == 6)
            {
                if (this.isGreekSelected)
                {
                    base.ChangeActiveItem(this.DynamicBrowserGr6, true);
                    this.DynamicBrowserGr6.ButtonsDetails = message.ButtonsDetails;
                    this.DynamicBrowserGr6.HomeUrl = message.Url;
                    this.DynamicBrowserGr6.SelectedPosition = message.Position;
                }
                else
                {
                    base.ChangeActiveItem(this.DynamicBrowserEn6, true);
                    this.DynamicBrowserEn6.ButtonsDetails = message.ButtonsDetails;
                    this.DynamicBrowserEn6.HomeUrl = message.Url;
                    this.DynamicBrowserEn6.SelectedPosition = message.Position;
                }
            }

            else if (buttonDetails.Count == 7)
            {
                if (this.isGreekSelected)
                {
                    base.ChangeActiveItem(this.DynamicBrowserGr7, true);
                    this.DynamicBrowserGr7.ButtonsDetails = message.ButtonsDetails;
                    this.DynamicBrowserGr7.HomeUrl = message.Url;
                    this.DynamicBrowserGr7.SelectedPosition = message.Position;
                }
                else
                {
                    base.ChangeActiveItem(this.DynamicBrowserEn7, true);
                    this.DynamicBrowserEn7.ButtonsDetails = message.ButtonsDetails;
                    this.DynamicBrowserEn7.HomeUrl = message.Url;
                    this.DynamicBrowserEn7.SelectedPosition = message.Position;
                }
            }

            else if (buttonDetails.Count == 8)
            {
                if (this.isGreekSelected)
                {
                    base.ChangeActiveItem(this.DynamicBrowserGr8, true);
                    this.DynamicBrowserGr8.ButtonsDetails = message.ButtonsDetails;
                    this.DynamicBrowserGr8.HomeUrl = message.Url;
                    this.DynamicBrowserGr8.SelectedPosition = message.Position;
                }
                else
                {
                    base.ChangeActiveItem(this.DynamicBrowserEn8, true);
                    this.DynamicBrowserEn8.ButtonsDetails = message.ButtonsDetails;
                    this.DynamicBrowserEn8.HomeUrl = message.Url;
                    this.DynamicBrowserEn8.SelectedPosition = message.Position;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("another length");
            }
        }

        public void Handle(ViewDynamicGreekShellCommand message)
        {
            List<ButtonLinkModel> ButtonsDetails = this.buttons.ToList();
            if (ButtonsDetails.Count == 4)
            {
                this.DynamicGrShell.PopulateButtonLinks(ButtonsDetails);
                base.ActivateItem(this.DynamicGrShell);
            }

            else if (ButtonsDetails.Count == 5)
            {
                this.DynamicGrShell5.PopulateButtonLinks(ButtonsDetails);
                base.ActivateItem(this.DynamicGrShell5);
            }

            else if (ButtonsDetails.Count == 6)
            {
                this.DynamicGrShell6.PopulateButtonLinks(ButtonsDetails);
                base.ActivateItem(this.DynamicGrShell6);
            }

            else if (ButtonsDetails.Count == 7)
            {
                this.DynamicGrShell7.PopulateButtonLinks(ButtonsDetails);
                base.ActivateItem(this.DynamicGrShell7);
            }

            else if (ButtonsDetails.Count == 8)
            {
                this.DynamicGrShell8.PopulateButtonLinks(ButtonsDetails);
                base.ActivateItem(this.DynamicGrShell8);
            }
        }
    }
}
