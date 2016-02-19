using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace PrismContext.Desktop.Main
{
    public class MainModule : IModule
    {
        private readonly IRegionManager _regionManager, _internRegionManager;
        private readonly IUnityContainer _unityContainer;

        public MainModule(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            _regionManager = regionManager;
            _internRegionManager = regionManager.CreateRegionManager();
            _unityContainer = unityContainer;
        }

        public void Initialize()
        {
            //Register types
            _unityContainer.RegisterType(typeof (MainModuleShell), new ContainerControlledLifetimeManager()); //Register as singleton
            _unityContainer.RegisterType(typeof(Views.Main));
            _unityContainer.RegisterType(typeof(Views.TreeTest));
            _unityContainer.RegisterType(typeof(Views.MindMapView));

            //Load Module Settings
            var moduleSettings = (NameValueCollection)ConfigurationManager.GetSection("regionMapping/mainModule");

            //Register module shell within application shell region
            var moduleShell = _unityContainer.Resolve<MainModuleShell>();
            _regionManager.RegisterViewWithRegion(moduleSettings["Region"], () => moduleShell);

            //Set RegionManager for local module shell
            RegionManager.SetRegionManager(moduleShell, _internRegionManager);

            //Register views into module shell region
            _internRegionManager.RegisterViewWithRegion(RegionNames.CenterRegion, () => _unityContainer.Resolve<Views.Main>());
            _internRegionManager.RegisterViewWithRegion(RegionNames.TopRegion, () => _unityContainer.Resolve<Views.TreeTest>());
            //_internRegionManager.RegisterViewWithRegion(RegionNames.CenterRegion, () => _unityContainer.Resolve<Views.MindMapView>());
        }

        private struct RegionNames
        {
            public const string TopRegion = "Top";
            public const string LeftRegion = "Left";
            public const string CenterRegion = "Center";
            public const string RightRegion = "Right";
        }
    }

    
}
