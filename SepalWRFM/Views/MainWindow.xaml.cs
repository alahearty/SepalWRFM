using System;
using System.Linq;
using System.Windows;
using Prism.Regions;
using SepalWRFM.Core;
using SepalWRFM.Services.Interfaces;
using Syncfusion.Windows.Shared;

namespace SepalWRFM.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ChromelessWindow
    {
        private readonly IRegionManager _regionManager;
        private readonly ILogger _logger;

        public MainWindow(IRegionManager regionManager, ILogger logger)
        {
            InitializeComponent();
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Fallback: Ensure navigation happens if module didn't navigate
            try
            {
                if (_regionManager?.Regions == null)
                {
                    _logger.Warning("RegionManager or Regions is null");
                    return;
                }

                var region = _regionManager.Regions[RegionNames.ContentRegion];
                if (region != null && !region.Views.Any())
                {
                    _logger.Info("No views in region, attempting navigation to AppsLandingView");
                    _regionManager.RequestNavigate(RegionNames.ContentRegion, AppConstants.ViewNames.AppsLanding);
                }
                else
                {
                    _logger.Debug($"ContentRegion has {region?.Views?.Count() ?? 0} view(s)");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("MainWindow navigation error", ex);
            }
        }
    }
}
