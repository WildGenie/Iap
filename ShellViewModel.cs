using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Iap.Commands;

namespace Iap
{
   public class ShellViewModel:Screen
    {
        private readonly IEventAggregator events;

        public ShellViewModel(IEventAggregator events)
        {
            this.events = events;
        }

        public IEventAggregator Events
        {
            get
            {
                return this.events;
            }
        }

        public void BuyWifi()
        {
            this.events.PublishOnBackgroundThread(new ViewBuyWifiCommand());
        }

        public void PrintBoardingPass()
        {

        }

        public void InternetAccess()
        {
             this.events.PublishOnCurrentThread(new ViewInternetAccessCommand());
            //this.events.BeginPublishOnUIThread(new ViewInternetAccessCommand());
            //  this.events.PublishOnUIThread(new ViewInternetAccessCommand());
           // this.events.PublishOnBackgroundThread(new ViewInternetAccessCommand());
        }

        public void TravelAuthorization()
        {

        }

        public void ViewGreek()
        {
            this.events.PublishOnCurrentThread(new ViewGreekCommand());
        }
    }
}
