using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SepalWRFM.Modules.ModuleName.ViewModels;

namespace SepalWRFM.Modules.ModuleName.Views
{
    /// <summary>
    /// Interaction logic for SepalAppView.xaml
    /// </summary>
    public partial class SepalAppView : UserControl
    {
        private AppsLandingViewModel _sharedViewModel;

        public SepalAppView()
        {
            InitializeComponent();
            this.DataContextChanged += SepalAppView_DataContextChanged;
            this.Loaded += SepalAppView_Loaded;
        }

        private void SepalAppView_Loaded(object sender, RoutedEventArgs e)
        {
            _sharedViewModel = GetSharedLandingViewModel();
            if (_sharedViewModel != null)
            {
                _sharedViewModel.PropertyChanged += SharedViewModel_PropertyChanged;
            }
            UpdateThemeResources();
        }

        private void SepalAppView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_sharedViewModel != null)
            {
                _sharedViewModel.PropertyChanged -= SharedViewModel_PropertyChanged;
            }
            _sharedViewModel = GetSharedLandingViewModel();
            if (_sharedViewModel != null)
            {
                _sharedViewModel.PropertyChanged += SharedViewModel_PropertyChanged;
            }
            UpdateThemeResources();
        }

        private void SharedViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AppsLandingViewModel.IsDarkTheme))
            {
                UpdateThemeResources();
            }
        }

        private void UpdateThemeResources()
        {
            var resources = this.Resources;
            var appResources = Application.Current.Resources;

            var isDarkTheme = GetCurrentTheme();

            var keysToUpdate = new[] {
                ("SidebarBackground", isDarkTheme ? "#1F1F1F" : "#FAF9F8"),
                ("MainBackground", isDarkTheme ? "#121212" : "#FFFFFF"),
                ("TextPrimary", isDarkTheme ? "#FFFFFF" : "#323130"),
                ("TextSecondary", isDarkTheme ? "#B0B0B0" : "#8A8886"),
                ("HoverBackground", isDarkTheme ? "#2D2D2D" : "#F3F2F1"),
                ("SelectedBackground", isDarkTheme ? "#3D3D3D" : "#EDEBE9"),
                ("BorderColor", isDarkTheme ? "#3D3D3D" : "#EDEBE9"),
                ("SelectedIndicator", isDarkTheme ? "#0078D4" : "#0078D4"),
                ("SidebarBorder", isDarkTheme ? "#3D3D3D" : "#E1DFDD"),
                ("AppCardBackground", isDarkTheme ? "#1E1E1E" : "#FFFFFF"),
                ("AppCardBorder", isDarkTheme ? "#3D3D3D" : "#EDEBE9")
            };

            foreach (var (key, colorHex) in keysToUpdate)
            {
                UpdateColorResource(resources, key, colorHex);
                UpdateColorResource(appResources, key, colorHex);
            }
        }

        private bool GetCurrentTheme()
        {
            if (Application.Current.Resources.Contains("IsDarkTheme"))
            {
                return (bool)Application.Current.Resources["IsDarkTheme"];
            }
            
            var landingViewModel = GetSharedLandingViewModel();
            if (landingViewModel != null)
            {
                return landingViewModel.IsDarkTheme;
            }
            
            return true;
        }

        private AppsLandingViewModel GetSharedLandingViewModel()
        {
            return AppsLandingViewModel.Instance;
        }

        private void UpdateColorResource(ResourceDictionary resources, string key, string colorHex)
        {
            if (resources.Contains(key))
            {
                var newBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHex));
                resources[key] = newBrush;
            }
        }
    }
}
