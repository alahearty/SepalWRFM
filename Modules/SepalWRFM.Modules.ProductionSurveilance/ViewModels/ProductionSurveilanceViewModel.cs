using Prism.Regions;
using SepalWRFM.Core.Mvvm;
using SepalWRFM.Services.Interfaces;
using System;

namespace SepalWRFM.Modules.ProductionSurveilance.ViewModels
{
    public class ProductionSurveilanceViewModel : RegionViewModelBase
    {
        private readonly ILogger _logger;
        private string _message;

        public ProductionSurveilanceViewModel(IRegionManager regionManager, ILogger logger) 
            : base(regionManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Message = "Production Surveillance Module";
            _logger.Info("ProductionSurveilanceViewModel initialized");
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public override void OnNavigatedTo(Prism.Regions.NavigationContext navigationContext)
        {
            _logger.Info("Navigated to ProductionSurveilanceView");
            base.OnNavigatedTo(navigationContext);
        }
    }
}
