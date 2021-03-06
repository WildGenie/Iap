﻿using Caliburn.Micro;
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
   public class CustomBoundObjectEl
    {
        private readonly string numberOfAvailablePagesToPrint;
        ChromiumWebBrowser _mainBrowser;
        private readonly ILog log;
        private readonly ISendStatsService sender;
        private readonly IEventAggregator events;

        public CustomBoundObjectEl(string numberOfAvailablePagesToPrint, ILog log, ISendStatsService sender, IEventAggregator events)
        {
            this.numberOfAvailablePagesToPrint = numberOfAvailablePagesToPrint;
            this.log = log;
            this.sender = sender;
            this.events = events;
        }

        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            
            _mainBrowser = sender as ChromiumWebBrowser;

            try
            {
                string scriptToHideOldVersion = "document.getElementsByClassName('pdp-psy og-pdp')[0].style.visibility = 'hidden'";
                _mainBrowser.ExecuteScriptAsync(scriptToHideOldVersion);
            }
            catch { }

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
                this.events.PublishOnUIThread(new ViewStartPrintProgressCommand());
            }

            catch
            {

            }

            System.Threading.Thread.Sleep(2000);

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
                            catch { }

                            try
                            {
                                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                                info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                                info.Verb = "print";
                                info.FileName = path;
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

                                try
                                {
                                    TaskbarManager.HideTaskbar();
                                }
                                catch { }
                            }
                            catch { }
                            GlobalCounters.numberOfCurrentPrintings += numberOfPages;
                            

                            try
                            {
                                TaskbarManager.HideTaskbar();
                            }
                            catch { }

                            System.Threading.Thread.Sleep(6000);

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
                            System.Windows.MessageBox.Show("Δυστυχώς, δεν μπορείτε να εκτυπώσετε τόσσες σελίδες. Πατήστε OK για να συνεχίσετε.");
                        }


                    }
                    catch (Exception ex)
                    {

                        this.log.Info("Exception: " + ex.ToString());
                    }
                }

               // else
                //{

                  //  System.Windows.MessageBox.Show("Αποτυχία εκτύπωσης! Παρακαλώ, δοκιμάστε ξανά..");

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
