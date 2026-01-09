using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SepalWRFM.Core;
using SepalWRFM.Modules.ModuleName.Views;
using SepalWRFM.Services.Interfaces;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace SepalWRFM.Modules.ModuleName
{
    public class ModuleNameModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly ILogger _logger;

        public ModuleNameModule(IRegionManager regionManager, ILogger logger)
        {
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Ensure navigation happens after the region is ready
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Loaded,
                new Action(() => NavigateToAppsLanding()));
        }

        private void NavigateToAppsLanding()
        {
            try
            {
                _logger.Debug("ModuleNameModule: Attempting navigation to AppsLandingView");
                
                if (_regionManager?.Regions == null)
                {
                    _logger.Error("RegionManager or Regions is null");
                    return;
                }

                var region = _regionManager.Regions[RegionNames.ContentRegion];
                if (region != null)
                {
                    _logger.Debug($"ContentRegion found with {region.Views.Count()} view(s)");
                    _regionManager.RequestNavigate(
                        RegionNames.ContentRegion,
                        AppConstants.ViewNames.AppsLanding,
                        result =>
                        {
                            if (result.Result == true)
                            {
                                _logger.Info("Navigation to AppsLandingView succeeded");
                            }
                            else
                            {
                                _logger.Error($"Navigation to AppsLandingView failed: {result.Error?.Message}", result.Error);
                            }
                        });
                }
                else
                {
                    _logger.Error("ContentRegion not found");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Navigation error in ModuleNameModule", ex);
            }
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            if (containerRegistry == null)
            {
                throw new ArgumentNullException(nameof(containerRegistry));
            }

            containerRegistry.RegisterForNavigation<AppsLandingView>(AppConstants.ViewNames.AppsLanding);
            containerRegistry.RegisterForNavigation<SettingsView>(AppConstants.ViewNames.Settings);
            containerRegistry.RegisterForNavigation<ViewA>(AppConstants.ViewNames.ViewA);
            containerRegistry.RegisterForNavigation<SepalAppView>(AppConstants.ViewNames.SepalApp);
            
            _logger.Info("Registered module views for navigation");
        }
    }
}