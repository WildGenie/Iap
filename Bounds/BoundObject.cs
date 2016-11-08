using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iap.Bounds
{
    public class BoundObject
    {
        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            ChromiumWebBrowser browser = sender as ChromiumWebBrowser;
            browser.ExecuteScriptAsync(@"
                    
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
        }

        public void OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChromiumWebBrowser browser = sender as ChromiumWebBrowser;

            string script =
                            @"(function ()
                    {
                        var isText = false;
                        var activeElement = document.activeElement;
                        if (activeElement) {
                            if (activeElement.tagName.toLowerCase() === 'textarea') {
                                isText = true;
                            } else {
                                if (activeElement.tagName.toLowerCase() === 'input') {
                                    if (activeElement.hasAttribute('type')) {
                                        var inputType = activeElement.getAttribute('type').toLowerCase();
                                        if (inputType === 'text' || inputType === 'email' || inputType === 'password' || inputType === 'tel' || inputType === 'number' || inputType === 'range' || inputType === 'search' || inputType === 'url') {  
                                        isText = true;
                                        }
                                    }
                                }
                            }
                        }
                        return isText;
                    })();";
            browser.GetMainFrame().ExecuteJavaScriptAsync(script);
        }

        public void showKBifIsText()
        {
            
        }
    }
}
