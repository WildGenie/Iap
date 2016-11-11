using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;

namespace Iap.Handlers
{
   public class CustomRenderProcessHandler : IRenderProcessMessageHandler
    {
        public void OnContextCreated(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            /* const string script = "document.addEventListener('DOMContentLoaded', function(){ alert('DomLoaded'); });";
             frame.ExecuteJavaScriptAsync(script);
             frame.ExecuteJavaScriptAsync(@"window.print=function(){alert('hello')}");*/
           
        }

        public void OnFocusedNodeChanged(IWebBrowser browserControl, IBrowser browser, IFrame frame, IDomNode node)
        {
            //throw new NotImplementedException();
        }
    }
}
