namespace SepalWRFM.Services.Interfaces
{
    /// <summary>
    /// Service for displaying messages to users.
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Gets a message string.
        /// </summary>
        string GetMessage();

        /// <summary>
        /// Shows an informational message to the user.
        /// </summary>
        void ShowInfo(string message);

        /// <summary>
        /// Shows an error message to the user.
        /// </summary>
        void ShowError(string message);

        /// <summary>
        /// Shows a warning message to the user.
        /// </summary>
        void ShowWarning(string message);

        /// <summary>
        /// Shows a confirmation dialog and returns the result.
        /// </summary>
        bool ShowConfirmation(string message, string title = "Confirm");
    }
}
