using SepalWRFM.Services.Interfaces;
using SepalWRFM.Views;
using Prism.Ioc;

namespace SepalWRFM.Services
{
    public class WindowService : IWindowService
    {
        private readonly IContainerProvider _containerProvider;

        public WindowService(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
        }

        public void OpenSepalWRFMWindow()
        {
            var window = _containerProvider.Resolve<SepalWRFMWindow>();
            window.Show();
        }
    }
}
