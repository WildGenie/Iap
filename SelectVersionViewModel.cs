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

        RadioButton wciRadio;
        RadioButton iapRadio;

        public SelectVersionViewModel(IEventAggregator events)
        {
            this.events = events;
        }

        protected override void OnViewLoaded(object view)
        {
            wciRadio = ((SelectVersionView)view).WCI;
            iapRadio = ((SelectVersionView)view).IAP;
            base.OnViewLoaded(view);
        }

        public IEventAggregator Events
        {
            get { return this.events; }
        }

        public void Next()
        {
            if(wciRadio.IsChecked==true)
            {
                this.events.PublishOnCurrentThread(new ViewFirstRegistrationCommand("WCI"));
            }
            else if(iapRadio.IsChecked==true)
            {
                this.events.PublishOnCurrentThread(new ViewFirstRegistrationCommand("IAP"));
            }
        }
    }
}
