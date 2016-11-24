using Caliburn.Micro;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Linq;
using Iap.Unitilities;

namespace Iap
{
    public class UserActivityLogger : ILog
    {
        private readonly Type type;
        private readonly string storeId;
        private readonly CultureInfo culture;

        public UserActivityLogger(CultureInfo culture)
            : this(null, culture)
        {
        }

        public UserActivityLogger(Type type, CultureInfo culture)
        {
            this.type = type;
            this.culture = culture;
        }

        public void Error(Exception exception)
        {
            
        }

        public void Info(string format, params object[] args)
        {
            var logDirectory =
                Path.Combine(
                    Path.GetDirectoryName(
                        Assembly.GetExecutingAssembly().Location),
                    "Log");

            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            var logFile =
                Path.Combine(
                    logDirectory,
                    DateTime.Now.Date.ToString("yyyy-MM-dd") + ".txt");

            using (var fs = File.Open(logFile, FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                try
                {
                    if (format.StartsWith("Invoking") ||
                        format.StartsWith("Printing"))
                    {
                        var logMessage =
                            "[" + TimeProvider.Current.UtcNow
                                .ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss tt", this.culture) + "] " +
                            
                             string.Format(format, args);
                       
                            sw.WriteLine(logMessage);
                    }
                }
                finally
                {
                    sw.Close();
                }
            }
        }

        public void Warn(string format, params object[] args)
        {
            
        }
    }
}
