using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Prism.Ioc;
using Prism.Regions;
using SepalWRFM.Core;

namespace SepalWRFM.Views
{
    /// <summary>
    /// Interaction logic for SepalWRFMWindow.xaml
    /// </summary>
    public partial class SepalWRFMWindow : Window
    {
        private readonly IRegionManager _scopedRegionManager;

        public SepalWRFMWindow(IRegionManager regionManager, IContainerProvider containerProvider)
        {
            InitializeComponent();
            _scopedRegionManager = new RegionManager();
            
            RegionManager.SetRegionManager(this, _scopedRegionManager);
            
            this.Loaded += SepalWRFMWindow_Loaded;
            this.Closed += SepalWRFMWindow_Closed;
        }

        private void SepalWRFMWindow_Closed(object sender, EventArgs e)
        {
            if (Application.Current.Resources.Contains("ThemeChanged"))
            {
                Application.Current.Resources.Remove("ThemeChanged");
            }
        }

        private void SepalWRFMWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Navigate to SepalAppView when window loads
            try
            {
                // Wait for region to be created by Prism
               Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                    new System.Action(() =>
                    {
                        try
                        {
                            if (_scopedRegionManager != null)
                            {
                                if (_scopedRegionManager.Regions.ContainsRegionWithName(RegionNames.SepalWRFMRegion))
                                {
                                    _scopedRegionManager.RequestNavigate(RegionNames.SepalWRFMRegion, "SepalAppView");
                                    Debug.WriteLine("Navigation to SepalAppView successful");
                                }
                                else
                                {
                                    Debug.WriteLine($"Region '{RegionNames.SepalWRFMRegion}' not found yet, will retry...");
                                    // Retry after a short delay
                                   Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                                        {
                                            if (_scopedRegionManager.Regions.ContainsRegionWithName(RegionNames.SepalWRFMRegion))
                                            {
                                                _scopedRegionManager.RequestNavigate(RegionNames.SepalWRFMRegion, "SepalAppView");
                                            }
                                        }), TimeSpan.FromMilliseconds(100));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"SepalWRFMWindow navigation error: {ex.Message}");
                            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                        }
                    }));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SepalWRFMWindow initialization error: {ex.Message}");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            // Clean up the scoped region manager when window closes
            if (_scopedRegionManager != null)
            {
                RegionManager.SetRegionManager(this, null);
            }
            base.OnClosed(e);
        }
    }
}
