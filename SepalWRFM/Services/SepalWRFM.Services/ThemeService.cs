using System;
using System.Linq;
using System.Windows;
using SepalWRFM.Services.Interfaces;

namespace SepalWRFM.Services
{
    /// <summary>
    /// Service for managing application themes.
    /// </summary>
    public class ThemeService : IThemeService
    {
        private readonly ILogger _logger;
        private bool _isDarkTheme;

        public ThemeService(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _isDarkTheme = true; // Default to dark theme
        }

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (_isDarkTheme != value)
                {
                    ApplyTheme(value);
                }
            }
        }

        public void ApplyTheme(bool isDarkTheme)
        {
            if (Application.Current == null)
            {
                _logger.Warning("Cannot apply theme: Application.Current is null");
                _isDarkTheme = isDarkTheme; // Still update the internal state
                return;
            }

            if (Application.Current.Resources == null)
            {
                _logger.Warning("Cannot apply theme: Application.Current.Resources is null");
                _isDarkTheme = isDarkTheme; // Still update the internal state
                return;
            }

            try
            {
                _isDarkTheme = isDarkTheme;
                var mergedDictionaries = Application.Current.Resources.MergedDictionaries;

                // Remove existing theme dictionaries
                var themeDictToRemove = mergedDictionaries
                    .FirstOrDefault(dict => dict.Source != null &&
                        (dict.Source.ToString().Contains("DarkTheme.xaml") ||
                         dict.Source.ToString().Contains("LightTheme.xaml")));

                if (themeDictToRemove != null)
                {
                    mergedDictionaries.Remove(themeDictToRemove);
                }

                // Add new theme dictionary
                var themeUri = isDarkTheme ? new Uri("pack://application:,,,/SepalWRFM.Modules.Landing;component/Themes/DarkTheme.xaml")
                    : new Uri("pack://application:,,,/SepalWRFM.Modules.Landing;component/Themes/LightTheme.xaml");

                mergedDictionaries.Add(new ResourceDictionary { Source = themeUri });
                Application.Current.Resources["IsDarkTheme"] = isDarkTheme;

                // Invalidate all windows to refresh theme
                if (Application.Current.Windows != null)
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window != null && window.IsLoaded)
                        {
                            window.InvalidateVisual();
                        }
                    }
                }

                _logger.Info($"Theme changed to {(isDarkTheme ? "Dark" : "Light")}");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to apply theme", ex);
                throw;
            }
        }

        public void ToggleTheme()
        {
            ApplyTheme(!_isDarkTheme);
        }
    }
}