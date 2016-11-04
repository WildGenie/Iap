using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Iap.Commands;
using CefSharp.Wpf;

namespace Iap
{
   public class InternetAccessViewModel:Screen
    {
        private readonly IEventAggregator events;

        public static ChromiumWebBrowser _internetAccessBrowser;

        public InternetAccessViewModel(IEventAggregator events)
        {
            this.events = events;
        }

        protected override void OnViewLoaded(object view)
        {
            _internetAccessBrowser = new ChromiumWebBrowser()
            {
                Address = "http://www.google.com",
            };



            _internetAccessBrowser.Load("http://www.google.com");

            ((InternetAccessView)view).InternetAccessBrowser.Children.Add(_internetAccessBrowser);
            
            base.OnViewLoaded(view);
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
            if (_internetAccessBrowser != null)
            {
                _internetAccessBrowser.Dispose();
            }
            this.events.PublishOnCurrentThread(new ViewEnglishCommand());
        }

        protected override void OnDeactivate(bool close)
        {
            if (_internetAccessBrowser != null)
            {
                _internetAccessBrowser.Dispose();
            }
            base.OnDeactivate(close);
        }
    }
}
