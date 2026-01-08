using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SepalWRFM.Core;
using SepalWRFM.Modules.ModuleName.Views;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace SepalWRFM.Modules.ModuleName
{
    public class ModuleNameModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleNameModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Ensure navigation happens after the region is ready
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                new Action(() =>
                {
                    try
                    {
                        Debug.WriteLine("ModuleNameModule: Attempting navigation to AppsLandingView");
                        var region = _regionManager.Regions[RegionNames.ContentRegion];
                        if (region != null)
                        {
                            Debug.WriteLine($"ContentRegion found with {region.Views.Count()} views");
                            _regionManager.RequestNavigate(RegionNames.ContentRegion, "AppsLandingView", 
                                result =>
                                {
                                    if (result.Result == true)
                                    {
                                        Debug.WriteLine("Navigation to AppsLandingView succeeded");
                                    }
                                    else
                                    {
                                        Debug.WriteLine($"Navigation to AppsLandingView failed: {result.Error?.Message}");
                                    }
                                });
                        }
                        else
                        {
                            Debug.WriteLine("ContentRegion not found!");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.WriteLine($"Navigation error: {ex.Message}");
                        Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                    }
                }));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AppsLandingView>();
            containerRegistry.RegisterForNavigation<SettingsView>();
            containerRegistry.RegisterForNavigation<ViewA>();
            containerRegistry.RegisterForNavigation<SepalAppView>();
        }
    }
}