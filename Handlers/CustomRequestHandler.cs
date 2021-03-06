﻿using CefSharp;
using CefSharp.Wpf;
using Iap.AdornerControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Iap.Services;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Iap.Commands;

namespace Iap.Handlers
{
    public class CustomRequestHandler : IRequestHandler
    {
        private string previousUrl;
        private readonly ILog log;
        private readonly ISendStatsService sender;
        private string numberOfAvailabelPagesToPrint;
        private readonly IEventAggregator events;

        public CustomRequestHandler(string previousUrl, ILog log, ISendStatsService sender, string numberOfAvailabelPagesToPrint, IEventAggregator events)
        {
            this.previousUrl = previousUrl;
            this.log = log;
            this.sender = sender;
            this.numberOfAvailabelPagesToPrint = numberOfAvailabelPagesToPrint;
            this.events = events;
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
            if (request.Url.EndsWith(".pdf"))
            {               
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                browserControl.Load(toNavigate);
            }

            else if(request.Url.EndsWith(".doc"))
            {
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                browserControl.Load(toNavigate);
            }

           else if(request.Url.EndsWith(".xls"))
            {
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                browserControl.Load(toNavigate);
            }

            else if(request.Url.EndsWith(".ppt"))
            {
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                browserControl.Load(toNavigate);
            }

           else if (request.Url.EndsWith("cv"))
            {
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                browserControl.Load(toNavigate);
            }

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

                if (targetUrl.Contains("print=true"))
                {
                    string UrlToDownload = targetUrl.Replace("print=true", "print=false");
                    Thread downLoadThread = new Thread(() =>
                    ConvertToStream(UrlToDownload));
                    downLoadThread.Start();
                }

               
                
            }
            
            return false;
        }

        private  void ConvertToStream(string fileUrl)
        {
            try
            {
                this.events.PublishOnUIThread(new ViewStartPrintProgressCommand());
            }

            catch(Exception ex)
            {
                this.log.Info("InvokingAction: ViewException " + ex.ToString());
            }

            System.Threading.Thread.Sleep(3000);

            try {

                ServicePointManager
        .ServerCertificateValidationCallback +=
        (sender, cert, chain, sslPolicyErrors) => true;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fileUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();



                var theStream = new object();
                string fileName = AppDomain.CurrentDomain.BaseDirectory + "Printings" + @"\toPrint.pdf";


                using (var stream = File.Create(fileName))
                {
                    File.SetAttributes(fileName, FileAttributes.Normal);
                    response.GetResponseStream().CopyTo(stream);
                }

                iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(fileName);
                int numberOfPages = pdfReader.NumberOfPages;
               
                if (GlobalCounters.numberOfCurrentPrintings + numberOfPages <= Convert.ToInt32(this.numberOfAvailabelPagesToPrint))
                {

                    try
                    {
                        this.log.Info("Invoking Action: ViewPrintRequested " + numberOfPages.ToString() + " pages.");
                    }
                    catch
                    { }

                    try
                    {
                        TaskbarManager.HideTaskbar();
                    }
                    catch { }

                    try
                    {

                        System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                        info.UseShellExecute = true;
                        info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        info.Verb = "print";
                        info.FileName = fileName;
                        info.CreateNoWindow = true;


                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        p.StartInfo = info;
                        p.Start();

                        p.WaitForExit();
                        p.CloseMainWindow();


                        if (false == p.CloseMainWindow())
                        {
                            try
                            {
                                p.Kill();
                            }
                            catch { }
                        }
                        else
                        {
                            try
                            {
                                p.Kill();
                            }
                            catch { }
                        }
                    }
                    catch { }
                   
                    GlobalCounters.numberOfCurrentPrintings += numberOfPages;

                    try
                    {
                        TaskbarManager.HideTaskbar();
                    }
                    catch { }
                    
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(6));

                    try
                    {
                        TaskbarManager.HideTaskbar();
                    }
                    catch { }

                   

                    try
                    {
                        this.sender.SendAction("Printed " + numberOfPages + " pages.");

                    }
                    catch { }
                   
                }

                else
                {
                    System.Windows.MessageBox.Show("Unfortunately, you can not print so many pages! Please press OK to continue.");
                }


            }
            catch(Exception ex)
            {
                this.log.Info("InvokingAction: ViewException " + ex.ToString());
            }
            finally
            {

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
               
                browserControl.Load(toNavigate);
            }

            else if (request.Url.EndsWith(".ppt"))
            {
                
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
                
                browserControl.Load(toNavigate);
            }

            else if (request.Url.EndsWith(".xls"))
            {
                
                string toNavigate = "http://docs.google.com/gview?url=" + request.Url + "&embedded=false";
               
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

        public bool OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            return false;
        }

        public void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {
            
        }
    }
}
