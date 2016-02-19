using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace PrismContext.Desktop.Data
{
    public class DataModule : IModule
    {
        private readonly IRegionManager _regionManager, _internRegionManager;
        private readonly IUnityContainer _unityContainer;

        public DataModule(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            _regionManager = regionManager;
            _internRegionManager = regionManager.CreateRegionManager();
            _unityContainer = unityContainer;
        }

        public void Initialize()
        {
            //Register types
            //_unityContainer.RegisterType(typeof(IService), typeof(MockupDataService), new ContainerControlledLifetimeManager());
            //Load Module Settings

            //Register Module shell within application shell region

            //Register view into module shell region
        }
    }
}
