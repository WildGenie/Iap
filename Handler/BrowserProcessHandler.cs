using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iap.Handler
{
   public class BrowserProcessHandler: IBrowserProcessHandler
    {
        protected const int MaxTimerDelay = 100000000 / 30;  // 30fps

        void IBrowserProcessHandler.OnContextInitialized()
        {
            System.Windows.MessageBox.Show("initialized");
            var cookieManager = Cef.GetGlobalCookieManager();
          //  cookieManager.SetStoragePath("cookies", true);
           // cookieManager.SetSupportedSchemes("custom");

           
            using (var context = Cef.GetGlobalRequestContext())
            {
                string errorMessage;
                context.SetPreference("webkit.webprefs.plugins_enabled", true, out errorMessage);
            }
        }

        void IBrowserProcessHandler.OnScheduleMessagePumpWork(long delay)
        {
            //If the delay is greater than the Maximum then use MaxTimerDelay
            //instead - we do this to achieve a minimum number of FPS
            if (delay > MaxTimerDelay)
            {
                delay = MaxTimerDelay;
            }
            OnScheduleMessagePumpWork((int)delay);
        }

        protected virtual void OnScheduleMessagePumpWork(int delay)
        {
            //TODO: Schedule work on the UI thread - call Cef.DoMessageLoopWork
        }

        public virtual void Dispose()
        {

        }
    }
}
