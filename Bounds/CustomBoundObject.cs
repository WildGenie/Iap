using Caliburn.Micro;
using CefSharp;
using CefSharp.Wpf;
using Iap.AdornerControl;
using Iap.Commands;
using Iap.Handlers;
using Iap.Services;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Iap.Bounds
{
   public class CustomBoundObject
    {
        private readonly string numberOfAvailablePagesToPrint;
        ChromiumWebBrowser _mainBrowser;
        private readonly ILog log;
        private readonly ISendStatsService sender;
        private readonly IEventAggregator events;

        public CustomBoundObject(string numberOfAvailablePagesToPrint, ILog log, ISendStatsService sender, IEventAggregator events)
        {
            this.numberOfAvailablePagesToPrint = numberOfAvailablePagesToPrint;
            this.log = log;
            this.sender = sender;
            this.events = events;
        }

        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
          
            _mainBrowser = sender as ChromiumWebBrowser;

            

           /* if (_mainBrowser.GetMainFrame().Url.EndsWith(".pdf") && !_mainBrowser.GetMainFrame().Url.Contains("docs.google.com"))
            {
                _mainBrowser.GetMainFrame().LoadUrl("http://docs.google.com/gview?url=" + _mainBrowser.GetMainFrame().Url + "&embedded=false");
            }*/


            if (_mainBrowser.GetMainFrame().Url.Contains("print=true"))
            {
                _mainBrowser.Load(_mainBrowser.GetMainFrame().Url.Replace("print=true", "print=false"));
            }

            _mainBrowser.ExecuteScriptAsync(@"document.onselectstart = function()
        {
            return false;
        }");


            if (_mainBrowser.GetMainFrame().Url.Contains("print=false"))
            {

                _mainBrowser.ExecuteScriptAsync(@"
                    
                
                var beforePrint=function(){

                    bound.onPrintRequested('before');
                };        

                var afterPrint = function() {
                   bound.onPrintRequested('after');
                };

                if (window.matchMedia) {
                    var mediaQueryList = window.matchMedia('print');
                    mediaQueryList.addListener(function(mql) {
                        if (mql.matches) {
                            beforePrint();
                        } else {
                            afterPrint();
                        }
                    });
                }

                window.onbeforeprint = beforePrint;
                window.onafterprint = afterPrint;

            ");



                var printScript = @"window.print=function() {bound.onPrintRequested('before');}";

                _mainBrowser.ExecuteScriptAsync(printScript);

            }

            else
            {
                var printScript = @"window.print=function() {bound.onPrintRequested('before');}";

                _mainBrowser.ExecuteScriptAsync(printScript);
            }

            _mainBrowser.ExecuteScriptAsync(
                   @"var elements = document.getElementsByClassName('ndfHFb-c4YZDc-to915-LgbsSe ndfHFb-c4YZDc-C7uZwb-LgbsSe VIpgJd-TzA9Ye-eEGnhe ndfHFb-c4YZDc-LgbsSe ndfHFb-c4YZDc-C7uZwb-LgbsSe-SfQLQb-Bz112c');
                    elements[1].style.display = 'none';
                    ");

        }

    

        public async void onPrintRequested(string selected)
        {
            try
            {

                /* Thread waitThread = new Thread(() =>
                 {
                     PleaseWaitWindow wait = new PleaseWaitWindow();

                     wait.ShowDialog();
                     wait.Close();

                 });
                 waitThread.SetApartmentState(ApartmentState.STA);
                 waitThread.Start();*/
                this.events.PublishOnUIThread(new ViewStartPrintProgressCommand());
            }

            catch
            {
                
            }

           // System.Threading.Thread.Sleep(2000);

                if (selected == "before")
                {
                   // PrinterCanceller.CancelPrint();

                    string path = System.IO.Path.Combine(
                     System.IO.Path.GetDirectoryName(
                     this.GetType().Assembly.Location),
                     "Printings", GlobalCounters.numberOfCurrentPrintings.ToString() + ".pdf");


                    var success = await _mainBrowser.PrintToPdfAsync(path, new PdfPrintSettings
                    {
                        MarginType = CefPdfPrintMarginType.Custom,
                        MarginBottom = 10,
                        MarginTop = 0,
                        MarginLeft = 20,
                        MarginRight = 10,
                    });

                    System.Threading.Thread.Sleep(3000);


                    if (success)
                    {

                        try
                        {
                            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(path);
                            int numberOfPages = pdfReader.NumberOfPages;

                        if (GlobalCounters.numberOfCurrentPrintings + numberOfPages <= Convert.ToInt32(this.numberOfAvailablePagesToPrint))
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

                            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                            info.UseShellExecute = true;
                                info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                                info.Verb = "print";
                                info.FileName = path;
                                info.CreateNoWindow = true;


                                System.Diagnostics.Process p = new System.Diagnostics.Process();
                                p.StartInfo = info;
                                p.Start();

                            // p.WaitForInputIdle();
                            p.WaitForExit();
                            p.CloseMainWindow();

                            try
                            {
                                TaskbarManager.HideTaskbar();
                            }
                            catch { }

                            if (numberOfPages < Int32.Parse(this.numberOfAvailablePagesToPrint))
                            {
                                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(6));
                            }
                            else
                            {
                                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(6));
                            }

                            try
                            {
                                TaskbarManager.HideTaskbar();
                            }
                            catch { }

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
                                this.sender.SendAction("Printed " + numberOfPages + " pages.");

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

                        this.log.Info("Exception: " + ex.ToString());
                        }
                    }

                //else
                //{

                //  System.Windows.MessageBox.Show("Failed to print cause unexptected problem! Please try again..");

                //}
               
            }
          /*  try
            {
                KillAdobe("AcroRd32");
            }
            catch { }*/
        }

        private static bool KillAdobe(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses().Where(
                         clsProcess => clsProcess.ProcessName.StartsWith(name)))
            {
                clsProcess.Kill();
                return true;
            }
            return false;
        }

        public void KillAdobeProcess()
        {
            Process[] processes = Process.GetProcessesByName("AcroRd32");
            foreach(var process in processes)
            {
                process.Kill();
            }
        }

        public bool CanPrint(string path)
        {
            PdfReader pdfReader = new PdfReader(path);
            int numberOfPages = pdfReader.NumberOfPages;

            if (numberOfPages < 4)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
