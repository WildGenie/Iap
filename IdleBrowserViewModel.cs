using Caliburn.Micro;
using Iap.Commands;
using Iap.Unitilities;
using System;
using System.Threading;


namespace Iap
{
   public class IdleBrowserViewModel:PropertyChangedBase
    {
        public long LastMouseDownEventTicks;

        private readonly IEventAggregator events;
        private readonly double applicationIsIdleAfterMinutes;
        private readonly Timer notifier;

        public IdleBrowserViewModel(
            IEventAggregator events,
            double applicationIsIdleAfterMinutes)
        {
            this.events = events;
            this.applicationIsIdleAfterMinutes = applicationIsIdleAfterMinutes;

            this.notifier = new Timer(
               this.ActiveSlideshow,
               null,
               TimeSpan.FromSeconds(0),
               TimeSpan.FromMinutes(this.applicationIsIdleAfterMinutes));
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
                this.events.PublishOnUIThread(new ViewSrceenSaverCommand());
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
                .FromMinutes(this.applicationIsIdleAfterMinutes)
                .Ticks;
        }
    }
}
