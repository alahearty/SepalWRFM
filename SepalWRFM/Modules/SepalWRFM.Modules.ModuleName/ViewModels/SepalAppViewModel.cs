using Prism.Commands;
using Prism.Regions;
using SepalWRFM.Core;
using SepalWRFM.Core.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace SepalWRFM.Modules.ModuleName.ViewModels
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
        private ObservableCollection<WRFMModule> _wrfmModules;
        private ObservableCollection<RecentDocument> _recentDocuments;
        private string _selectedTab = "Recent";
        private string _greeting;

        public SepalAppViewModel(IRegionManager regionManager) : base(regionManager)
        {
            InitializeWRFMModules();
            InitializeRecentDocuments();
            UpdateGreeting();
        }

        public ObservableCollection<WRFMModule> WRFMModules
        {
            get { return _wrfmModules; }
            set { SetProperty(ref _wrfmModules, value); }
        }

        public ObservableCollection<RecentDocument> RecentDocuments
        {
            get { return _recentDocuments; }
            set { SetProperty(ref _recentDocuments, value); }
        }

        public string SelectedTab
        {
            get { return _selectedTab; }
            set { SetProperty(ref _selectedTab, value); }
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
            System.Diagnostics.Debug.WriteLine($"Opening WRFM module: {module.Name}");
            // Navigate to the specific module
        }

        private void OpenDocument(RecentDocument document)
        {
            System.Diagnostics.Debug.WriteLine($"Opening document: {document.Name}");
        }

        private void NavigateTab(string tab)
        {
            SelectedTab = tab;
        }

        private void InitializeWRFMModules()
        {
            WRFMModules = new ObservableCollection<WRFMModule>
            {
                new WRFMModule
                {
                    Name = "Production Analysis",
                    Description = "Analyze production trends, decline curves, and performance metrics",
                    Category = "Production",
                    IconKind = "ChartLine",
                    GradientStartColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3C72")),
                    GradientEndColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2A5298")),
                    IconBackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3C72")),
                    OpenCommand = OpenModuleCommand
                },
                new WRFMModule
                {
                    Name = "Production Geology",
                    Description = "Geological mapping, reservoir characterization, and stratigraphy",
                    Category = "Geology",
                    IconKind = "Map",
                    GradientStartColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B4513")),
                    GradientEndColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CD853F")),
                    IconBackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8B4513")),
                    OpenCommand = OpenModuleCommand
                },
                new WRFMModule
                {
                    Name = "Petrophysics",
                    Description = "Rock properties, log analysis, and reservoir evaluation",
                    Category = "Analysis",
                    IconKind = "ChartBar",
                    GradientStartColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D32F2F")),
                    GradientEndColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336")),
                    IconBackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D32F2F")),
                    OpenCommand = OpenModuleCommand
                },
                new WRFMModule
                {
                    Name = "Schematic",
                    Description = "Well schematics, facility diagrams, and network visualization",
                    Category = "Visualization",
                    IconKind = "Network",
                    GradientStartColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00796B")),
                    GradientEndColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#009688")),
                    IconBackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00796B")),
                    OpenCommand = OpenModuleCommand
                },
                new WRFMModule
                {
                    Name = "Well Integrity",
                    Description = "Monitor well integrity, casing condition, and safety compliance",
                    Category = "Safety",
                    IconKind = "ShieldCheck",
                    GradientStartColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F57C00")),
                    GradientEndColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800")),
                    IconBackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F57C00")),
                    OpenCommand = OpenModuleCommand
                },
                new WRFMModule
                {
                    Name = "Reservoir Management",
                    Description = "Reservoir modeling, simulation, and optimization strategies",
                    Category = "Reservoir",
                    IconKind = "Database",
                    GradientStartColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#512DA8")),
                    GradientEndColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#673AB7")),
                    IconBackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#512DA8")),
                    OpenCommand = OpenModuleCommand
                },
                new WRFMModule
                {
                    Name = "Well Testing",
                    Description = "Pressure transient analysis, flow testing, and diagnostics",
                    Category = "Testing",
                    IconKind = "Speedometer",
                    GradientStartColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0288D1")),
                    GradientEndColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03A9F4")),
                    IconBackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0288D1")),
                    OpenCommand = OpenModuleCommand
                },
                new WRFMModule
                {
                    Name = "Facility Management",
                    Description = "Surface facility operations, equipment tracking, and maintenance",
                    Category = "Facilities",
                    IconKind = "Factory",
                    GradientStartColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#388E3C")),
                    GradientEndColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50")),
                    IconBackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#388E3C")),
                    OpenCommand = OpenModuleCommand
                },
                new WRFMModule
                {
                    Name = "Economic Analysis",
                    Description = "Production economics, forecasting, and financial planning",
                    Category = "Economics",
                    IconKind = "CurrencyUsd",
                    GradientStartColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C2185B")),
                    GradientEndColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E91E63")),
                    IconBackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C2185B")),
                    OpenCommand = OpenModuleCommand
                },
                new WRFMModule
                {
                    Name = "Data Analytics",
                    Description = "Advanced analytics, machine learning, and predictive modeling",
                    Category = "Analytics",
                    IconKind = "Brain",
                    GradientStartColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5D4037")),
                    GradientEndColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#795548")),
                    IconBackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5D4037")),
                    OpenCommand = OpenModuleCommand
                }
            };
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
                    Name = "WORIAYIBAPRI HEARTY ALAPHER Resume",
                    Location = "Downloads",
                    DateModified = now.AddDays(-1).AddHours(-4).AddMinutes(26),
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6264A7")),
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
                    Name = "GeoBunker Trace 2",
                    Location = "OneDrive",
                    DateModified = new DateTime(2025, 12, 18),
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    OpenCommand = OpenDocumentCommand
                },
                new RecentDocument
                {
                    Name = "Mini CV WORIAYIBAPRI HEARTY ALAPHER",
                    Location = "OneDrive",
                    DateModified = new DateTime(2025, 12, 18),
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    OpenCommand = OpenDocumentCommand
                },
                new RecentDocument
                {
                    Name = "Digital and Innovation Services with Renaissance Ideation Document",
                    Location = "OneDrive",
                    DateModified = new DateTime(2025, 12, 15),
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    OpenCommand = OpenDocumentCommand
                },
                new RecentDocument
                {
                    Name = "GeoBunkerTrace - A SPATIAL INTELLIGENCE FRAMEWORK FOR MONITORING AND REPORTING ILLEGAL",
                    Location = "OneDrive",
                    DateModified = new DateTime(2025, 12, 11),
                    IconColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0078D4")),
                    OpenCommand = OpenDocumentCommand
                },
                new RecentDocument
                {
                    Name = "WORIAYIBAPRI HEARTY ALAPHER",
                    Location = "OneDrive",
                    DateModified = new DateTime(2025, 12, 11),
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
