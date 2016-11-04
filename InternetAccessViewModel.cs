using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Iap.Commands;
using CefSharp.Wpf;
using CefSharp;

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

            _internetAccessBrowser.BrowserSettings = new CefSharp.BrowserSettings()
            {
                OffScreenTransparentBackground = false,
            };

            _internetAccessBrowser.Load("http://www.google.com");

            ((InternetAccessView)view).InternetAccessBrowser.Children.Add(_internetAccessBrowser);

            _internetAccessBrowser.TouchDown += _internetAccessBrowser_TouchDown;

            _internetAccessBrowser.TouchMove += _internetAccessBrowser_TouchMove;

            _internetAccessBrowser.RequestContext = new RequestContext();

            _internetAccessBrowser.Focus();

            base.OnViewLoaded(view);
        }

        public int lastMousePositionX;
        public int lastMousePositionY;

        private void _internetAccessBrowser_TouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {
            int x = (int)e.GetTouchPoint(_internetAccessBrowser).Position.X;
            int y = (int)e.GetTouchPoint(_internetAccessBrowser).Position.Y;


            int deltax = x - lastMousePositionX;
            int deltay = y - lastMousePositionY;

            _internetAccessBrowser.SendMouseWheelEvent((int)_internetAccessBrowser.Width, (int)_internetAccessBrowser.Height, deltax, deltay, CefEventFlags.None);
        }

        private void _internetAccessBrowser_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            lastMousePositionX = (int)e.GetTouchPoint(_internetAccessBrowser).Position.X;
            lastMousePositionY = (int)e.GetTouchPoint(_internetAccessBrowser).Position.Y;
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

        public void ViewBuyWifi()
        {
            this.events.PublishOnCurrentThread(new ViewBuyWifiCommand());
        }
    }
}
