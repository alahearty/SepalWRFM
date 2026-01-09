using System;
using System.Windows;
using System.Windows.Threading;
using Prism.Regions;
using SepalWRFM.Services.Interfaces;
using Syncfusion.Windows.Tools.Controls;

namespace SepalWRFM.Views
{
    /// <summary>
    /// Interaction logic for ProductionAnalysisWindow.xaml
    /// </summary>
    public partial class ProductionAnalysisWindow : Syncfusion.Windows.Shared.ChromelessWindow
    {
        private readonly IRegionManager _scopedRegionManager;
        private readonly ILogger _logger;
        private readonly IThemeService _themeService;
        private DispatcherTimer _statusBarTimer;

        public ProductionAnalysisWindow(IRegionManager regionManager, ILogger logger, IThemeService themeService)
        {
            InitializeComponent();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
            _scopedRegionManager = new RegionManager();
            
            RegionManager.SetRegionManager(this, _scopedRegionManager);
            
            this.Loaded += ProductionAnalysisWindow_Loaded;
            this.Closed += ProductionAnalysisWindow_Closed;
            this.Activated += ProductionAnalysisWindow_Activated;
            
            // Initialize status bar timer
            _statusBarTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _statusBarTimer.Tick += StatusBarTimer_Tick;
        }
        
        private void ProductionAnalysisWindow_Activated(object sender, EventArgs e)
        {
            // Refresh theme when window is activated (in case theme changed while window was inactive)
            UpdateThemeResources(_themeService.IsDarkTheme);
        }
        
        private void UpdateThemeResources(bool isDarkTheme)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    // Update Syncfusion theme using attached property
                    var syncfusionTheme = isDarkTheme 
                        ? Syncfusion.SfSkinManager.VisualStyles.MaterialDark 
                        : Syncfusion.SfSkinManager.VisualStyles.MaterialLight;
                    Syncfusion.SfSkinManager.SfSkinManager.SetVisualStyle(this, syncfusionTheme);
                    
                    // Since ThemeService already updates Application.Current.Resources,
                    // DynamicResource bindings will automatically update
                    // Force UI refresh to ensure all changes are applied
                    this.InvalidateVisual();
                    this.UpdateLayout();
                }, System.Windows.Threading.DispatcherPriority.Render);
            }
            catch (Exception ex)
            {
                _logger.Error("Error updating theme resources", ex);
            }
        }

        private void ProductionAnalysisWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _logger.Info("ProductionAnalysisWindow loaded successfully");
                
                // Apply current theme
                UpdateThemeResources(_themeService.IsDarkTheme);
                
                // Start status bar timer to update time
                _statusBarTimer?.Start();
                UpdateStatusBarTime();
                
                // Initialize Copilot panel as hidden
                if (CopilotPanel != null)
                {
                    DockingManager.SetState(CopilotPanel, DockState.Hidden);
                }
                
                // Navigate to initial view if needed
                // _scopedRegionManager.RequestNavigate("ProductionAnalysisWorkspaceRegion", "SomeView");
            }
            catch (Exception ex)
            {
                _logger.Error("Error loading ProductionAnalysisWindow", ex);
            }
        }
        
        private void CopilotButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CopilotPanel != null && dockingManager != null)
                {
                    var currentState = DockingManager.GetState(CopilotPanel);
                    
                    // Toggle Copilot panel visibility
                    if (currentState == DockState.Hidden || currentState == DockState.AutoHidden)
                    {
                        // Show the Copilot panel (dock it to the right)
                        CopilotPanel.Visibility = Visibility.Visible;
                        DockingManager.SetState(CopilotPanel, DockState.Dock);
                        DockingManager.SetSideInDockedMode(CopilotPanel, DockSide.Right);
                        
                        // Navigate to CopilotView if not already loaded
                        try
                        {
                            if (_scopedRegionManager != null && 
                                _scopedRegionManager.Regions.ContainsRegionWithName(SepalWRFM.Core.RegionNames.CopilotRegion))
                            {
                                var region = _scopedRegionManager.Regions[SepalWRFM.Core.RegionNames.CopilotRegion];
                                if (region.Views.Count() == 0)
                                {
                                    _scopedRegionManager.RequestNavigate(SepalWRFM.Core.RegionNames.CopilotRegion, "CopilotView");
                                }
                            }
                        }
                        catch (Exception navEx)
                        {
                            _logger.Warning("Could not navigate to CopilotView, will retry on next show", navEx);
                        }
                        
                        _logger.Info("Copilot panel shown");
                    }
                    else
                    {
                        // Hide the Copilot panel
                        DockingManager.SetState(CopilotPanel, DockState.Hidden);
                        CopilotPanel.Visibility = Visibility.Collapsed;
                        _logger.Info("Copilot panel hidden");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error toggling Copilot panel", ex);
            }
        }

        private void StatusBarTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatusBarTime();
        }

        private void UpdateStatusBarTime()
        {
            if (StatusBarTime != null)
            {
                StatusBarTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            }
        }

        private void ProductionAnalysisWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                this.Activated -= ProductionAnalysisWindow_Activated;
                
                // Stop and dispose timer
                _statusBarTimer?.Stop();
                _statusBarTimer = null;
                
                if (_scopedRegionManager != null)
                {
                    // Clean up regions if needed
                    _logger.Info("ProductionAnalysisWindow closed");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error closing ProductionAnalysisWindow", ex);
            }
        }
    }
}
