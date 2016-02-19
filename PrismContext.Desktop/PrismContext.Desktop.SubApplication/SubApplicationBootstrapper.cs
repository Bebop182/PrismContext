using System;
using System.Windows;
using Prism.Modularity;
using Prism.Unity;

namespace PrismContext.Desktop.SubApplication
{
    public class SubApplicationBootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return new SubApplicationShell();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            //var moduleCatalog = (ModuleCatalog)ModuleCatalog;

            //Type
            //    main = typeof(PrismContext.Desktop.Main.MainModule),
            //    data = typeof(Data.DataModule),
            //    infra = typeof(Infrastructure.InfrastructureModule);

            //moduleCatalog.AddModule(main, InitializationMode.WhenAvailable, data.Name);
            //moduleCatalog.AddModule(data, InitializationMode.WhenAvailable, infra.Name);
            //moduleCatalog.AddModule(infra, InitializationMode.WhenAvailable);
        }
    }
}
