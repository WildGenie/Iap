using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.IO;
using Iap.Commands;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Iap
{
   public class ScreenSaverViewModel:Screen
    {
        private readonly IEventAggregator events;

        DispatcherTimer timer = new DispatcherTimer();

        MediaElement ScreenSaverVideo;

        public ScreenSaverViewModel(IEventAggregator events)
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

        protected override void OnViewLoaded(object view)
        {
            try
            {
                string path = System.IO.Path.Combine(
                 System.IO.Path.GetDirectoryName(
                 this.GetType().Assembly.Location),
                 "Media", "AIA_revised_converted.mp4");

                ScreenSaverVideo = ((ScreenSaverView)view).ScreenSaverVideo;

                /* ((ScreenSaverView)view).ScreenSaverVideo.Source = new Uri(path);

                 ((ScreenSaverView)view).ScreenSaverVideo.ScrubbingEnabled = true;
                 ((ScreenSaverView)view).ScreenSaverVideo.Play();

                 ((ScreenSaverView)view).ScreenSaverVideo.MediaEnded += ScreenSaverVideo_MediaEnded;

                 ((ScreenSaverView)view).ScreenSaverVideo.MouseDown += ScreenSaverVideo_MouseDown;*/

                ScreenSaverVideo.Source = new Uri(path);

                ScreenSaverVideo.ScrubbingEnabled = true;
                ScreenSaverVideo.Play();

                ScreenSaverVideo.MediaEnded += ScreenSaverVideo_MediaEnded;

                ScreenSaverVideo.MouseDown += ScreenSaverVideo_MouseDown;

                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Tick += Timer_Tick;

                this.TimeElapsed = 0;

            }
            catch
            {
                this.events.PublishOnCurrentThread(new ViewEnglishCommand());
            }

            try
            {
                string path = System.IO.Path.Combine(
                              System.IO.Path.GetDirectoryName(
                              this.GetType().Assembly.Location),
                              "Printings");


                System.IO.DirectoryInfo di = new DirectoryInfo(path);

                if (Directory.Exists(path))
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }
            }
            catch { }

            base.OnViewLoaded(view);
        }

        public int TimeElapsed
        {
            get;
            set;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
           if(TimeElapsed>60)
            {
                ScreenSaverVideo.Position = TimeSpan.FromSeconds(0);
                ScreenSaverVideo.Play();
                timer.Stop();
            }
           else
            {
                TimeElapsed++;
            }
        }

        private void ScreenSaverVideo_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            /* System.Threading.Thread.Sleep(TimeSpan.FromMinutes(2));
             MediaElement video = sender as MediaElement;
             video.Position = TimeSpan.FromSeconds(0);
             video.Play();*/
            TimeElapsed = 0;
            timer.Start();
        }

        private void ScreenSaverVideo_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            timer.Stop();
            //this.events.PublishOnCurrentThread(new ViewEnglishCommand());
           //loopThread.Abort();
              this.events.PublishOnCurrentThread(new ViewDynamicEnglishShellCommand());
        }
    }
}
