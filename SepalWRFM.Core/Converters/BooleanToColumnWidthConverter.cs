using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SepalWRFM.Core.Converters
{
    public class BooleanToColumnWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isVisible && isVisible)
            {
                return new GridLength(240); // Sidebar width when visible
            }
            return new GridLength(0); // Collapsed when hidden
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
