﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.IO;
using Iap.Commands;
using System.Windows.Controls;

namespace Iap
{
   public class ScreenSaverViewModel:Screen
    {
        private readonly IEventAggregator events;

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
                string path = @"C:\Users\Σεραφειμ\Documents\Visual Studio 2015\Projects\Iap\Iap\bin\x64\Debug\Media\screensaverVideo.mp4";

                ((ScreenSaverView)view).ScreenSaverVideo.Source = new Uri(path);

                ((ScreenSaverView)view).ScreenSaverVideo.ScrubbingEnabled = true;
                ((ScreenSaverView)view).ScreenSaverVideo.Play();

                ((ScreenSaverView)view).ScreenSaverVideo.MediaEnded += ScreenSaverVideo_MediaEnded;

                ((ScreenSaverView)view).ScreenSaverVideo.MouseDown += ScreenSaverVideo_MouseDown;

            }
            catch (Exception ex)
            {
                this.events.PublishOnCurrentThread(new ViewEnglishCommand());
            }
            base.OnViewLoaded(view);
        }

        private void ScreenSaverVideo_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            MediaElement video = sender as MediaElement;
            video.Position = TimeSpan.FromSeconds(0);
            video.Play();
        }

        private void ScreenSaverVideo_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.events.PublishOnCurrentThread(new ViewEnglishCommand());
        }
    }
}
