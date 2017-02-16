using System;
using System.Collections.Generic;
using System.IO;
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


        public static string getDiscalimerEnglishText()
        {
            StringBuilder sb = new StringBuilder();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\DiscalimerEn.txt";

            //  System.Windows.MessageBox.Show(path);
            using (StreamReader sr = new StreamReader(path))
            {
                String line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            string allines = sb.ToString();
            // System.Windows.MessageBox.Show(allines);
            return allines;
        }

        public static string getDiscalimerGreekText()
        {
            StringBuilder sb = new StringBuilder();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\DiscalimerGr.txt";

            //  System.Windows.MessageBox.Show(path);
            using (StreamReader sr = new StreamReader(path))
            {
                String line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            string allines = sb.ToString();
            // System.Windows.MessageBox.Show(allines);
            return allines;
        }
    }
}
