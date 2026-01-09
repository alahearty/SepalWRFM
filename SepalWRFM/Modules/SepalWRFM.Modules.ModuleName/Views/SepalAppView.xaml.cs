using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SepalWRFM.Modules.ModuleName.Views
{
    /// <summary>
    /// Interaction logic for SepalAppView.xaml
    /// </summary>
    public partial class SepalAppView : UserControl
    {
        public SepalAppView()
        {
            InitializeComponent();
            this.Loaded += SepalAppView_Loaded;
        }

        private void SepalAppView_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateThemeResources();
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
            if (Application.Current?.Resources != null && Application.Current.Resources.Contains("IsDarkTheme"))
            {
                return (bool)Application.Current.Resources["IsDarkTheme"];
            }
            
            // Default to dark theme if not set
            return true;
        }

        private void UpdateColorResource(ResourceDictionary resources, string key, string colorHex)
        {
            if (resources != null && resources.Contains(key))
            {
                try
                {
                    var newBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHex));
                    resources[key] = newBrush;
                }
                catch
                {
                    // Ignore color conversion errors
                }
            }
        }
    }
}
