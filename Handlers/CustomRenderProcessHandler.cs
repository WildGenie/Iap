using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using Caliburn.Micro;

namespace Iap.Handlers
{
   public class CustomRenderProcessHandler : IRenderProcessMessageHandler
    {

        private readonly ILog log;

        public CustomRenderProcessHandler(ILog log)
        {
            this.log = log;
        }

        public void OnContextCreated(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            this.log.Info("Invoking Action: ViewNavigateTo" + browserControl.GetMainFrame().Url + ".");
        }

        public void OnFocusedNodeChanged(IWebBrowser browserControl, IBrowser browser, IFrame frame, IDomNode node)
        {
           
        }
    }
}
