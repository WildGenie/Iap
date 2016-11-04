using System;
using System.Windows;
using CefSharp;
using Iap.Handler;

namespace Iap
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
           /* AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            const bool multiThreadedMessageLoop = true;

            IBrowserProcessHandler browserProcessHandler;

            if (multiThreadedMessageLoop)
            {
                browserProcessHandler = new BrowserProcessHandler();
            }
            else
            {
                browserProcessHandler = new WpfBrowserProcessHandler(Dispatcher);
            }*/

            //  CefExample.Init(osr: true, multiThreadedMessageLoop: multiThreadedMessageLoop, browserProcessHandler: browserProcessHandler);
           // Cef.Initialize(new CefSettings(), true, browserProcessHandler);

            base.OnStartup(e);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // put your tracing or logging code here (I put a message box as an example)
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        public App()
        {
            InitializeComponent();
            var settings = new CefSettings();

            IBrowserProcessHandler browserProcessHandler;
            browserProcessHandler = new BrowserProcessHandler();
            // settings.EnableInternalPdfViewerOffScreen();
            // settings.CefCommandLineArgs.Add("disable-gpu", "0");
            settings.CefCommandLineArgs["touch-events"] = "enabled";
            settings.PackLoadingDisabled = false;
           //  Cef.Initialize(settings, shutdownOnProcessExit: true, performDependencyCheck: true);
            Cef.Initialize(settings);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Cef.Shutdown();
            Application.Current.Shutdown();
            base.OnExit(e);
        }
    }
}
