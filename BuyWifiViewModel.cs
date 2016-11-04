using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Iap
{
   public class BuyWifiViewModel:Screen
    {
        private readonly IEventAggregator events;

        public BuyWifiViewModel(IEventAggregator events)
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
    }
}
