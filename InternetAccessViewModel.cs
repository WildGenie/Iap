using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Iap.Commands;

namespace Iap
{
   public class InternetAccessViewModel:Screen
    {
        private readonly IEventAggregator events;

        public InternetAccessViewModel(IEventAggregator events)
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

        public void Back()
        {
            this.events.PublishOnCurrentThread(new ViewEnglishCommand());
        }
    }
}
