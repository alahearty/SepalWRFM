using System;
using System.Globalization;
using System.Windows.Data;

namespace SepalWRFM.Modules.ModuleName.Converters
{
    public class BooleanToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFeatured && isFeatured)
            {
                return 400.0; // Featured cards are wider
            }
            return 140.0; // Regular cards
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
