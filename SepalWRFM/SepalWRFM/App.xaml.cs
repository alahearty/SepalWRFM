using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using SepalWRFM.Modules.ModuleName;
using SepalWRFM.Modules.WRFM;
using SepalWRFM.Services;
using SepalWRFM.Services.Interfaces;
using SepalWRFM.Views;
using Syncfusion.Licensing;

namespace SepalWRFM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            // Register Syncfusion license key
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JGaF5cXGpCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWX5eeHZVQ2NcVUN0WktWYEs=");
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Core services
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<IWindowService, WindowService>();
            
            // Logging service
            containerRegistry.RegisterSingleton<Services.Interfaces.ILogger, Services.Logger>();
            
            // Theme service
            containerRegistry.RegisterSingleton<Services.Interfaces.IThemeService, Services.ThemeService>();
            
            // WRFM module service
            containerRegistry.RegisterSingleton<Services.Interfaces.IWRFMModuleService, Services.WRFMModuleService>();
            
            // Views
            containerRegistry.Register<SepalWRFMWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
            moduleCatalog.AddModule<WRFMModule>();
        }
    }
}
