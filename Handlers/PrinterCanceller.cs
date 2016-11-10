using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using iTextSharp.text.pdf;

namespace Iap.Handlers
{
   internal static class PrinterCanceller
    {
        public static void CancelPrint()
        {
            try
            {
                bool isActionPerformed = false;
                string searchQuery;
                String jobName;
                char[] splitArr;
                int prntJobID;
                ManagementObjectSearcher searchPrintJobs;
                ManagementObjectCollection prntJobCollection;


                searchQuery = "SELECT * FROM Win32_PrintJob";

                searchPrintJobs = new ManagementObjectSearcher(searchQuery);

                prntJobCollection = searchPrintJobs.Get();

                foreach (ManagementObject prntJob in prntJobCollection)
                {
                    jobName = prntJob.Properties["Name"].Value.ToString();

                    splitArr = new char[1];
                    splitArr[0] = Convert.ToChar(",");

                    prntJobID = Convert.ToInt32(jobName.Split(splitArr)[1]);

                    prntJob.Delete();
                    isActionPerformed = true;
                    break;

                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public static bool CanPrint(string path)
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
