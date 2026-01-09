using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SepalWRFM.Modules.Copilot.Services
{
    /// <summary>
    /// Service for communicating with the SEPAL WRFM MCP server
    /// </summary>
    public interface IMcpClientService
    {
        /// <summary>
        /// Gets all available tools from the MCP server
        /// </summary>
        Task<IEnumerable<ToolInfo>> GetToolsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a tool with the given arguments
        /// </summary>
        Task<ToolExecutionResult> ExecuteToolAsync(string toolName, Dictionary<string, object?> arguments, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a chat message to the AI assistant via MCP
        /// </summary>
        Task<string> SendChatMessageAsync(string message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets or sets the SEPAL WRFM API base URL
        /// </summary>
        string? ApiBaseUrl { get; set; }

        /// <summary>
        /// Sets the authentication token for API requests
        /// </summary>
        void SetAuthToken(string? token);
    }

    /// <summary>
    /// Information about an available MCP tool
    /// </summary>
    public class ToolInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    /// <summary>
    /// Result of tool execution
    /// </summary>
    public class ToolExecutionResult
    {
        public bool Success { get; set; }
        public string? Result { get; set; }
        public string? Error { get; set; }
        public string ToolName { get; set; } = string.Empty;
    }
}
