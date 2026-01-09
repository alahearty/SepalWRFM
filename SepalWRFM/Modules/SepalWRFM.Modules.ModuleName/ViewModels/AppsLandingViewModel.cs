using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SepalWRFM.Core;
using SepalWRFM.Core.Mvvm;
using SepalWRFM.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SepalWRFM.Modules.ModuleName.ViewModels
{
    public class AppItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public Brush IconColor { get; set; }
        public bool IsFeatured { get; set; }
        public ICommand ClickCommand { get; set; }
    }

    public class AppsLandingViewModel : RegionViewModelBase
    {
        private ObservableCollection<AppItem> _apps;
        private ObservableCollection<AppItem> _crossPlatformApps;
        private ObservableCollection<AppItem> _workApps;
        private string _selectedSection = "Apps";
        private bool _isSidebarVisible = true;
        private bool _isDarkTheme = true;
        private readonly IWindowService _windowService;
        private static AppsLandingViewModel _instance;

        public AppsLandingViewModel(IRegionManager regionManager, IWindowService windowService) : base(regionManager)
        {
            _windowService = windowService;
            _instance = this;
            InitializeApps();
            OnThemeChanged();
        }

        public static AppsLandingViewModel Instance => _instance;

        public bool IsSidebarVisible
        {
            get { return _isSidebarVisible; }
            set { SetProperty(ref _isSidebarVisible, value); }
        }

        public bool IsDarkTheme
        {
            get { return _isDarkTheme; }
            set 
            { 
                if (SetProperty(ref _isDarkTheme, value))
                {
                    OnThemeChanged();
                }
            }
        }

        public ICommand ToggleSidebarCommand => new DelegateCommand(ToggleSidebar);
        public ICommand ToggleThemeCommand => new DelegateCommand(ToggleTheme);
        public ICommand OpenSettingsCommand => new DelegateCommand(OpenSettings);

        private void ToggleSidebar()
        {
            IsSidebarVisible = !IsSidebarVisible;
        }

        private void ToggleTheme()
        {
            System.Diagnostics.Debug.WriteLine($"ToggleTheme called. Current theme: {(_isDarkTheme ? "Dark" : "Light")}");
            IsDarkTheme = !IsDarkTheme;
            System.Diagnostics.Debug.WriteLine($"Theme toggled to: {(IsDarkTheme ? "Dark" : "Light")}");
        }

        private void OpenSettings()
        {
            // Share this view model with Settings view
            SettingsViewModel.SetSharedViewModel(this);
            
            // Navigate to settings view with current theme
            var parameters = new Prism.Regions.NavigationParameters();
            parameters.Add("IsDarkTheme", IsDarkTheme);
            RegionManager.RequestNavigate(RegionNames.ContentRegion, "SettingsView", parameters);
        }

        private void OnThemeChanged()
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            
            ResourceDictionary themeDictToRemove = null;
            foreach (var dict in mergedDictionaries)
            {
                if (dict.Source != null && 
                    (dict.Source.ToString().Contains("DarkTheme.xaml") || 
                     dict.Source.ToString().Contains("LightTheme.xaml")))
                {
                    themeDictToRemove = dict;
                    break;
                }
            }
            
            if (themeDictToRemove != null)
            {
                mergedDictionaries.Remove(themeDictToRemove);
            }
            
            if (IsDarkTheme)
            {
                mergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/SepalWRFM.Modules.Landing;component/Themes/DarkTheme.xaml") });
            }
            else
            {
                mergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/SepalWRFM.Modules.Landing;component/Themes/LightTheme.xaml") });
            }
            
            Application.Current.Resources["IsDarkTheme"] = IsDarkTheme;
            
            foreach (Window window in Application.Current.Windows)
            {
                if (window != null && window.IsLoaded)
                {
                    window.InvalidateVisual();
                }
            }
        }

        public ObservableCollection<AppItem> Apps
        {
            get { return _apps; }
            set { SetProperty(ref _apps, value); }
        }

        public ObservableCollection<AppItem> CrossPlatformApps
        {
            get { return _crossPlatformApps; }
            set { SetProperty(ref _crossPlatformApps, value); }
        }

        public ObservableCollection<AppItem> WorkApps
        {
            get { return _workApps; }
            set { SetProperty(ref _workApps, value); }
        }

        public string SelectedSection
        {
            get { return _selectedSection; }
            set { SetProperty(ref _selectedSection, value); }
        }

        public ICommand NavigateCommand => new DelegateCommand<string>(Navigate);
        public ICommand AppClickCommand => new DelegateCommand<AppItem>(OnAppClick);

        private void Navigate(string section)
        {
            SelectedSection = section;
        }

        private void OnAppClick(AppItem app)
        {
            // Handle app click - can navigate to app details or launch app
            System.Diagnostics.Debug.WriteLine($"Clicked on {app?.Name ?? "null"}");
            
            if (app == null)
            {
                System.Diagnostics.Debug.WriteLine("AppItem is null!");
                return;
            }
            
            // Open SepalWRFMWindow when SEPAL WRFM is clicked
            if (app.Name == "SEPAL WRFM")
            {
                System.Diagnostics.Debug.WriteLine("Opening SEPAL WRFM Window...");
                try
                {
                    if (_windowService == null)
                    {
                        System.Diagnostics.Debug.WriteLine("WindowService is null!");
                        return;
                    }
                    _windowService.OpenSepalWRFMWindow();
                    System.Diagnostics.Debug.WriteLine("Window opened successfully");
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error opening window: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                }
            }
        }

        private void InitializeApps()
        {
            Apps = new ObservableCollection<AppItem>
            {
                new AppItem
                {
                    Name = "SEPAL WRFM",
                    Description = "Production Analysis, Geology, Petrophysics",
                    Icon = "Email",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    IsFeatured = true,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "BFS",
                    Description = "Business Forecasting System.",
                    Icon = "FileDocument",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#185ABD")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "EPS",
                    Description = "Enterprise Planning System",
                    Icon = "Table",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#107C41")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "SMBS",
                    Description = "Analytic tool",
                    Icon = "FilePowerpoint",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D24726")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "DIAP",
                    Description = "Drilling and Intervention Services",
                    Icon = "Notebook",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7719AA")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "REINEUR",
                    Description = "Store and share files.",
                    Icon = "Cloud",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "ESTURDI",
                    Description = "Chat, meet, and collaborate.",
                    Icon = "MicrosoftTeams",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6264A7")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "DOKU",
                    Description = "Create and edit videos.",
                    Icon = "Video",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B5CF6")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "SharePoint",
                    Description = "Collaborate and share content.",
                    Icon = "ShareVariant",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                }
            };

            CrossPlatformApps = new ObservableCollection<AppItem>
            {
                new AppItem
                {
                    Name = "Bookings",
                    Description = "Schedule and manage appointments.",
                    Icon = "CalendarClock",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "Copilot",
                    Description = "AI-powered assistance.",
                    Icon = "Robot",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6B35")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "Org Explorer",
                    Description = "Explore your organization.",
                    Icon = "AccountNetwork",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "Sales",
                    Description = "Manage sales and customers.",
                    Icon = "Tag",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                }
            };

            WorkApps = new ObservableCollection<AppItem>
            {
                new AppItem
                {
                    Name = "Project",
                    Description = "Plan and manage projects.",
                    Icon = "ViewGrid",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "Visio",
                    Description = "Create diagrams and flowcharts.",
                    Icon = "ChartBox",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3955A3")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "Forms",
                    Description = "Create surveys and quizzes.",
                    Icon = "FormSelect",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                }
            };
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            // Initialize when navigated to
        }
    }
}
