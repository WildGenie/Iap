using Caliburn.Micro;
using CefSharp;
using CefSharp.Wpf;
using Iap.AdornerControl;
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
   public class CustomBoundObjectEl
    {
        private readonly string numberOfAvailablePagesToPrint;
        ChromiumWebBrowser _mainBrowser;
        private readonly ILog log;
        private readonly ISendStatsService sender;

        public CustomBoundObjectEl(string numberOfAvailablePagesToPrint, ILog log, ISendStatsService sender)
        {
            this.numberOfAvailablePagesToPrint = numberOfAvailablePagesToPrint;
            this.log = log;
            this.sender = sender;
        }

        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {


            _mainBrowser = sender as ChromiumWebBrowser;


            _mainBrowser.ExecuteScriptAsync(@"document.onselectstart = function()
        {
            return false;
        }");



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


            _mainBrowser.ExecuteScriptAsync(
                @"var elements = document.getElementsByClassName('ndfHFb-c4YZDc-to915-LgbsSe ndfHFb-c4YZDc-C7uZwb-LgbsSe VIpgJd-TzA9Ye-eEGnhe ndfHFb-c4YZDc-LgbsSe ndfHFb-c4YZDc-C7uZwb-LgbsSe-SfQLQb-Bz112c');
                    elements[1].style.display = 'none';
                    ");

        }

        public async void onPrintRequested(string selected)
        {


            try
            {
                Thread waitThread = new Thread(() =>
                {
                    PleaseWaitWindow wait = new PleaseWaitWindow();
                   
                    wait.ShowDialog();
                    wait.LoadingAdorner.IsAdornerVisible = true;
                    wait.Close();
                    
                });
                waitThread.SetApartmentState(ApartmentState.STA);
                waitThread.Start();
            }

            catch
            {

            }

            if (selected == "before")
            {
                PrinterCanceller.CancelPrint();

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
                            catch { }
                            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                                info.Verb = "print";
                                info.FileName = path;
                                info.CreateNoWindow = true;
                                info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                                System.Diagnostics.Process p = new System.Diagnostics.Process();
                                p.StartInfo = info;
                                p.Start();

                                p.WaitForInputIdle();
                                System.Threading.Thread.Sleep(3000);
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

                            this.sender.SendAction("Printed " + numberOfPages + " pages.");

                            GlobalCounters.numberOfCurrentPrintings += numberOfPages;

                            }

                            else
                            {
                                System.Windows.MessageBox.Show("Μεγάλος αριθμός σελίδων για εκτύπωση");
                            }

                       
                    }
                    catch (Exception ex)
                    {
                        this.log.Info("Exception: " + ex.ToString());
                    }
                }

            }
            try
            {
                KillAdobe("AcroRd32");
            }
            catch { }
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
