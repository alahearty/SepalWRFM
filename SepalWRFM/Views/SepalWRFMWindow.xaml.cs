using System;
using System.Windows;
using System.Windows.Threading;
using Prism.Regions;
using SepalWRFM.Core;
using SepalWRFM.Services.Interfaces;

namespace SepalWRFM.Views
{
    /// <summary>
    /// Interaction logic for SepalWRFMWindow.xaml
    /// </summary>
    public partial class SepalWRFMWindow : Window
    {
        private readonly IRegionManager _scopedRegionManager;
        private readonly ILogger _logger;

        public SepalWRFMWindow(IRegionManager regionManager, ILogger logger)
        {
            InitializeComponent();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scopedRegionManager = new RegionManager();
            
            RegionManager.SetRegionManager(this, _scopedRegionManager);
            
            this.Loaded += SepalWRFMWindow_Loaded;
            this.Closed += SepalWRFMWindow_Closed;
        }

        private void SepalWRFMWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                // Clean up resources if needed
                if (Application.Current?.Resources != null && Application.Current.Resources.Contains("ThemeChanged"))
                {
                    Application.Current.Resources.Remove("ThemeChanged");
                }
                
                _logger.Debug("SepalWRFMWindow closed");
            }
            catch (Exception ex)
            {
                _logger.Error("Error during window close", ex);
            }
        }

        private void SepalWRFMWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _logger.Debug("SepalWRFMWindow loaded, attempting navigation to SepalAppView");
                
                // Wait for region to be created by Prism
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Loaded,
                    new Action(() => NavigateToSepalAppView()));
            }
            catch (Exception ex)
            {
                _logger.Error("Error during window load", ex);
            }
        }

        private void NavigateToSepalAppView()
        {
            try
            {
                if (_scopedRegionManager == null)
                {
                    _logger.Error("Scoped region manager is null");
                    return;
                }

                if (_scopedRegionManager.Regions.ContainsRegionWithName(RegionNames.SepalWRFMRegion))
                {
                    _scopedRegionManager.RequestNavigate(RegionNames.SepalWRFMRegion, AppConstants.ViewNames.SepalApp);
                    _logger.Info("Navigation to SepalAppView successful");
                }
                else
                {
                    _logger.Warning($"Region '{RegionNames.SepalWRFMRegion}' not found yet, retrying...");
                    
                    // Retry after a short delay
                    Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Loaded,
                        new Action(() =>
                        {
                            if (_scopedRegionManager.Regions.ContainsRegionWithName(RegionNames.SepalWRFMRegion))
                            {
                                _scopedRegionManager.RequestNavigate(RegionNames.SepalWRFMRegion, AppConstants.ViewNames.SepalApp);
                                _logger.Info("Navigation to SepalAppView successful after retry");
                            }
                            else
                            {
                                _logger.Error($"Region '{RegionNames.SepalWRFMRegion}' still not found after retry");
                            }
                        }),
                        TimeSpan.FromMilliseconds(100));
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Navigation error in SepalWRFMWindow", ex);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                // Clean up the scoped region manager when window closes
                if (_scopedRegionManager != null)
                {
                    RegionManager.SetRegionManager(this, null);
                }
            }
            catch (Exception ex)
            {
                _logger?.Error("Error during window cleanup", ex);
            }
            finally
            {
                base.OnClosed(e);
            }
        }
    }
}
