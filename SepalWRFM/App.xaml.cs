using System;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using SepalWRFM.Modules.ModuleName;
using SepalWRFM.Modules.WRFM;
using SepalWRFM.Modules.ProductionSurveilance;
using SepalWRFM.Modules.Copilot;
using SepalWRFM.Services;
using SepalWRFM.Services.Interfaces;
using SepalWRFM.Views;
using Services = SepalWRFM.Services;
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
            // Register global exception handlers
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Register Syncfusion license key from environment variable or use default
            var licenseKey = Environment.GetEnvironmentVariable("SYNCFUSION_LICENSE_KEY") 
                ?? "Ngo9BigBOggjHTQxAR8/V1JGaF5cXGpCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWX5eeHZVQ2NcVUN0WktWYEs=";
            SyncfusionLicenseProvider.RegisterLicense(licenseKey);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Log the exception
            System.Diagnostics.Debug.WriteLine($"Unhandled exception in UI thread: {e.Exception}");
            
            // Show user-friendly error message
            try
            {
                MessageBox.Show(
                    "An unexpected error occurred. Please try again or contact support if the problem persists.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch
            {
                // If we can't show a message box, at least log it
                System.Diagnostics.Debug.WriteLine($"Exception details: {e.Exception}");
            }

            // Mark as handled to prevent application crash
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            System.Diagnostics.Debug.WriteLine($"Unhandled exception in application domain: {exception}");
            
            // Log critical exceptions
            if (e.IsTerminating)
            {
                System.Diagnostics.Debug.WriteLine("Application is terminating due to unhandled exception.");
            }
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Logging service (registered first as other services depend on it)
            containerRegistry.RegisterSingleton<Services.Interfaces.ILogger, Services.Logger>();
            
            // Core services
            containerRegistry.RegisterSingleton<IMessageService, Services.MessageService>();
            containerRegistry.RegisterSingleton<IWindowService, Services.WindowService>();
            
            // Theme service
            containerRegistry.RegisterSingleton<Services.Interfaces.IThemeService, Services.ThemeService>();
            
            // WRFM module service
            containerRegistry.RegisterSingleton<Services.Interfaces.IWRFMModuleService, Services.WRFMModuleService>();
            
            // Views
            containerRegistry.Register<SepalWRFMWindow>();
            containerRegistry.Register<ProductionAnalysisWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
            moduleCatalog.AddModule<WRFMModule>();
            moduleCatalog.AddModule<ProductionSurveilanceModule>();
            moduleCatalog.AddModule<CopilotModule>();
        }
    }
}
