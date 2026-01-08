using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SepalWRFM.Modules.ModuleName.ViewModels;

namespace SepalWRFM.Modules.ModuleName.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private SettingsViewModel _viewModel;

        public SettingsView()
        {
            InitializeComponent();
            this.DataContextChanged += SettingsView_DataContextChanged;
            this.Loaded += SettingsView_Loaded;
        }

        private void SettingsView_Loaded(object sender, RoutedEventArgs e)
        {
            // Ensure theme is applied on load
            _viewModel = this.DataContext as SettingsViewModel;
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;
                UpdateThemeResources(_viewModel.IsDarkTheme);
            }
        }

        private void SettingsView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }

            _viewModel = e.NewValue as SettingsViewModel;
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;
                UpdateThemeResources(_viewModel.IsDarkTheme);
            }
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_viewModel != null)
            {
                if (e.PropertyName == nameof(SettingsViewModel.IsDarkTheme))
                {
                    System.Diagnostics.Debug.WriteLine($"SettingsView: Theme changed to: {(_viewModel.IsDarkTheme ? "Dark" : "Light")}");
                    UpdateThemeResources(_viewModel.IsDarkTheme);
                }
                else if (e.PropertyName == nameof(SettingsViewModel.IsLightTheme))
                {
                    // Also update when IsLightTheme changes (it's a computed property)
                    UpdateThemeResources(_viewModel.IsDarkTheme);
                }
            }
        }

        private void UpdateThemeResources(bool isDarkTheme)
        {
            var resources = this.Resources;
            var appResources = Application.Current.Resources;

            var keysToUpdate = new[] {
                ("SidebarBackground", isDarkTheme ? "#1F1F1F" : "#FAF9F8"),
                ("MainBackground", isDarkTheme ? "#121212" : "#FFFFFF"),
                ("TextPrimary", isDarkTheme ? "#FFFFFF" : "#323130"),
                ("TextSecondary", isDarkTheme ? "#B0B0B0" : "#8A8886"),
                ("HoverBackground", isDarkTheme ? "#2D2D2D" : "#F3F2F1"),
                ("SelectedBackground", isDarkTheme ? "#3D3D3D" : "#EDEBE9"),
                ("BorderColor", isDarkTheme ? "#3D3D3D" : "#EDEBE9"),
                ("PrimaryButton", isDarkTheme ? "#6264A7" : "#6264A7"), // Same for both themes
                ("SidebarHover", isDarkTheme ? "#2D2D2D" : "#F3F2F1"),
                ("SidebarSelected", isDarkTheme ? "#3D3D3D" : "#EDEBE9"),
                ("SidebarBorder", isDarkTheme ? "#3D3D3D" : "#E1DFDD"),
                ("SidebarTextSecondary", isDarkTheme ? "#A19F9D" : "#605E5C"),
                ("CardHoverBorder", isDarkTheme ? "#4D4D4D" : "#C8C6C4"),
                ("AppCardBackground", isDarkTheme ? "#1E1E1E" : "#FFFFFF"),
                ("AppCardBorder", isDarkTheme ? "#3D3D3D" : "#EDEBE9"),
                ("SelectedIndicator", isDarkTheme ? "#0078D4" : "#0078D4"), // Same for both themes
                ("SettingsNavBackground", isDarkTheme ? "#1F1F1F" : "#FAF9F8"),
                ("SettingsContentBackground", isDarkTheme ? "#252423" : "#FFFFFF")
            };

            foreach (var (key, colorHex) in keysToUpdate)
            {
                UpdateColorResource(resources, key, colorHex);
                UpdateColorResource(appResources, key, colorHex);
            }

            this.Dispatcher.Invoke(() =>
            {
                this.UpdateLayout();
                this.InvalidateVisual();
                this.InvalidateArrange();
                this.InvalidateMeasure();
            }, System.Windows.Threading.DispatcherPriority.Render);
        }

        private void UpdateColorResource(ResourceDictionary resources, string key, string colorHex)
        {
            if (resources.Contains(key))
            {
                var newBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHex));
                resources[key] = newBrush;
            }
        }

        private void ThemeOption_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is string themeTag)
            {
                if (_viewModel != null)
                {
                    switch (themeTag)
                    {
                        case "Light":
                            _viewModel.IsDarkTheme = false;
                            break;
                        case "Dark":
                            _viewModel.IsDarkTheme = true;
                            break;
                    }
                }
            }
        }
    }
}
