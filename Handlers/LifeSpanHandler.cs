using CefSharp;
using CefSharp.Wpf;
using Iap.Bounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iap.Handlers
{
    public class LifeSpanHandler : ILifeSpanHandler
    {
        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            return false;
        }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
            
        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
            
        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            
            if (!browserControl.GetMainFrame().Url.Contains("docs.google.com"))
            {
                newBrowser = null;
                
                browserControl.Load(targetUrl);
                browserControl.ExecuteScriptAsync(@"window.print=function(){alert('hello')");
                return true;
            }
            else
            {
                newBrowser = null;
                browserControl.Load(targetUrl);
                return false;
            }
            
        }
    }
}
