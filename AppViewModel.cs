using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Iap.Commands;
using Iap.Gr;
namespace Iap
{
   public class AppViewModel:Conductor<Screen>,
        IHandle<ViewGreekCommand>,
        IHandle<ViewEnglishCommand>,
        IHandle<ViewInternetAccessCommand>,
        IHandle<ViewBuyWifiCommand>,
        IHandle<ViewPrintBoardingPassCommand>,
        IHandle<ViewTravelAuthorizationCommand>,
        IHandle<ViewChangeLanguageCommand>
    {
        public IEventAggregator events;
        private bool isGreekSelected;

        public AppViewModel(IEventAggregator events)
        {
            this.events = events;
        }

        public ShellViewModel Shell { get; set; }
        public ShellGrViewModel ShellGr {get;set;}

        public PrintBoardingPassViewModel PrintBoardingPass { get; set; }
        public PrintBoardingPassGrViewModel PrintBoardingPassGr { get; set; }
        public TravelAuthorizationViewModel TravelAuthorization { get; set; }
        public TravelAuthorizationGrViewModel TravelAuthorizationGr { get; set; }

        public ScreenSaverViewModel ScreenSaver { get; set; }

        protected override void OnViewLoaded(object view)
        {
            //base.ActivateItem(this.Shell);
             //this.Shell.Parent = this;

            base.ActivateItem(this.ScreenSaver);
            this.ScreenSaver.Parent = this;

            base.OnViewLoaded(view);
        }

        protected override void OnDeactivate(bool close)
        {
            System.Windows.Application.Current.Shutdown();
            base.OnDeactivate(close);
        }

        public void Handle(ViewGreekCommand message)
        {
              base.ActivateItem(new ShellGrViewModel(events));
           // base.ActivateItem(new ScreenSaverViewModel(events));
        }

        public void Handle(ViewEnglishCommand message)
        {
            base.ActivateItem(this.Shell);
        }

        public void Handle(ViewBuyWifiCommand message)
        {
            if (this.isGreekSelected)
            {
                base.ActivateItem(new BuyWifiGrViewModel(events));
            }
            else
            {
                base.ActivateItem(new BuyWifiViewModel(events));
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
                base.ActivateItem(new InternetAccessGrViewModel(events));
            }
            else
            {
                base.ActivateItem(new InternetAccessViewModel(events));
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
    }
}
