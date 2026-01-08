using System;
using System.Globalization;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace SepalWRFM.Modules.ModuleName.Converters
{
    public class IconKindConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string iconName)
            {
                if (Enum.TryParse<PackIconKind>(iconName, true, out var iconKind))
                {
                    return iconKind;
                }
            }
            return PackIconKind.Application;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
