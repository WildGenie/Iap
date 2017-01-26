using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iap
{
  internal static  class GlobalText
    {
        public static string DisclaimerEnglish = System.Configuration.ConfigurationManager.AppSettings["disclaimerEn"];

        public static string DisclaimerGreek = System.Configuration.ConfigurationManager.AppSettings["disclaimerGr"];

        public static string beforeStartPrintingUrl = "";
    }
}
