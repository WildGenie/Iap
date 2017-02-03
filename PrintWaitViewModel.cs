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

        PrintWaitView myView;

        DispatcherTimer hideBarTimer;

        public PrintWaitViewModel(IEventAggregator events)
        {
            this.events = events;
        }

        protected override void OnViewLoaded(object view)
        {
            myView = ((PrintWaitView)view);
            ((PrintWaitView)view).Focus();

            hideBarTimer = new DispatcherTimer();
            hideBarTimer.Interval = TimeSpan.FromMilliseconds(1);
            hideBarTimer.Tick += HideBarTimer_Tick;
            hideBarTimer.Start();

            this.StartCloseTimer();
            base.OnViewLoaded(view);
        }

        private void HideBarTimer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer hideTimer = (DispatcherTimer)sender;
            try
            {
                TaskbarManager.HideTaskbar();
            }
            catch { }
            hideTimer.Tick -= TimerTick;
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
            hideBarTimer.Stop();
            this.TryClose();
        }
    }
}
