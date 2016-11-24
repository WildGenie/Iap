using Caliburn.Micro;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Threading;

namespace Iap.Windsor
{
   internal class AppBootstrapper: BootstrapperBase
    {
        private WindsorContainer container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            this.container = new WindsorContainer();
            this.container.AddFacility<EventRegistrationFacility>();
            this.container.Kernel.Resolver.AddSubResolver(
                new AppSettingsConvention());

            this.container.Install(
                Castle.Windsor.Installer.Configuration.FromAppConfig());
            this.container.Register(
                Component
                    .For<CultureInfo>()
                    .Instance(new CultureInfo("el-GR")),
                // Component
                //    .For<MapsBackground>(),
                Component
                    .For<IWindowManager>()
                    .ImplementedBy<NoBorderWindowManager>(),
                Component
                    .For<IEventAggregator>()
                    .ImplementedBy<EventAggregator>(),
                Classes
                    .FromThisAssembly()
                    .BasedOn<PropertyChangedBase>()
                    .LifestyleTransient(),
                Classes
                    .FromThisAssembly()
                    .Where(x => x.Name.EndsWith("Service"))
                    .WithServiceDefaultInterfaces(),
                Classes
                    .FromThisAssembly()
                    .Where(x => x.Name.EndsWith("Command"))
                    .WithServiceDefaultInterfaces(),
                Classes
                    .FromThisAssembly()
                    .Where(x => x.Name.EndsWith("Query"))
                    .WithServiceDefaultInterfaces(),
                Classes
                    .FromThisAssembly()
                    .Where(x => x.Name.EndsWith("Projection"))
                    .WithServiceDefaultInterfaces());

            this.ConfigureLog();
            this.container
                .Resolve<ILog>()
                .Info("Invoking Action: Start Application.");

            base.Configure();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            if (string.IsNullOrEmpty(key))
                return this.container.Resolve(serviceType);
            return this.container.Resolve(key, serviceType);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return this.container.ResolveAll(serviceType).Cast<object>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            DisplayRootViewFor<AppViewModel>();


            base.OnStartup(sender, e);
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            this.container.Dispose();
            base.OnExit(sender, e);
        }

        private void ConfigureLog()
        {
            this.container.Register(
                Component
                    .For<ILog>()
                    .ImplementedBy<UserActivityLogger>());

            LogManager.GetLog = caliburnType =>
                new UserActivityLogger(
                    caliburnType,
                    this.container.Resolve<CultureInfo>());
        }
    }
}
