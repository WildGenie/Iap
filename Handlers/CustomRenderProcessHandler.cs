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
            /* const string script = "document.addEventListener('DOMContentLoaded', function(){ alert('DomLoaded'); });";
             frame.ExecuteJavaScriptAsync(script);
             frame.ExecuteJavaScriptAsync(@"window.print=function(){alert('hello')}");*/
            // frame.ExecuteJavaScriptAsync(@"window.print=function(){alert('hello');}");
            /* if (browserControl.GetMainFrame().Url.Contains("print = true"))
             {
                 System.Windows.MessageBox.Show("pdf");
                 PrinterCanceller.CancelPrint();
             }

             if (browser.MainFrame.Url.Contains("print=false"))
             {
                 System.Windows.MessageBox.Show("print");
             }*/
        }

        public void OnFocusedNodeChanged(IWebBrowser browserControl, IBrowser browser, IFrame frame, IDomNode node)
        {
            //throw new NotImplementedException();
        }
    }
}
