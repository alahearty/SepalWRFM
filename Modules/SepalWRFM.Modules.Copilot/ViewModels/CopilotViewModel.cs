using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using SepalWRFM.Core.Mvvm;
using SepalWRFM.Modules.Copilot.Services;
using Prism.Commands;
using Prism.Regions;

namespace SepalWRFM.Modules.Copilot.ViewModels
{
    public class CopilotViewModel : RegionViewModelBase
    {
        private readonly IMcpClientService _mcpClientService;
        private string _userMessage = string.Empty;
        private string _assistantResponse = string.Empty;
        private bool _isLoading;
        private bool _isVisible = true;
        private bool _showWelcomeScreen = true;
        private bool _showMoreMenu = false;
        private string _selectedModel = "Auto";
        private ObservableCollection<ChatMessage> _messages = new();
        private ObservableCollection<ToolInfo> _availableTools = new();
        private ObservableCollection<SuggestedPrompt> _suggestedPrompts = new();

        public CopilotViewModel(IRegionManager regionManager, IMcpClientService mcpClientService)
            : base(regionManager)
        {
            _mcpClientService = mcpClientService ?? throw new ArgumentNullException(nameof(mcpClientService));
            SendMessageCommand = new DelegateCommand(async () => await SendMessageAsync(), () => !IsLoading && !string.IsNullOrWhiteSpace(UserMessage))
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => UserMessage);
            
            LoadToolsCommand = new DelegateCommand(async () => await LoadToolsAsync());
            ToggleVisibilityCommand = new DelegateCommand(() => IsVisible = !IsVisible);
            SelectSuggestedPromptCommand = new DelegateCommand<string>(async (prompt) => await SelectSuggestedPromptAsync(prompt));
            CloseCopilotCommand = new DelegateCommand(() => IsVisible = false);
            ShowMoreMenuCommand = new DelegateCommand(() => ShowMoreMenu = !ShowMoreMenu);
            NewTemporaryChatCommand = new DelegateCommand(() => NewTemporaryChat());
            OpenInAppCommand = new DelegateCommand(() => OpenInApp());
            SendFeedbackCommand = new DelegateCommand(() => SendFeedback());
            OpenSettingsCommand = new DelegateCommand(() => OpenSettings());
            SelectModelCommand = new DelegateCommand<string>((model) => SelectedModel = model);

            // Initialize suggested prompts
            InitializeSuggestedPrompts();

            // Load available tools on initialization
            _ = LoadToolsAsync();
        }

        public string UserMessage
        {
            get => _userMessage;
            set => SetProperty(ref _userMessage, value);
        }

