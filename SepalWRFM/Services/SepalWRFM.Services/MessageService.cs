using System;
using System.Windows;
using SepalWRFM.Services.Interfaces;

namespace SepalWRFM.Services
{
    /// <summary>
    /// Service for displaying messages to users.
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly ILogger _logger;

        public MessageService(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GetMessage()
        {
            return "Hello from the Message Service";
        }

        public void ShowInfo(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                _logger.Warning("ShowInfo called with null or empty message");
                return;
            }

            try
            {
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                });
                _logger.Debug($"Info message shown: {message}");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to show info message", ex);
            }
        }

        public void ShowError(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                _logger.Warning("ShowError called with null or empty message");
                return;
            }

            try
            {
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
                _logger.Debug($"Error message shown: {message}");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to show error message", ex);
            }
        }

        public void ShowWarning(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                _logger.Warning("ShowWarning called with null or empty message");
                return;
            }

            try
            {
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                });
                _logger.Debug($"Warning message shown: {message}");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to show warning message", ex);
            }
        }

        public bool ShowConfirmation(string message, string title = "Confirm")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                _logger.Warning("ShowConfirmation called with null or empty message");
                return false;
            }

            try
            {
                bool? result = null;
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    var dialogResult = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    result = dialogResult == MessageBoxResult.Yes;
                });
                _logger.Debug($"Confirmation dialog shown: {message}, result: {result}");
                return result ?? false;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to show confirmation dialog", ex);
                return false;
            }
        }
    }
}
