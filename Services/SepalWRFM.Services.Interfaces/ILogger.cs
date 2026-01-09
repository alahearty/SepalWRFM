namespace SepalWRFM.Services.Interfaces
{
    /// <summary>
    /// Interface for logging operations throughout the application.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a debug message.
        /// </summary>
        void Debug(string message);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        void Info(string message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        void Warning(string message);

        /// <summary>
        /// Logs an error message with optional exception details.
        /// </summary>
        void Error(string message, System.Exception exception = null);
    }
}