        public string AssistantResponse
        {
            get => _assistantResponse;
            set => SetProperty(ref _assistantResponse, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public ObservableCollection<ChatMessage> Messages
        {
            get => _messages;
            set => SetProperty(ref _messages, value);
        }

        public ObservableCollection<ToolInfo> AvailableTools
        {
            get => _availableTools;
            set => SetProperty(ref _availableTools, value);
        }

        public bool ShowWelcomeScreen
        {
            get => _showWelcomeScreen;
            set => SetProperty(ref _showWelcomeScreen, value);
        }

        public ObservableCollection<SuggestedPrompt> SuggestedPrompts
        {
            get => _suggestedPrompts;
            set => SetProperty(ref _suggestedPrompts, value);
        }

        public bool ShowMoreMenu
        {
            get => _showMoreMenu;
            set => SetProperty(ref _showMoreMenu, value);
        }

        public string SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        public ICommand SendMessageCommand { get; }
        public ICommand LoadToolsCommand { get; }
        public ICommand ToggleVisibilityCommand { get; }
        public ICommand SelectSuggestedPromptCommand { get; }
        public ICommand CloseCopilotCommand { get; }
        public ICommand ShowMoreMenuCommand { get; }
        public ICommand NewTemporaryChatCommand { get; }
        public ICommand OpenInAppCommand { get; }
        public ICommand SendFeedbackCommand { get; }
        public ICommand OpenSettingsCommand { get; }
        public ICommand SelectModelCommand { get; }

        private void InitializeSuggestedPrompts()
        {
            SuggestedPrompts.Add(new SuggestedPrompt
            {
                Text = "Show me all **business plans**",
                Placeholder = "business plans"
            });
            SuggestedPrompts.Add(new SuggestedPrompt
            {
                Text = "Analyze **production forecast** for this project",
                Placeholder = "production forecast"
            });
            SuggestedPrompts.Add(new SuggestedPrompt
            {
                Text = "What are the **budget variances** this quarter?",
                Placeholder = "budget variances"
            });
        }

        private async Task SelectSuggestedPromptAsync(string prompt)
        {
            UserMessage = prompt;
            ShowWelcomeScreen = false;
            await SendMessageAsync();
        }

        private async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(UserMessage) || IsLoading)
                return;

            var userMsg = UserMessage.Trim();
            UserMessage = string.Empty;

            ShowWelcomeScreen = false;
            Messages.Add(new ChatMessage
            {
                Content = userMsg,
                Sender = MessageSender.User,
                Timestamp = DateTime.Now
            });

            IsLoading = true;

            try
            {
                var toolResult = await TryExecuteToolFromPromptAsync(userMsg);
                
                if (toolResult != null && toolResult.Success)
                {
                    var response = FormatToolResponse(toolResult);
                    Messages.Add(new ChatMessage
                    {
                        Content = response,
                        Sender = MessageSender.Assistant,
                        Timestamp = DateTime.Now
                    });
                    AssistantResponse = response;
                }
                else
                {
                    var response = await _mcpClientService.SendChatMessageAsync(userMsg);
                    Messages.Add(new ChatMessage
                    {
                        Content = response,
                        Sender = MessageSender.Assistant,
                        Timestamp = DateTime.Now
                    });
                    AssistantResponse = response;
                }
            }
            catch (System.Net.Http.HttpRequestException ex) when (ex.Message.Contains("SSL") || ex.Message.Contains("certificate"))
            {
                Messages.Add(new ChatMessage
                {
                    Content = "⚠️ SSL Certificate Error: Unable to connect to the API. Please ensure:\n" +
                             "1. The API is running\n" +
                             "2. The API URL is correct (https://localhost:7001)\n" +
                             "3. Restart the WPF application to apply SSL certificate fixes\n\n" +
                             $"Technical details: {ex.Message}",
                    Sender = MessageSender.System,
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                Messages.Add(new ChatMessage
                {
                    Content = $"⚠️ Error: {ex.Message}\n\nPlease check:\n" +
                             "1. API is running and accessible\n" +
                             "2. Network connection is working\n" +
                             "3. API base URL is configured correctly",
                    Sender = MessageSender.System,
                    Timestamp = DateTime.Now
                });
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task<ToolExecutionResult?> TryExecuteToolFromPromptAsync(string prompt)
        {
            var lowerPrompt = prompt.ToLowerInvariant();

            try
            {
                if (lowerPrompt.Contains("business plan") && 
                    (lowerPrompt.Contains("list") || lowerPrompt.Contains("show") || lowerPrompt.Contains("all") || lowerPrompt.Contains("get all")))
                {
                    return await _mcpClientService.ExecuteToolAsync("eps_list_business_plans", new Dictionary<string, object?>());
                }

                if (lowerPrompt.Contains("asset") && 
                    (lowerPrompt.Contains("list") || lowerPrompt.Contains("show") || lowerPrompt.Contains("all")) &&
                    !lowerPrompt.Contains("business plan"))
                {
                    return await _mcpClientService.ExecuteToolAsync("eps_list_assets", new Dictionary<string, object?>());
                }

                if (lowerPrompt.Contains("project") && 
                    (lowerPrompt.Contains("list") || lowerPrompt.Contains("show") || lowerPrompt.Contains("all")))
                {
                    return await _mcpClientService.ExecuteToolAsync("eps_list_projects", new Dictionary<string, object?>());
                }

                if (lowerPrompt.Contains("field") && 
                    (lowerPrompt.Contains("list") || lowerPrompt.Contains("show") || lowerPrompt.Contains("all")))
                {
                    return await _mcpClientService.ExecuteToolAsync("eps_list_fields", new Dictionary<string, object?>());
                }

                if (lowerPrompt.Contains("admin dashboard") || lowerPrompt.Contains("system dashboard") || lowerPrompt.Contains("system-wide"))
                {
                    return await _mcpClientService.ExecuteToolAsync("eps_get_admin_dashboard", new Dictionary<string, object?>());
                }

                if (lowerPrompt.Contains("dashboard") && !lowerPrompt.Contains("admin"))
                {
                    var businessPlanId = ExtractGuidFromPrompt(prompt);
                    if (businessPlanId.HasValue)
                    {
                        return await _mcpClientService.ExecuteToolAsync("eps_get_dashboard", new Dictionary<string, object?>
                        {
                            { "businessPlanId", businessPlanId.Value.ToString() }
                        });
                    }
                    
                    return new ToolExecutionResult
                    {
                        Success = false,
                        ToolName = "eps_get_dashboard",
                        Error = "I need a business plan ID to get the dashboard. Please provide it in the format: 'Show dashboard for business plan {GUID}' or specify the business plan name."
                    };
                }

                if (lowerPrompt.Contains("production forecast") || lowerPrompt.Contains("production data"))
                {
                    var businessPlanId = ExtractGuidFromPrompt(prompt);
                    if (businessPlanId.HasValue)
                    {
                        var year = ExtractYearFromPrompt(prompt);
                        if (year.HasValue)
                        {
                            return await _mcpClientService.ExecuteToolAsync("eps_get_production_data_by_month", new Dictionary<string, object?>
                            {
                                { "businessPlanId", businessPlanId.Value.ToString() },
                                { "year", year.Value }
                            });
                        }
                        return await _mcpClientService.ExecuteToolAsync("eps_get_production_data_by_year", new Dictionary<string, object?>
                        {
                            { "businessPlanId", businessPlanId.Value.ToString() }
                        });
                    }
                    return new ToolExecutionResult
                    {
                        Success = false,
                        ToolName = "eps_get_production_forecast",
                        Error = "I need a business plan ID to get production forecast. Please provide it."
                    };
                }

                if (lowerPrompt.Contains("variance"))
                {
                    var businessPlanId = ExtractGuidFromPrompt(prompt);
                    if (businessPlanId.HasValue)
                    {
                        if (lowerPrompt.Contains("budget"))
                            return await _mcpClientService.ExecuteToolAsync("eps_get_budget_variance", new Dictionary<string, object?>
                            {
                                { "businessPlanId", businessPlanId.Value.ToString() }
                            });
                        else if (lowerPrompt.Contains("production"))
                            return await _mcpClientService.ExecuteToolAsync("eps_get_production_variance", new Dictionary<string, object?>
                            {
                                { "businessPlanId", businessPlanId.Value.ToString() }
                            });
                        else if (lowerPrompt.Contains("cashflow") || lowerPrompt.Contains("cash flow"))
                            return await _mcpClientService.ExecuteToolAsync("eps_get_cashflow_variance", new Dictionary<string, object?>
                            {
                                { "businessPlanId", businessPlanId.Value.ToString() }
                            });
                    }
                    return new ToolExecutionResult
                    {
                        Success = false,
                        ToolName = "eps_get_variance",
                        Error = "I need a business plan ID to get variance analysis. Please specify: budget variance, production variance, or cashflow variance."
                    };
                }

                if (lowerPrompt.Contains("sensitivity"))
                {
                    var businessPlanId = ExtractGuidFromPrompt(prompt);
                    if (businessPlanId.HasValue)
                    {
                        return await _mcpClientService.ExecuteToolAsync("eps_get_sensitivity_iterations", new Dictionary<string, object?>
                        {
                            { "businessPlanId", businessPlanId.Value.ToString() }
                        });
                    }
                    return new ToolExecutionResult
                    {
                        Success = false,
                        ToolName = "eps_get_sensitivity",
                        Error = "I need a business plan ID to get sensitivity analysis. Please provide it."
                    };
                }

                if (lowerPrompt.Contains("report"))
                {
                    if (lowerPrompt.Contains("work program"))
                        return await _mcpClientService.ExecuteToolAsync("eps_get_work_program", new Dictionary<string, object?>());
                    else if (lowerPrompt.Contains("funding summary"))
                        return await _mcpClientService.ExecuteToolAsync("eps_get_detailed_funding_summary", new Dictionary<string, object?>());
                    else if (lowerPrompt.Contains("asset summary"))
                        return await _mcpClientService.ExecuteToolAsync("eps_get_asset_summary", new Dictionary<string, object?>());
                    else if (lowerPrompt.Contains("cost category"))
                        return await _mcpClientService.ExecuteToolAsync("eps_get_cost_category_summary", new Dictionary<string, object?>());
                    else if (lowerPrompt.Contains("financial variance"))
                        return await _mcpClientService.ExecuteToolAsync("eps_get_financial_variance_report", new Dictionary<string, object?>());
                    
                    return new ToolExecutionResult
                    {
                        Success = false,
                        ToolName = "eps_report",
                        Error = "Available reports: Work Program, Funding Summary, Asset Summary, Cost Category Summary, Financial Variance. Please specify which one you need."
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error executing tool from prompt: {ex.Message}");
                return null;
            }
        }

        private Guid? ExtractGuidFromPrompt(string prompt)
        {
            // Try to find GUID pattern in the prompt
            var guidPattern = @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
            var match = System.Text.RegularExpressions.Regex.Match(prompt, guidPattern);
            if (match.Success && Guid.TryParse(match.Value, out var guid))
            {
                return guid;
            }
            return null;
        }

        private int? ExtractYearFromPrompt(string prompt)
        {
            var yearPattern = @"\b(19|20)\d{2}\b";
            var match = System.Text.RegularExpressions.Regex.Match(prompt, yearPattern);
            if (match.Success && int.TryParse(match.Value, out var year) && year >= 1900 && year <= 2100)
            {
                return year;
            }
            return null;
        }

        private string FormatToolResponse(ToolExecutionResult result)
        {
            if (!result.Success)
            {
                return $"⚠️ Tool execution failed: {result.Error ?? "Unknown error"}";
            }

            try
            {
                // Try to parse and format JSON result
                if (!string.IsNullOrWhiteSpace(result.Result))
                {
                    var json = System.Text.Json.JsonDocument.Parse(result.Result);
                    if (json.RootElement.TryGetProperty("result", out var resultElement))
                    {
                        // Try to parse nested JSON
                        var resultStr = resultElement.GetString();
                        if (!string.IsNullOrWhiteSpace(resultStr))
                        {
                            try
                            {
                                var nestedJson = System.Text.Json.JsonDocument.Parse(resultStr);
                                return $"✅ **{result.ToolName}** executed successfully:\n\n```json\n{nestedJson.RootElement.GetRawText()}\n```";
                            }
                            catch
                            {
                                return $"✅ **{result.ToolName}** executed successfully:\n\n{resultStr}";
                            }
                        }
                    }
                    return $"✅ **{result.ToolName}** executed successfully:\n\n```json\n{result.Result}\n```";
                }
                
                return $"✅ **{result.ToolName}** executed successfully.";
            }
            catch
            {
                // If JSON parsing fails, return raw result
                return $"✅ **{result.ToolName}** executed successfully:\n\n{result.Result}";
            }
        }

        private async Task LoadToolsAsync()
        {
            try
            {
                var tools = await _mcpClientService.GetToolsAsync();
                AvailableTools.Clear();
                foreach (var tool in tools.OrderBy(t => t.Category).ThenBy(t => t.Name))
                {
                    AvailableTools.Add(tool);
                }
            }
            catch (Exception ex)
            {
                // In a real implementation, log this exception
                System.Diagnostics.Debug.WriteLine($"Failed to load tools: {ex.Message}");
            }
        }

        private void NewTemporaryChat()
        {
            ShowMoreMenu = false;
            Messages.Clear();
            ShowWelcomeScreen = true;
            UserMessage = string.Empty;
        }

        private void OpenInApp()
        {
            ShowMoreMenu = false;
            // In a real implementation, this would open Copilot in a separate window/app
            System.Diagnostics.Debug.WriteLine("Open in SEPAL WRFM Copilot app");
        }

        private void SendFeedback()
        {
            ShowMoreMenu = false;
            // In a real implementation, this would open a feedback dialog
            System.Diagnostics.Debug.WriteLine("Send feedback");
        }

        private void OpenSettings()
        {
            ShowMoreMenu = false;
            // In a real implementation, this would open settings dialog
            System.Diagnostics.Debug.WriteLine("Open settings");
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            // Reload tools when navigating to Copilot
            _ = LoadToolsAsync();
        }
    }

    public class ChatMessage
    {
        public string Content { get; set; } = string.Empty;
        public MessageSender Sender { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public enum MessageSender
    {
        User,
        Assistant,
        System
    }

    public class SuggestedPrompt
    {
        public string Text { get; set; } = string.Empty;
        public string Placeholder { get; set; } = string.Empty;
    }
}
