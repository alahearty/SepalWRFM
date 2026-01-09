using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SepalWRFM.Modules.Copilot.Services
{
    public class McpClientService : IMcpClientService
    {
        private readonly HttpClient _httpClient;
        private string? _apiBaseUrl = "https://localhost:7001";
        private string? _authToken;

        public McpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public void SetAuthToken(string? token)
        {
            _authToken = token;
            if (_httpClient != null)
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                if (!string.IsNullOrWhiteSpace(token))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                }
            }
        }

        public string? ApiBaseUrl
        {
            get => _apiBaseUrl;
            set => _apiBaseUrl = value;
        }

        public async Task<IEnumerable<ToolInfo>> GetToolsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(_apiBaseUrl))
                {
                    throw new InvalidOperationException("API base URL is not configured");
                }

                var url = $"{_apiBaseUrl}/api/mcp/tools";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                
                if (!string.IsNullOrWhiteSpace(_authToken))
                {
                    request.Headers.Add("Authorization", $"Bearer {_authToken}");
                }
                
                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    throw new HttpRequestException($"Failed to get tools: {response.StatusCode} - {errorContent}");
                }

                var toolsJson = await response.Content.ReadAsStringAsync(cancellationToken);
                var tools = JsonSerializer.Deserialize<List<ToolDto>>(toolsJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return tools?.Select(t => new ToolInfo
                {
                    Name = t.Name ?? string.Empty,
                    Description = t.Description ?? string.Empty,
                    IsApproved = IsToolApproved(t.Name ?? string.Empty),
                    Category = GetToolCategory(t.Name ?? string.Empty)
                }) ?? Enumerable.Empty<ToolInfo>();
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("SSL") || ex.Message.Contains("certificate"))
            {
                System.Diagnostics.Debug.WriteLine($"SSL Error: {ex.Message}");
                return Enumerable.Empty<ToolInfo>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting tools: {ex.Message}");
                return Enumerable.Empty<ToolInfo>();
            }
        }

        public async Task<ToolExecutionResult> ExecuteToolAsync(string toolName, Dictionary<string, object?> arguments, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(_apiBaseUrl))
                {
                    return new ToolExecutionResult
                    {
                        Success = false,
                        Error = "API base URL is not configured",
                        ToolName = toolName
                    };
                }

                if (!IsToolApproved(toolName))
                {
                    return new ToolExecutionResult
                    {
                        Success = false,
                        Error = $"Tool '{toolName}' is not approved for execution",
                        ToolName = toolName
                    };
                }

                var url = $"{_apiBaseUrl}/api/mcp/tools/{toolName}/execute";
                var requestBody = new
                {
                    arguments = arguments
                };

                var request = new HttpRequestMessage(HttpMethod.Post, url);
                
                if (!string.IsNullOrWhiteSpace(_authToken))
                {
                    request.Headers.Add("Authorization", $"Bearer {_authToken}");
                }
                
                request.Content = JsonContent.Create(requestBody);
                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    return new ToolExecutionResult
                    {
                        Success = false,
                        Error = errorContent,
                        ToolName = toolName
                    };
                }

                var result = await response.Content.ReadAsStringAsync(cancellationToken);
                return new ToolExecutionResult
                {
                    Success = true,
                    Result = result,
                    ToolName = toolName
                };
            }
            catch (Exception ex)
            {
                return new ToolExecutionResult
                {
                    Success = false,
                    Error = ex.Message,
                    ToolName = toolName
                };
            }
        }

        public async Task<string> SendChatMessageAsync(string message, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(_apiBaseUrl))
                {
                    return "Error: API base URL is not configured";
                }

                var url = $"{_apiBaseUrl}/api/mcp/chat";
                var requestBody = new { message = message };

                var request = new HttpRequestMessage(HttpMethod.Post, url);
                
                if (!string.IsNullOrWhiteSpace(_authToken))
                {
                    request.Headers.Add("Authorization", $"Bearer {_authToken}");
                }
                
                request.Content = JsonContent.Create(requestBody);
                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    return $"Error: Failed to send message ({response.StatusCode})";
                }

                var result = await response.Content.ReadAsStringAsync(cancellationToken);
                var chatResponse = JsonSerializer.Deserialize<ChatResponseDto>(result, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return chatResponse?.Response ?? "No response received";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private bool IsToolApproved(string toolName)
        {
            if (string.IsNullOrWhiteSpace(toolName))
                return false;

            return toolName.StartsWith("eps_", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Categorizes tools for better organization in the UI
        /// </summary>
        private string GetToolCategory(string toolName)
        {
            if (toolName.StartsWith("eps_", StringComparison.OrdinalIgnoreCase))
            {
                if (toolName.Contains("business_plan", StringComparison.OrdinalIgnoreCase))
                    return "Business Plans";
                if (toolName.Contains("asset", StringComparison.OrdinalIgnoreCase))
                    return "Assets & Projects";
                if (toolName.Contains("production", StringComparison.OrdinalIgnoreCase))
                    return "Production Forecast";
                if (toolName.Contains("dashboard", StringComparison.OrdinalIgnoreCase))
                    return "Dashboard";
                if (toolName.Contains("budget", StringComparison.OrdinalIgnoreCase))
                    return "Budget Analysis";
                if (toolName.Contains("variance", StringComparison.OrdinalIgnoreCase))
                    return "Variance Analysis";
                if (toolName.Contains("sensitivity", StringComparison.OrdinalIgnoreCase))
                    return "Sensitivity Analysis";
            }
            return "General";
        }

        private class ToolDto
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
        }

        private class ChatResponseDto
        {
            public string? Response { get; set; }
        }
    }
}
