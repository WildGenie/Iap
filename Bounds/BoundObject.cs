using CefSharp;
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
        private int numberOfAvailablePages;

        public BoundObject(int numberOfAvailablePages)
        {
            this.numberOfAvailablePages = numberOfAvailablePages;
        }

        public int NumberOfAvailblePages
        {
            set
            {
                this.numberOfAvailablePages = value;
            }
            get
            {
                return this.numberOfAvailablePages;
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

                _mainBrowser.ExecuteScriptAsync(
                    @"var elements = document.getElementsByClassName('ndfHFb-c4YZDc-to915-LgbsSe ndfHFb-c4YZDc-C7uZwb-LgbsSe VIpgJd-TzA9Ye-eEGnhe ndfHFb-c4YZDc-LgbsSe ndfHFb-c4YZDc-C7uZwb-LgbsSe-SfQLQb-Bz112c');
                    elements[1].style.display = 'none';
                    ");
             
        }

        public void OnSelected(string selected, string noOfPages)
        {
            if (selected == "before")
            {
                string path = System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(
                        this.GetType().Assembly.Location),
                        "Printings", "fileToPrint.pdf");
                _mainBrowser.PrintToPdfAsync(path);
                System.Threading.Thread.Sleep(3000);
                if (this.CanPrint(path))
                {
                    Process p = new Process();
                    p.StartInfo = new ProcessStartInfo()
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                        Verb = "print",
                        FileName = path
                    };
                    p.Start();
                }

                else
                {
                    System.Windows.MessageBox.Show("many docs to print");
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
