using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using SepalWRFM.Modules.ModuleName;
using SepalWRFM.Services;
using SepalWRFM.Services.Interfaces;
using SepalWRFM.Views;

namespace SepalWRFM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<IWindowService, Services.WindowService>();
            containerRegistry.Register<Views.SepalWRFMWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
        }
    }
}
