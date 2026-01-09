using System;
using System.Windows;
using SepalWRFM.Services.Interfaces;
using SepalWRFM.Views;
using Prism.Ioc;

namespace SepalWRFM.Services
{
    /// <summary>
    /// Service for managing window operations.
    /// </summary>
    public class WindowService : IWindowService
    {
        private readonly IContainerProvider _containerProvider;
        private readonly ILogger _logger;

        public WindowService(IContainerProvider containerProvider, ILogger logger)
        {
            _containerProvider = containerProvider ?? throw new ArgumentNullException(nameof(containerProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void OpenSepalWRFMWindow()
        {
            try
            {
                _logger.Info("Opening SEPAL WRFM window");
                var window = _containerProvider.Resolve<SepalWRFMWindow>();
                window?.Show();
                _logger.Info("SEPAL WRFM window opened successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to open SEPAL WRFM window", ex);
                throw;
            }
        }

        public void OpenModuleWindow(string moduleName)
        {
            try
            {
                _logger.Info($"Opening module window: {moduleName}");
                
                Window window = null;
                
                switch (moduleName)
                {
                    case "Production":
                    case "Production Analysis":
                        window = _containerProvider.Resolve<ProductionAnalysisWindow>();
                        break;
                    default:
                        _logger.Warning($"No window handler for module: {moduleName}");
                        return;
                }
                
                if (window != null)
                {
                    window.Show();
                    _logger.Info($"Module window '{moduleName}' opened successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to open module window: {moduleName}", ex);
                throw;
            }
        }
    }
}
