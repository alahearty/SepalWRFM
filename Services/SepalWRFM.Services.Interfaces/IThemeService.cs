namespace SepalWRFM.Services.Interfaces
{
    /// <summary>
    /// Interface for theme management operations.
    /// </summary>
    public interface IThemeService
    {
        /// <summary>
        /// Gets or sets the current theme (true for dark, false for light).
        /// </summary>
        bool IsDarkTheme { get; set; }

        /// <summary>
        /// Applies the specified theme to the application.
        /// </summary>
        /// <param name="isDarkTheme">True for dark theme, false for light theme.</param>
        void ApplyTheme(bool isDarkTheme);

        /// <summary>
        /// Toggles between dark and light themes.
        /// </summary>
        void ToggleTheme();
    }
}