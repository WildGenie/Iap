using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iap.Handlers
{
    public class RequestHandler : IRequestHandler
    {
        public bool GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            return false;
        }

        public IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            
            return null;
        }

        public bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool isRedirect)
        {
            return false;
        }

        public CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            return CefReturnValue.Continue;
        }

        public bool OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            return false;
        }

        public bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            if (browser.IsPopup)
            {
                browser.MainFrame.ExecuteJavaScriptAsync(@"window.close()");
                browserControl.Load(targetUrl.Replace("print=true", "print=false"));
                PrinterCanceller.CancelPrint();
            }
            return false;
        }

        public void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        {
           
        }

        public bool OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
        {
            return false;
        }

        public bool OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            return false;
        }

        public void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
        {
            
        }

        public void OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
        {
           
        }

        public void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            //browserControl.ExecuteScriptAsync(@"function($){}")

            if (request.Url.EndsWith(".doc"))
            {
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                browserControl.Load(toNavigate);
            }

            else if (request.Url.EndsWith(".ppt"))
            {
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=true&fullscreen=yes";
                browserControl.Load(toNavigate);
            }

            else if (request.Url.EndsWith(".xls"))
            {
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=true&fullscreen=yes";
                browserControl.Load(toNavigate);
            }

            else if (request.Url.EndsWith(".pdf"))
            {
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                browserControl.Load(toNavigate);
            }
        }

        public void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, ref string newUrl)
        {
           
        }

        public bool OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            return false;
        }
    }
}
