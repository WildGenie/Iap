using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.IO;

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

               /* string videoPath =
                    Path.Combine(
                    Path.GetDirectoryName(
                        this.GetType().Assembly.Location),
                    "Media",
                    "AIAnoloopyellow.mov").ToString();*/

               // string path = @"C:\Users\Σεραφειμ\Documents\Visual Studio 2015\Projects\Iap\Iap\bin\x64\Debug\Media\SampleVideo_1280x720_1mb.mp4";

               // ((ScreenSaverView)view).ScreensaverVideo.Source = new Uri(path);

               /* ((ScreenSaverView)view).ScreensaverVideo.ScrubbingEnabled = true;
                ((ScreenSaverView)view).ScreensaverVideo.Play();*/
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
            base.OnViewLoaded(view);
        }
    }
}
