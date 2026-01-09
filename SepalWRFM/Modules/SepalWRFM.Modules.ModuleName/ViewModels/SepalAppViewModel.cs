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
    public class DocumentTemplate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public Brush BackgroundColor { get; set; }
        public ICommand CreateCommand { get; set; }
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
        private ObservableCollection<DocumentTemplate> _templates;
        private ObservableCollection<RecentDocument> _recentDocuments;
        private string _selectedTab = "Recent";
        private string _greeting;

        public SepalAppViewModel(IRegionManager regionManager) : base(regionManager)
        {
            InitializeTemplates();
            InitializeRecentDocuments();
            UpdateGreeting();
        }

        public ObservableCollection<DocumentTemplate> Templates
        {
            get { return _templates; }
            set { SetProperty(ref _templates, value); }
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

        public ICommand CreateDocumentCommand => new DelegateCommand<DocumentTemplate>(CreateDocument);
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

        private void CreateDocument(DocumentTemplate template)
        {
            System.Diagnostics.Debug.WriteLine($"Creating document from template: {template.Name}");
        }

        private void OpenDocument(RecentDocument document)
        {
            System.Diagnostics.Debug.WriteLine($"Opening document: {document.Name}");
        }

        private void NavigateTab(string tab)
        {
            SelectedTab = tab;
        }

        private void InitializeTemplates()
        {
            Templates = new ObservableCollection<DocumentTemplate>
            {
                new DocumentTemplate
                {
                    Name = "Blank document",
                    Description = "Start with a blank page",
                    BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")),
                    CreateCommand = CreateDocumentCommand
                },
                new DocumentTemplate
                {
                    Name = "Welcome to Word",
                    Description = "Take a tour",
                    BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8F4F8")),
                    CreateCommand = CreateDocumentCommand
                },
                new DocumentTemplate
                {
                    Name = "Insert your first table of cont...",
                    Description = "Template",
                    BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F8F8F8")),
                    CreateCommand = CreateDocumentCommand
                },
                new DocumentTemplate
                {
                    Name = "Banner calendar",
                    Description = "January",
                    BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF4E6")),
                    CreateCommand = CreateDocumentCommand
                },
                new DocumentTemplate
                {
                    Name = "Horizontal calendar (Sunday...",
                    Description = "Template",
                    BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0F0F0")),
                    CreateCommand = CreateDocumentCommand
                },
                new DocumentTemplate
                {
                    Name = "Vivid shapes event brochure",
                    Description = "Template",
                    BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E6F3FF")),
                    CreateCommand = CreateDocumentCommand
                },
                new DocumentTemplate
                {
                    Name = "Service invoice (simple lines...",
                    Description = "Template",
                    BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")),
                    CreateCommand = CreateDocumentCommand
                },
                new DocumentTemplate
                {
                    Name = "Invoice (document)",
                    Description = "Template",
                    BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5F5F5")),
                    CreateCommand = CreateDocumentCommand
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
