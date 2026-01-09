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
        private readonly IWindowService _windowService;
        private readonly IThemeService _themeService;
        private readonly ILogger _logger;
        private readonly IMessageService _messageService;
        private ObservableCollection<AppItem> _apps;
        private ObservableCollection<AppItem> _crossPlatformApps;
        private ObservableCollection<AppItem> _workApps;
        private string _selectedSection = AppConstants.Sections.Apps;
        private bool _isSidebarVisible = true;

        public AppsLandingViewModel(
            IRegionManager regionManager,
            IWindowService windowService,
            IThemeService themeService,
            ILogger logger,
            IMessageService messageService) 
            : base(regionManager)
        {
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            
            InitializeApps();
            
            // Subscribe to theme changes
            _themeService.ApplyTheme(_themeService.IsDarkTheme);
        }

        public bool IsSidebarVisible
        {
            get { return _isSidebarVisible; }
            set { SetProperty(ref _isSidebarVisible, value); }
        }

        public bool IsDarkTheme
        {
            get => _themeService.IsDarkTheme;
            set => _themeService.IsDarkTheme = value;
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
            _logger.Debug($"ToggleTheme called. Current theme: {(IsDarkTheme ? "Dark" : "Light")}");
            _themeService.ToggleTheme();
            _logger.Info($"Theme toggled to: {(IsDarkTheme ? "Dark" : "Light")}");
        }

        private void OpenSettings()
        {
            try
            {
                var parameters = new Prism.Regions.NavigationParameters();
                parameters.Add("IsDarkTheme", IsDarkTheme);
                RegionManager.RequestNavigate(RegionNames.ContentRegion, AppConstants.ViewNames.Settings, parameters);
                _logger.Info("Navigated to Settings view");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to navigate to Settings view", ex);
            }
        }

        public ObservableCollection<AppItem> Apps
        {
            get => _apps;
            set => SetProperty(ref _apps, value);
        }

        public ObservableCollection<AppItem> CrossPlatformApps
        {
            get => _crossPlatformApps;
            set => SetProperty(ref _crossPlatformApps, value);
        }

        public ObservableCollection<AppItem> WorkApps
        {
            get => _workApps;
            set => SetProperty(ref _workApps, value);
        }

        public string SelectedSection
        {
            get => _selectedSection;
            set => SetProperty(ref _selectedSection, value);
        }

        public ICommand NavigateCommand => new DelegateCommand<string>(Navigate);
        public ICommand AppClickCommand => new DelegateCommand<AppItem>(OnAppClick);

        private void Navigate(string section)
        {
            SelectedSection = section;
        }

        private void OnAppClick(AppItem app)
        {
            if (app == null)
            {
                _logger.Warning("Attempted to click on null AppItem");
                return;
            }

            _logger.Debug($"Clicked on app: {app.Name}");

            if (app.Name == AppConstants.AppNames.SepalWRFM)
            {
                try
                {
                    _windowService.OpenSepalWRFMWindow();
                    _logger.Info("SEPAL WRFM window opened successfully");
                }
                catch (Exception ex)
                {
                    _logger.Error("Failed to open SEPAL WRFM window", ex);
                    _messageService?.ShowError("Unable to open SEPAL WRFM window. Please try again or contact support if the issue persists.");
                }
            }
            else
            {
                _logger.Info($"App '{app.Name}' clicked - handler not yet implemented");
                _messageService?.ShowInfo($"{app.Name} is coming soon!");
            }
        }

        private void InitializeApps()
        {
            Apps = new ObservableCollection<AppItem>
            {
                new AppItem
                {
                    Name = AppConstants.AppNames.SepalWRFM,
                    Description = "Production Analysis, Geology, Petrophysics",
                    Icon = "SepalWRFM",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    IsFeatured = true,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "BFS",
                    Description = "Business Forecasting System.",
                    Icon = "BFS",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#185ABD")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "EPS",
                    Description = "Enterprise Planning System",
                    Icon = "EPS",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#107C41")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "SMBS",
                    Description = "Analytic tool",
                    Icon = "SMBS",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D24726")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "DIAP",
                    Description = "Drilling and Intervention Services",
                    Icon = "DIAP",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7719AA")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "REINEUR",
                    Description = "Store and share files.",
                    Icon = "REINEUR",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "ESTURDI",
                    Description = "Chat, meet, and collaborate.",
                    Icon = "ESTURDI",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6264A7")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "DOKU",
                    Description = "Create and edit videos.",
                    Icon = "DOKU",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B5CF6")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "SharePoint",
                    Description = "Collaborate and share content.",
                    Icon = "SharePoint",
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
                    Icon = "Bookings",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "Copilot",
                    Description = "AI-powered assistance.",
                    Icon = "Copilot",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6B35")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "Org Explorer",
                    Description = "Explore your organization.",
                    Icon = "OrgExplorer",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "Sales",
                    Description = "Manage sales and customers.",
                    Icon = "Sales",
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
                    Icon = "Project",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "Visio",
                    Description = "Create diagrams and flowcharts.",
                    Icon = "Visio",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3955A3")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                },
                new AppItem
                {
                    Name = "Forms",
                    Description = "Create surveys and quizzes.",
                    Icon = "Forms",
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    IsFeatured = false,
                    ClickCommand = AppClickCommand
                }
            };
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _logger.Debug("AppsLandingViewModel navigated to");
            // Additional initialization if needed
        }
    }
}
