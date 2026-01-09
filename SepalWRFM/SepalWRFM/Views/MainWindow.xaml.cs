using System.Linq;
using System.Windows;
using Prism.Regions;
using SepalWRFM.Core;
using Syncfusion.Windows.Shared;

namespace SepalWRFM.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ChromelessWindow
    {
        private readonly IRegionManager _regionManager;

        public MainWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            _regionManager = regionManager;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Fallback: Ensure navigation happens if module didn't navigate
            try
            {
                var region = _regionManager.Regions[RegionNames.ContentRegion];
                if (region != null && !region.Views.Any())
                {
                    System.Diagnostics.Debug.WriteLine("No views in region, attempting navigation...");
                    _regionManager.RequestNavigate(RegionNames.ContentRegion, "AppsLandingView");
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MainWindow navigation error: {ex.Message}");
            }
        }
    }
}
