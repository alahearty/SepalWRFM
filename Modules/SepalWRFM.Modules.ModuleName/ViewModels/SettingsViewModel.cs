using Prism.Commands;
using Prism.Regions;
using SepalWRFM.Core;
using SepalWRFM.Core.Mvvm;
using SepalWRFM.Services.Interfaces;
using System;
using System.Windows.Input;

namespace SepalWRFM.Modules.ModuleName.ViewModels
{
    public class SettingsViewModel : RegionViewModelBase
    {
        private readonly IThemeService _themeService;
        private readonly ILogger _logger;
        private bool _isGeneralSelected = true;

        public SettingsViewModel(IRegionManager regionManager, IThemeService themeService, ILogger logger) 
            : base(regionManager)
        {
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool IsDarkTheme
        {
            get => _themeService.IsDarkTheme;
            set
            {
                if (_themeService.IsDarkTheme != value)
                {
                    _themeService.IsDarkTheme = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(IsLightTheme));
                    _logger.Info($"Theme changed to {(value ? "Dark" : "Light")} from Settings");
                }
            }
        }

        public bool IsGeneralSelected
        {
            get => _isGeneralSelected;
            set => SetProperty(ref _isGeneralSelected, value);
        }

        public bool IsLightTheme => !IsDarkTheme;
        public bool IsSystemTheme => false;

        public ICommand CloseSettingsCommand => new DelegateCommand(CloseSettings);
        public ICommand NavigateToSectionCommand => new DelegateCommand<string>(NavigateToSection);

        private void CloseSettings()
        {
            try
            {
                RegionManager.RequestNavigate(RegionNames.ContentRegion, AppConstants.ViewNames.AppsLanding);
                _logger.Info("Navigated back to Apps Landing from Settings");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to navigate back to Apps Landing", ex);
            }
        }

        private void NavigateToSection(string section)
        {
            IsGeneralSelected = section == "General";
        }

        public override void OnNavigatedTo(Prism.Regions.NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            
            // Update theme state from navigation parameters if provided
            if (navigationContext?.Parameters != null && navigationContext.Parameters.ContainsKey("IsDarkTheme"))
            {
                var themeFromParam = navigationContext.Parameters.GetValue<bool>("IsDarkTheme");
                if (themeFromParam != IsDarkTheme)
                {
                    RaisePropertyChanged(nameof(IsDarkTheme));
                    RaisePropertyChanged(nameof(IsLightTheme));
                }
            }
            
            _logger.Debug("SettingsViewModel navigated to");
        }
    }
}
