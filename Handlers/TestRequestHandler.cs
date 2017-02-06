using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.Wpf;
using Caliburn.Micro;
using System.Security.Cryptography.X509Certificates;

namespace Iap.Handlers
{
   public class TestRequestHandler:IRequestHandler
    {
        private string previousUrl;
        private readonly ILog log;

        public TestRequestHandler(string previousUrl, ILog log)
        {
            this.previousUrl = previousUrl;
            this.log = log;
        }

        public string beforePrintPdfUrl
        {
            get;
            set;
        }

        public bool GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            return false;
        }

        public IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            /*   if(request.Url.EndsWith(".pdf"))
               {
                   string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                   browserControl.Load(toNavigate);
                   GlobalText.beforeStartPrintingUrl = toNavigate;
               }

               else if(request.Url.EndsWith(".doc"))
               {
                   string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                   browserControl.Load(toNavigate);
                   GlobalText.beforeStartPrintingUrl = toNavigate;
               }

              else if(request.Url.EndsWith(".xls"))
               {
                   string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                   browserControl.Load(toNavigate);
                   GlobalText.beforeStartPrintingUrl = toNavigate;
               }

               else if(request.Url.EndsWith(".ppt"))
               {
                   string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                   browserControl.Load(toNavigate);
                   GlobalText.beforeStartPrintingUrl = toNavigate;
               }*/
            this.log.Info("Invoking Action: View request url is " +request.Url);

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
            this.log.Info("Invoking Action: View Target url is " + targetUrl);
            previousUrl = browserControl.GetMainFrame().Url;

            // previousUrl = GlobalText.beforeStartPrintingUrl;
            if (browser.IsPopup)
            {
                browser.MainFrame.ExecuteJavaScriptAsync(@"window.close()");

                if (targetUrl.Contains("print=true"))
                {
                    browserControl.Load(targetUrl.Replace("print=true", "print=false"));
                }

                //   System.Threading.Thread.Sleep(2000);
                // browserControl.Load(GlobalText.beforeStartPrintingUrl);

                browserControl.FrameLoadEnd +=(s,e)=> BrowserControl_FrameLoadEnd(previousUrl,s,e);

            }
            
            return false;
        }

        public  void BrowserControl_FrameLoadEnd(string previous,object sender, FrameLoadEndEventArgs e)
        {
            if (this.previousUrl != "")
            {
                ChromiumWebBrowser mainBrowser = sender as ChromiumWebBrowser;

                if (mainBrowser.GetMainFrame().Url.Contains("print"))
                {
                    string path = System.IO.Path.Combine(
                               System.IO.Path.GetDirectoryName(
                               this.GetType().Assembly.Location),
                               "Printings", "test.pdf");

                    mainBrowser.PrintToPdfAsync(path);

                    // mainBrowser.Load(this.previousUrl);
                    // mainBrowser.Load(beforePrintPdfUrl);
                    this.log.Info("Invoking Action: View Final Url is " + mainBrowser.GetMainFrame().Url);
                    this.log.Info("Invoking Action: View FinalFromMe is " + previousUrl);
                    // mainBrowser.Load(GlobalText.beforeStartPrintingUrl);
                    mainBrowser.Load(previousUrl);
                    if(mainBrowser.GetMainFrame().Url.Contains(".googleusercontent.com"))
                    {
                        this.log.Info("Invoking Action: View sos i must redirect");
                        mainBrowser.GetMainFrame().LoadUrl(previousUrl);
                    }
                }
               // this.previousUrl = "";
            }
            else
            {
                this.log.Info("InvokingAction: View no previous url");
            }
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
            if (request.Url.EndsWith(".doc"))
            {
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                //beforePrintPdfUrl = toNavigate;
                // GlobalText.beforeStartPrintingUrl = toNavigate;
                browserControl.Load(toNavigate);
            }

            else if (request.Url.EndsWith(".ppt"))
            {
                // string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=true&fullscreen=yes";
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                //beforePrintPdfUrl = toNavigate;
                //GlobalText.beforeStartPrintingUrl = toNavigate;
                browserControl.Load(toNavigate);
            }

            else if (request.Url.EndsWith(".xls"))
            {
                //string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=true&fullscreen=yes";
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                // beforePrintPdfUrl = toNavigate;
                //GlobalText.beforeStartPrintingUrl = toNavigate;
                browserControl.Load(toNavigate);
            }

            else if (request.Url.EndsWith(".pdf"))
            {
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                //beforePrintPdfUrl = toNavigate;
                //GlobalText.beforeStartPrintingUrl = toNavigate;
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

        public bool OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            return false;
        }

        public void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {
           
        }
    }
}
