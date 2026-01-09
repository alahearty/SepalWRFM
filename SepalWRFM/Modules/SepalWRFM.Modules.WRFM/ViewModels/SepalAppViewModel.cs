using Prism.Commands;
using Prism.Regions;
using SepalWRFM.Core;
using SepalWRFM.Core.Mvvm;
using SepalWRFM.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace SepalWRFM.Modules.WRFM.ViewModels
{
    public class WRFMModule
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }
        public string IconKind { get; set; }
        public Brush GradientStartColor { get; set; }
        public Brush GradientEndColor { get; set; }
        public Brush IconBackgroundColor { get; set; }
        public string Category { get; set; }
        public ICommand OpenCommand { get; set; }
    }

    public class RecentDocument
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateModified { get; set; }
        public string DateModifiedText => GetDateModifiedText();
        public Brush IconColor { get; set; }
        public ICommand OpenCommand { get; set; }

        private string GetDateModifiedText()
        {
            var now = DateTime.Now;
            var diff = now - DateModified;

            if (diff.TotalHours < 24)
            {
                var hours = (int)diff.TotalHours;
                if (hours == 0)
                    return "Just now";
                if (hours == 1)
                    return "1h ago";
                return $"{hours}h ago";
            }
            else if (DateModified.Date == now.Date.AddDays(-1))
            {
                return $"Yesterday at {DateModified:h:mm tt}";
            }
            else if (diff.TotalDays < 7)
            {
                return DateModified.ToString("M/d/yyyy");
            }
            else
            {
                return DateModified.ToString("M/d/yyyy");
            }
        }
    }

    public class SepalAppViewModel : RegionViewModelBase
    {
        private readonly IWRFMModuleService _wrfmModuleService;
        private readonly ILogger _logger;
        private readonly IWindowService _windowService;
        private ObservableCollection<WRFMModule> _wrfmModules;
        private ObservableCollection<RecentDocument> _recentDocuments;
        private string _selectedTab = AppConstants.Tabs.Recent;
        private string _greeting;

        public SepalAppViewModel(IRegionManager regionManager, IWRFMModuleService wrfmModuleService, ILogger logger, IWindowService windowService) : base(regionManager)
        {
            _wrfmModuleService = wrfmModuleService ?? throw new ArgumentNullException(nameof(wrfmModuleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            
            InitializeWRFMModules();
            InitializeRecentDocuments();
            UpdateGreeting();
        }

        public ObservableCollection<WRFMModule> WRFMModules
        {
            get => _wrfmModules;
            set => SetProperty(ref _wrfmModules, value);
        }

        public ObservableCollection<RecentDocument> RecentDocuments
        {
            get => _recentDocuments;
            set => SetProperty(ref _recentDocuments, value);
        }

        public string SelectedTab
        {
            get => _selectedTab;
            set => SetProperty(ref _selectedTab, value);
        }

        public string Greeting
        {
            get { return _greeting; }
            set { SetProperty(ref _greeting, value); }
        }

        public ICommand OpenModuleCommand => new DelegateCommand<WRFMModule>(OpenModule);
        public ICommand OpenDocumentCommand => new DelegateCommand<RecentDocument>(OpenDocument);
        public ICommand NavigateTabCommand => new DelegateCommand<string>(NavigateTab);

        private void UpdateGreeting()
        {
            var hour = DateTime.Now.Hour;
            if (hour < 12)
                Greeting = "Good morning";
            else if (hour < 17)
                Greeting = "Good afternoon";
            else
                Greeting = "Good evening";
        }

        private void OpenModule(WRFMModule module)
        {
            if (module == null)
            {
                _logger.Warning("Attempted to open null WRFM module");
                return;
            }

            _logger.Info($"Opening WRFM module: {module.Name}");
            
            try
            {
                _windowService.OpenModuleWindow(module.Name);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to open module window: {module.Name}", ex);
            }
        }

        private void OpenDocument(RecentDocument document)
        {
            if (document == null)
            {
                _logger.Warning("Attempted to open null document");
                return;
            }

            _logger.Info($"Opening document: {document.Name}");
        }

        private void NavigateTab(string tab)
        {
            SelectedTab = tab;
        }

        private void InitializeWRFMModules()
        {
            try
            {
                var moduleData = _wrfmModuleService.GetWRFMModules();
                WRFMModules = new ObservableCollection<WRFMModule>();

                foreach (var data in moduleData)
                {
                    var module = new WRFMModule
                    {
                        Name = data.Name,
                        Description = data.Description,
                        Category = data.Category,
                        IconKind = data.IconKind,
                        GradientStartColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(data.GradientStartColor)),
                        GradientEndColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(data.GradientEndColor)),
                        IconBackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(data.IconBackgroundColor)),
                        OpenCommand = OpenModuleCommand
                    };
                    WRFMModules.Add(module);
                }
                
                _logger.Info($"Initialized {WRFMModules.Count} WRFM modules");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to initialize WRFM modules", ex);
                WRFMModules = new ObservableCollection<WRFMModule>();
            }
        }

        private void InitializeRecentDocuments()
        {
            var now = DateTime.Now;
            RecentDocuments = new ObservableCollection<RecentDocument>
            {
                new RecentDocument
                {
                    Name = "Document 2",
                    Location = "OneDrive",
                    DateModified = now.AddHours(-8),
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    OpenCommand = OpenDocumentCommand
                },
                new RecentDocument
                {
                    Name = "AI Paper for CypherCrescent",
                    Location = "OneDrive",
                    DateModified = now.AddDays(-1).AddHours(-4).AddMinutes(15),
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    OpenCommand = OpenDocumentCommand
                },
                new RecentDocument
                {
                    Name = "NIGCOMSAT Use Case Document",
                    Location = "OneDrive",
                    DateModified = new DateTime(2025, 12, 18),
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    OpenCommand = OpenDocumentCommand
                },
                new RecentDocument
                {
                    Name = "Digital and Innovation Services with Renaissance Ideation Document",
                    Location = "Testing",
                    DateModified = new DateTime(2025, 12, 15),
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    OpenCommand = OpenDocumentCommand
                }
            };
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            UpdateGreeting();
        }
    }
}
