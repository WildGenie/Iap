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
        IHandle<ViewRedirectToBrowserCommand>
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
            this.DynamicEnShell.PopulateButtonLinks(this.buttons.ToList());
            base.ActivateItem(this.DynamicEnShell);
            //this.DynamicEnShell.Parent = this;
        }

        public void Handle(ViewRedirectToBrowserCommand message)
        {
            List<ButtonLinkModel> buttonDetails = this.buttons.ToList();
            if (buttonDetails.Count == 4)
            {
                base.ChangeActiveItem(this.DynamicBrowserEn, true);
                this.DynamicBrowserEn.ButtonsDetails = message.ButtonsDetails;
                this.DynamicBrowserEn.HomeUrl = message.Url;
                this.DynamicBrowserEn.SelectedPosition = message.Position;
            }

            else if (buttonDetails.Count == 6)
            {
                base.ChangeActiveItem(this.DynamicBrowserEn6, true);
                this.DynamicBrowserEn6.ButtonsDetails = message.ButtonsDetails;
                this.DynamicBrowserEn6.HomeUrl = message.Url;
                this.DynamicBrowserEn6.SelectedPosition = message.Position;
            }
            else
            {
                System.Windows.MessageBox.Show("another length");
            }
        }
    }
}
