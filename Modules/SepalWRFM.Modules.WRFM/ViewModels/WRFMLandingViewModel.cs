using Prism.Mvvm;

namespace SepalWRFM.Modules.WRFM.ViewModels
{
    public class WRFMLandingViewModel : BindableBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public WRFMLandingViewModel()
        {
            Message = "View A from your Prism Module";
        }
    }
}
