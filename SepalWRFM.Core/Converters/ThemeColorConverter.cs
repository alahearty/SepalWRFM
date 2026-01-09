using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SepalWRFM.Core.Converters
{
    public class ThemeColorConverter : IValueConverter
    {
        public Color DarkColor { get; set; }
        public Color LightColor { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isDarkTheme)
            {
                return isDarkTheme ? DarkColor : LightColor;
            }
            return DarkColor; // Default to dark
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
