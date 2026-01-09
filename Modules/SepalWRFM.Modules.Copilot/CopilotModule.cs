using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Security;
using SepalWRFM.Core;
using SepalWRFM.Modules.Copilot.Services;
using SepalWRFM.Modules.Copilot.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace SepalWRFM.Modules.Copilot
{
    public class CopilotModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public CopilotModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var mcpService = containerProvider.Resolve<IMcpClientService>();
            
            var apiUrl = Environment.GetEnvironmentVariable("EPS_API_URL") 
                        ?? Environment.GetEnvironmentVariable("EPS_API_BASE_URL")
                        ?? "https://localhost:7001";
            mcpService.ApiBaseUrl = apiUrl;
            
            var authToken = Environment.GetEnvironmentVariable("EPS_AUTH_TOKEN");
            if (!string.IsNullOrWhiteSpace(authToken))
            {
                mcpService.SetAuthToken(authToken);
            }

            // Navigate to CopilotView when region is available (lazy initialization)
            // The navigation will happen when the Copilot panel is first shown
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Loaded,
                new Action(() =>
                {
                    try
                    {
                        if (_regionManager.Regions.ContainsRegionWithName(RegionNames.CopilotRegion))
                        {
                            _regionManager.RequestNavigate(RegionNames.CopilotRegion, "CopilotView");
                        }
                    }
                    catch
                    {
                        // Region not available yet, will be navigated when panel is shown
                    }
                }));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<HttpClient>(() =>
            {
                var handler = new HttpClientHandler();
                var isDevelopment = Debugger.IsAttached || 
                                   Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
                
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    var host = message.RequestUri?.Host?.ToLowerInvariant();
                    if (host == "localhost" || host == "127.0.0.1")
                    {
                        return true;
                    }
                    
                    if (isDevelopment)
                    {
                        return true;
                    }
                    
                    return errors == SslPolicyErrors.None;
                };
                
                var client = new HttpClient(handler);
                client.Timeout = TimeSpan.FromMinutes(5);
                return client;
            });

            containerRegistry.RegisterSingleton<IMcpClientService, McpClientService>();
            containerRegistry.RegisterForNavigation<CopilotView>("CopilotView");
        }
    }
}
