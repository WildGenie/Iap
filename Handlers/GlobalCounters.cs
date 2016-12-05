using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iap.Handlers
{
   internal static class GlobalCounters
    {
       public static int numberOfCurrentPrintings = 0;
      

       public static void ResetAll()
        {
            numberOfCurrentPrintings= 0;
        }

        public static void DeletePrintedFiles()
        {
           
        }
      }
}
