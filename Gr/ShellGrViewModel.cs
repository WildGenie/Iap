using System;
using Caliburn.Micro;
using Iap.Commands;

namespace Iap.Gr
{
   public class ShellGrViewModel:Screen
    {
        private readonly IEventAggregator events;

        public ShellGrViewModel(IEventAggregator events)
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

        public void ViewEnglish()
        {
            this.events.PublishOnCurrentThread(new ViewEnglishCommand());
        }
    }
}
