using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Windows.Controls;
using Iap.Commands;

namespace Iap
{
   public class SelectVersionViewModel:Screen
    {
        private readonly IEventAggregator events;
        private readonly ILicenceProviderService licenceProvider;
        private bool nextEnabled;
      

        RadioButton wciRadio;
        RadioButton iapRadio;

        public SelectVersionViewModel(IEventAggregator events)
        {
            this.events = events;
        }

        protected override void OnViewLoaded(object view)
        {
            this.NextEnabled = true;
            wciRadio = ((SelectVersionView)view).WCI;
            iapRadio = ((SelectVersionView)view).IAP;
            base.OnViewLoaded(view);
        }

        public bool NextEnabled
        {
           set
            {
                this.nextEnabled = value;
                NotifyOfPropertyChange(() => this.nextEnabled);
            }
            get
            {
                return this.nextEnabled;
            }
        }

       

        public IEventAggregator Events
        {
            get { return this.events; }
        }

       
        public  void Next()
        {
            this.NextEnabled = false;

            if(wciRadio.IsChecked==true)
            {
                CheckAndValidate("WCI");
               
            }
            else if(iapRadio.IsChecked==true)
            {
                CheckAndValidate("IAP");
            }
            
        }

        private  void CheckAndValidate(string type)
        {
            this.events.PublishOnCurrentThread(new ViewInstallationCommand(type));
        }

    }
}
