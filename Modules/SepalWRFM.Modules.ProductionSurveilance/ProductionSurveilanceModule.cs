using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SepalWRFM.Core;
using SepalWRFM.Modules.ProductionSurveilance.Views;
using SepalWRFM.Services.Interfaces;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace SepalWRFM.Modules.ProductionSurveilance
{
    public class ProductionSurveilanceModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly ILogger _logger;

        public ProductionSurveilanceModule(IRegionManager regionManager, ILogger logger)
        {
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _logger.Debug("ProductionSurveilanceModule initialized");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ProductionSurveilanceView>("ProductionSurveilanceView");
            _logger.Debug("ProductionSurveilanceView registered for navigation");
        }
    }
}