using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SepalWRFM.Core.Converters
{
    /// <summary>
    /// Converts an icon name string to an ImageSource for displaying images instead of MaterialDesign icons.
    /// </summary>
    public class IconToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string iconName && !string.IsNullOrWhiteSpace(iconName))
            {
                try
                {
                    // Try to get the image path from resources
                    var resourceKey = $"Icon_{iconName}";
                    var imagePath = System.Windows.Application.Current?.Resources[resourceKey] as string;
                    
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        return bitmap;
                    }
                    
                    // Fallback to default icon if not found
                    var defaultKey = "Icon_Default";
                    var defaultPath = System.Windows.Application.Current?.Resources[defaultKey] as string;
                    
                    if (!string.IsNullOrEmpty(defaultPath))
                    {
                        var defaultBitmap = new BitmapImage();
                        defaultBitmap.BeginInit();
                        defaultBitmap.UriSource = new Uri(defaultPath, UriKind.Absolute);
                        defaultBitmap.CacheOption = BitmapCacheOption.OnLoad;
                        defaultBitmap.EndInit();
                        return defaultBitmap;
                    }
                }
                catch (Exception ex)
                {
                    // Log the error for debugging (optional - can be removed in production)
                    System.Diagnostics.Debug.WriteLine($"Failed to load icon '{iconName}': {ex.Message}");
                    // Return null if image loading fails - this will show only the colored background
                    return null;
                }
            }
            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}