using CefSharp;
using CefSharp.Wpf;
using Iap.AdornerControl;
using Iap.Bounds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
          /*  if (browserControl.GetMainFrame().Url.Contains("mail"))
            {
                ConvertToStream(browserControl.GetMainFrame().Url);
            }*/
            
            if (!browserControl.GetMainFrame().Url.Contains("docs.google.com"))
            {
                newBrowser = null;
                
                browserControl.Load(targetUrl);
                //browserControl.ExecuteScriptAsync(@"window.print=function(){alert('hello')");
               // frame.ExecuteJavaScriptAsync(@"window.print = function(){ alert('hello');");

               // var task = browserControl.EvaluateScriptAsync(@"window.print = function(){ alert('hello');", TimeSpan.FromSeconds(10));
                //task.Wait();

                return true;
            }
            else
            {
              //  GlobalText.beforeStartPrintingUrl = browserControl.GetMainFrame().Url;
                newBrowser = null;
                browserControl.Load(targetUrl);
                return false;
            }
            
        }

        private void ConvertToStream(string fileUrl)
        {
            try
            {

                Thread waitThread = new Thread(() =>
                {
                    PleaseWaitWindow wait = new PleaseWaitWindow();

                    wait.ShowDialog();
                    // wait.LoadingAdorner.IsAdornerVisible = true;
                    wait.Close();

                });
                waitThread.SetApartmentState(ApartmentState.STA);
                waitThread.Start();
            }

            catch
            {

            }

            System.Threading.Thread.Sleep(2000);

            try
            {

              //  ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

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

                if (GlobalCounters.numberOfCurrentPrintings + numberOfPages <= Convert.ToInt32("6"))
                {

                    try
                    {
                       // this.log.Info("Invoking Action: ViewPrintRequested " + numberOfPages.ToString() + " pages.");
                    }
                    catch
                    { }

                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                    info.UseShellExecute = true;
                    info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    info.Verb = "print";
                    info.FileName = fileName;
                    info.CreateNoWindow = true;


                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo = info;
                    p.Start();

                    p.WaitForInputIdle();
                    p.CloseMainWindow();
                    /*  if (numberOfPages < Int32.Parse(this.numberOfAvailabelPagesToPrint))
                      {
                          System.Threading.Thread.Sleep(TimeSpan.FromSeconds(numberOfPages));
                      }
                      else
                      {
                          System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));
                      }*/
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

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

                    try
                    {
                      //  this.sender.SendAction("Printed " + numberOfPages + " pages.");

                    }
                    catch { }
                    GlobalCounters.numberOfCurrentPrintings += numberOfPages;
                }

                else
                {
                    System.Windows.MessageBox.Show("Unfortunately, you can not print so many pages! Please press OK to continue.");
                }


            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
                //System.Windows.MessageBox.Show(ex.ToString());
            }
            finally
            {

                //response.Close();
            }
        }
    }
}
