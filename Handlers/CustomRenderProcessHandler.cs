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

       

        public CustomRenderProcessHandler()
        {
        }

        public void OnContextCreated(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
        }

        public void OnFocusedNodeChanged(IWebBrowser browserControl, IBrowser browser, IFrame frame, IDomNode node)
        {
           
        }
    }
}
