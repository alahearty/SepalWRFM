using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SepalWRFM.Core;
using SepalWRFM.Modules.WRFM.Views;

namespace SepalWRFM.Modules.WRFM
{
    public class WRFMModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SepalAppView>(AppConstants.ViewNames.SepalApp);
            containerRegistry.RegisterForNavigation<WRFMLandingView>("WRFMLandingView");
        }
    }
}