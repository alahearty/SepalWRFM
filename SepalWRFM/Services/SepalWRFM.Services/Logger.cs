using System;
using SepalWRFM.Services.Interfaces;

namespace SepalWRFM.Services
{
    /// <summary>
    /// Implementation of ILogger using Debug output.
    /// Can be extended to use file logging, database logging, etc.
    /// </summary>
    public class Logger : ILogger
    {
        public void Debug(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[DEBUG] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }

        public void Info(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }

        public void Warning(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[WARNING] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }

        public void Error(string message, Exception exception = null)
        {
            var errorMessage = $"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            if (exception != null)
            {
                errorMessage += $"{Environment.NewLine}Exception: {exception.Message}";
                errorMessage += $"{Environment.NewLine}Stack Trace: {exception.StackTrace}";
            }
            System.Diagnostics.Debug.WriteLine(errorMessage);
            
            // In production, you might want to:
            // - Write to a log file
            // - Send to a logging service
            // - Show user-friendly error messages
        }
    }
}