using Caliburn.Micro;
using Iap.Commands;
using Iap.Unitilities;
using System;
using System.Threading;

namespace Iap
{
   public class IdleTimeViewModel:PropertyChangedBase
    {
        public DateTime LastMouseDownEventTime;

        private readonly IEventAggregator events;
        private readonly double applicationIsIdleAfterMinutes;
        private readonly Timer notifier;

        public IdleTimeViewModel(
            IEventAggregator events,
            double applicationIsIdleAfterMinutes)
        {
            this.events = events;
            this.applicationIsIdleAfterMinutes = applicationIsIdleAfterMinutes;

            this.notifier = new Timer(
               this.ActiveSlideshow,
               null,
               TimeSpan.FromSeconds(0),
               TimeSpan.FromSeconds(30));
        }

        public IEventAggregator Events
        {
            get { return this.events; }
        }

        public double ApplicationIsIdleAfterMinutes
        {
            get { return this.applicationIsIdleAfterMinutes; }
        }

        private void ActiveSlideshow(object state)
        {
            if (this.ApplicationIsIdle())
            {
                this.events.PublishOnUIThread(new ViewSrceenSaverCommand());
            }
        }

        private bool ApplicationIsIdle()
        {
            var now = DateTime.Now;
            var lastActivity = this.LastMouseDownEventTime;

            TimeSpan span = now.Subtract(lastActivity);

            return this.ApplicationIsIdleAfterMinutes  < span.Minutes;
        }

        private long GetMaximumAllowedInactivity()
        {
            return TimeSpan
                .FromMinutes(this.applicationIsIdleAfterMinutes)
                .Seconds;
        }

    }
}
