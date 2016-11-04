using System;
using System.Windows;
using CefSharp;

namespace Iap
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        public App()
        {
            InitializeComponent();
            var settings = new CefSettings();
             settings.EnableInternalPdfViewerOffScreen();
             settings.CefCommandLineArgs.Add("disable-gpu", "0");
             settings.CefCommandLineArgs["touch-events"] = "enabled";
            settings.PackLoadingDisabled = false;
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
