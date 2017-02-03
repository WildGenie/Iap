using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Iap
{
   public class PrintWaitViewModel:Screen
    {
        private readonly IEventAggregator events;

        public PrintWaitViewModel(IEventAggregator events)
        {
            this.events = events;
        }

        protected override void OnViewLoaded(object view)
        {
            this.StartCloseTimer();
            base.OnViewLoaded(view);
        }

        private void StartCloseTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(15);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= TimerTick;
            // Close();
            this.TryClose();
        }
    }
}
