using System;
using System.Globalization;
using System.Windows.Data;

namespace SepalWRFM.Modules.ModuleName.Converters
{
    public class BooleanToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFeatured && isFeatured)
            {
                return double.NaN; // Auto height for featured
            }
            return 140.0; // Fixed height for regular cards
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
