using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using SepalWRFM.Modules.ModuleName.ViewModels;

namespace SepalWRFM.Modules.ModuleName.Views
{
    /// <summary>
    /// Interaction logic for AppsLandingView.xaml
    /// </summary>
    public partial class AppsLandingView : UserControl
    {
        private AppsLandingViewModel _viewModel;

        public AppsLandingView()
        {
            InitializeComponent();
            this.DataContextChanged += AppsLandingView_DataContextChanged;
            this.Loaded += AppsLandingView_Loaded;
        }

        private void AppsLandingView_Loaded(object sender, RoutedEventArgs e)
        {
            // Ensure theme is applied on load
            _viewModel = this.DataContext as AppsLandingViewModel;
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;
                UpdateThemeResources(_viewModel.IsDarkTheme);
            }
        }

        private void AppsLandingView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }

            _viewModel = e.NewValue as AppsLandingViewModel;
            
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;
                UpdateThemeResources(_viewModel.IsDarkTheme);
            }
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AppsLandingViewModel.IsDarkTheme) && _viewModel != null)
            {
                System.Diagnostics.Debug.WriteLine($"Theme changed to: {(_viewModel.IsDarkTheme ? "Dark" : "Light")}");
                UpdateThemeResources(_viewModel.IsDarkTheme);
            }
        }

        private void UpdateThemeResources(bool isDarkTheme)
        {
            var localResources = this.Resources;
            var appResources = Application.Current.Resources;
            
            System.Diagnostics.Debug.WriteLine($"Updating theme resources to: {(isDarkTheme ? "Dark" : "Light")}");
            
            var keysToUpdate = new[] {
                ("SidebarBackground", isDarkTheme ? "#1F1F1F" : "#FAF9F8"),
                ("MainBackground", isDarkTheme ? "#121212" : "#FFFFFF"),
                ("TextPrimary", isDarkTheme ? "#FFFFFF" : "#323130"),
                ("TextSecondary", isDarkTheme ? "#B0B0B0" : "#8A8886"),
                ("HoverBackground", isDarkTheme ? "#2D2D2D" : "#F3F2F1"),
                ("SelectedBackground", isDarkTheme ? "#3D3D3D" : "#EDEBE9"),
                ("BorderColor", isDarkTheme ? "#3D3D3D" : "#EDEBE9"),
                ("SidebarHover", isDarkTheme ? "#2D2D2D" : "#F3F2F1"),
                ("SidebarSelected", isDarkTheme ? "#3D3D3D" : "#EDEBE9"),
                ("SidebarBorder", isDarkTheme ? "#3D3D3D" : "#E1DFDD"),
                ("SidebarTextSecondary", isDarkTheme ? "#A19F9D" : "#605E5C"),
                ("CardHoverBorder", isDarkTheme ? "#4D4D4D" : "#C8C6C4"),
                ("AppCardBackground", isDarkTheme ? "#1E1E1E" : "#FFFFFF"),
                ("AppCardBorder", isDarkTheme ? "#3D3D3D" : "#EDEBE9")
            };
            
            foreach (var (key, colorHex) in keysToUpdate)
            {
                UpdateColorResource(localResources, key, colorHex);
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
        
        private System.Collections.Generic.IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void UpdateColorResource(ResourceDictionary resources, string key, string colorHex)
        {
            if (resources.Contains(key))
            {
                var newBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHex));
                resources[key] = newBrush;
                System.Diagnostics.Debug.WriteLine($"Updated resource '{key}' to color {colorHex}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Resource '{key}' not found!");
            }
        }

        private void AppCard_Click(object sender, RoutedEventArgs e)
        {
            // Handle app card menu click (dots button) - stop propagation so it doesn't trigger the card click
            e.Handled = true;
            System.Diagnostics.Debug.WriteLine("Menu button clicked - stopping event propagation");
        }

        private void AccountMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountMenuPopup != null)
            {
                AccountMenuPopup.IsOpen = !AccountMenuPopup.IsOpen;
            }
        }

        private void AccountMenuPopup_Closed(object sender, EventArgs e)
        {
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountMenuPopup != null)
            {
                AccountMenuPopup.IsOpen = false;
            }
        }
    }
}
