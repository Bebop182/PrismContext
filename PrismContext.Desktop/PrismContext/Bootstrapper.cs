using System;
using System.Windows;
using Prism.Modularity;
using Prism.Unity;

namespace PrismContext.Desktop
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return new Shell();
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

            var moduleCatalog = (ModuleCatalog)ModuleCatalog;

            Type
                main = typeof (Main.MainModule),
                data = typeof (Data.DataModule);

            moduleCatalog.AddModule(main, InitializationMode.WhenAvailable, data.Name);
            moduleCatalog.AddModule(data, InitializationMode.WhenAvailable);
        }
    }
}
