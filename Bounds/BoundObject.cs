﻿using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using iTextSharp.text.pdf;
using Iap.Handlers;
using System.Diagnostics;

namespace Iap.Bounds
{
    public class BoundObject
    {
        private int numberOfAvailablePagesToPrint;
        private string language;

        public BoundObject(string language, int numberOfAvailablePagesToPrint)
        {
            this.numberOfAvailablePagesToPrint = numberOfAvailablePagesToPrint;
            this.language = language;
        }

        public int NumberOfAvailblePages
        {
            set
            {
                this.numberOfAvailablePagesToPrint = value;
            }
            get
            {
                return this.numberOfAvailablePagesToPrint;
            }
        }

        public string Language
        {
            set
            {
                this.language = value;
            }

            get
            {
                return this.language;
            }
        }

        ChromiumWebBrowser _mainBrowser;
        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
           

                 _mainBrowser = sender as ChromiumWebBrowser;


            _mainBrowser.ExecuteScriptAsync(@"document.onselectstart = function()
        {
            return false;
        }");

           

            _mainBrowser.ExecuteScriptAsync(@"
                    
                var pageElements = document.getElementsByClassName('ndfHFb-c4YZDc-DARUcf-NnAfwf-j4LONd');
                
                 var noOfPages='0';

                 if(pageElements.length!==0)
                    {
                        noOfPages= pageElements[0].innerText;
                    }       
                
                
                var beforePrint=function(){

                    bound.onPrintRequested('before',noOfPages);
                };        

                var afterPrint = function() {
                   bound.onPrintRequested('after',noOfPages);
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


            var scriptjq = @"(function () {
    // more or less stolen form jquery core and adapted by paul irish
    function getScript(url, success) {
        var script = document.createElement('script');
        script.src = url;
        var head = document.getElementsByTagName('head')[0],
            done = false;
        // Attach handlers for all browsers
        script.onload = script.onreadystatechange = function () {
            if (!done && (!this.readyState
                || this.readyState == 'loaded'
                || this.readyState == 'complete')) {
                done = true;
                success();
                script.onload = script.onreadystatechange = null;
                head.removeChild(script);
            }
        };
        head.appendChild(script);
    }
    getScript('http://code.jquery.com/jquery-latest.min.js', function () {
        if (typeof jQuery == 'undefined') {
            alert('no jquery');
        } else {
            console.log('This page is now jQuerified with v' + $.fn.jquery);

            $(document).ready(function () {
              
                    var mediaQueryList = window.matchMedia('print');
mediaQueryList.addListener(function(mql) {
    if (mql.matches) {
        alert('onafterprint');
    };
});

                //here you can write your jquery code
            });
        }
    });
})();";
            //  _mainBrowser.ExecuteScriptAsync(scriptjq);

            // _mainBrowser.ExecuteScriptAsync(@"document.onload = function () {alert('before');}");

            //_mainBrowser.ExecuteScriptAsync(@"window.print=function() {alert('hello')}");


            var printScript = @"window.print=function() {bound.onPrintRequested('before','0');}";

            _mainBrowser.ExecuteScriptAsync(printScript);


                _mainBrowser.ExecuteScriptAsync(
                    @"var elements = document.getElementsByClassName('ndfHFb-c4YZDc-to915-LgbsSe ndfHFb-c4YZDc-C7uZwb-LgbsSe VIpgJd-TzA9Ye-eEGnhe ndfHFb-c4YZDc-LgbsSe ndfHFb-c4YZDc-C7uZwb-LgbsSe-SfQLQb-Bz112c');
                    elements[1].style.display = 'none';
                    ");
             
        }

        public async void onPrintRequested(string selected, string noOfPages)
        {

             System.Windows.MessageBox.Show("print ok to continue");

           // System.Threading.Thread.Sleep(5000);
            
                if (selected == "before")
                {
                   PrinterCanceller.CancelPrint();

                    string path = System.IO.Path.Combine(
                     System.IO.Path.GetDirectoryName(
                     this.GetType().Assembly.Location),
                     "Printings", GlobalCounters.numberOfCurrentPrintings.ToString()+".pdf");


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
                        if (!(numberOfPages > this.NumberOfAvailblePages))
                        {
                            if (GlobalCounters.numberOfCurrentPrintings + numberOfPages <= this.NumberOfAvailblePages)
                            {

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

                                GlobalCounters.numberOfCurrentPrintings += numberOfPages;

                            }

                            else
                            {
                                if (this.Language == "el")
                                {
                                    System.Windows.MessageBox.Show("Δεν μπορείτε να εκτυπώσετε άλλες σελίδες");
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("Can not print other pages");
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                       // System.Windows.MessageBox.Show(ex.ToString());
                    }
                }

                else
                {
                    if (this.Language == "en")
                    {
                        System.Windows.MessageBox.Show("Failed to print");
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Αποτυχία εκτύπωσης");
                    }
                }
                
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
