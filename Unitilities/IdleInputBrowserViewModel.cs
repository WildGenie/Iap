using Caliburn.Micro;
using Iap.Commands;
using System;
using System.Threading;

namespace Iap.Unitilities
{
   public class IdleInputBrowserViewModel:PropertyChangedBase
    {
        public long LastMouseDownEventTicks;

        private readonly IEventAggregator events;
        private readonly double browserIsIdleAfterMinutes;
        private readonly Timer notifier;

        public IdleInputBrowserViewModel(IEventAggregator events,
            double browserIsIdleAfterMinutes)
        {
            this.events = events;
            this.browserIsIdleAfterMinutes = browserIsIdleAfterMinutes;

            this.notifier = new Timer(
                this.ActiveSlideshow,
                null,
                TimeSpan.FromSeconds(0),
                TimeSpan.FromMinutes(this.browserIsIdleAfterMinutes));
        }

        public IEventAggregator Events
        {
            get { return this.events; }
        }

        public double ApplicationIsIdleAfterMinutes
        {
            get { return this.browserIsIdleAfterMinutes; }
        }

        private void ActiveSlideshow(object state)
        {
            if (this.ApplicationIsIdle())
                this.events.PublishOnCurrentThread(new ViewSrceenSaverCommand());
        }

        private bool ApplicationIsIdle()
        {
            var now = TimeProvider.Current.UtcNow.ToLocalTime().Ticks;
            var lastActivity = this.LastMouseDownEventTicks;

            return this.GetMaximumAllowedInactivity() < (now - lastActivity);
        }

        private long GetMaximumAllowedInactivity()
        {
            return TimeSpan
                .FromMinutes(this.browserIsIdleAfterMinutes)
                .Ticks;
        }
    }
}
