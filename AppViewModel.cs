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
        IHandle<ViewBuyWifiCommand>
    {
        public IEventAggregator events;

        public AppViewModel(IEventAggregator events)
        {
            this.events = events;
        }

        public ShellViewModel Shell { get; set; }
        public ShellGrViewModel ShellGr {get;set;}

        protected override void OnViewLoaded(object view)
        {
            base.ActivateItem(this.Shell);
            this.Shell.Parent = this;
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
        }

        public void Handle(ViewEnglishCommand message)
        {
            base.ActivateItem(this.Shell);
        }

        public void Handle(ViewInternetAccessCommand message)
        {
            base.ActivateItem(new InternetAccessViewModel(events));
        }

        public void Handle(ViewBuyWifiCommand message)
        {
            base.ActivateItem(new BuyWifiViewModel(events));
        }
    }
}
