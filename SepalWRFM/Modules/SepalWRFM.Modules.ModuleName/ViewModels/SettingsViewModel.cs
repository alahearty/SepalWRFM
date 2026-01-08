using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SepalWRFM.Core;
using SepalWRFM.Core.Mvvm;
using System;
using System.Windows.Input;

namespace SepalWRFM.Modules.ModuleName.ViewModels
{
    public class SettingsViewModel : RegionViewModelBase
    {
        private bool _isDarkTheme = true;
        private bool _isGeneralSelected = true;
        private static AppsLandingViewModel _sharedViewModel;

        public SettingsViewModel(IRegionManager regionManager) : base(regionManager)
        {
        }

        public bool IsDarkTheme
        {
            get 
            { 
                // Get theme from shared view model if available
                if (_sharedViewModel != null)
                {
                    return _sharedViewModel.IsDarkTheme;
                }
                return _isDarkTheme; 
            }
            set 
            { 
                if (SetProperty(ref _isDarkTheme, value))
                {
                    RaisePropertyChanged(nameof(IsLightTheme));
                    
                    // Update shared view model
                    if (_sharedViewModel != null)
                    {
                        _sharedViewModel.IsDarkTheme = value;
                    }
                }
            }
        }

        public bool IsGeneralSelected
        {
            get { return _isGeneralSelected; }
            set { SetProperty(ref _isGeneralSelected, value); }
        }

        public bool IsLightTheme => !IsDarkTheme;
        public bool IsSystemTheme => false; // Can implement system theme detection later

        public ICommand CloseSettingsCommand => new DelegateCommand(CloseSettings);
        public ICommand NavigateToSectionCommand => new DelegateCommand<string>(NavigateToSection);

        public static void SetSharedViewModel(AppsLandingViewModel viewModel)
        {
            _sharedViewModel = viewModel;
        }

        private void CloseSettings()
        {
            // Navigate back to Apps landing view
            RegionManager.RequestNavigate(RegionNames.ContentRegion, "AppsLandingView");
        }

        private void NavigateToSection(string section)
        {
            // Handle section navigation if needed
            IsGeneralSelected = section == "General";
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            // Get current theme from shared view model
            if (_sharedViewModel != null)
            {
                if (SetProperty(ref _isDarkTheme, _sharedViewModel.IsDarkTheme, nameof(IsDarkTheme)))
                {
                    RaisePropertyChanged(nameof(IsLightTheme));
                }
            }
        }
    }
}
