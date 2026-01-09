using Prism.Regions;
using SepalWRFM.Core.Mvvm;
using SepalWRFM.Services.Interfaces;

namespace SepalWRFM.Modules.ModuleName.ViewModels
{
    public class ViewAViewModel : RegionViewModelBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ViewAViewModel(IRegionManager regionManager, IMessageService messageService) :
            base(regionManager)
        {
            Message = messageService.GetMessage();
        }

        public override void OnNavigatedTo(Prism.Regions.NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
        }
    }
}
