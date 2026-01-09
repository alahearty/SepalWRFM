using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SepalWRFM.Modules.Copilot.Converters
{
    public class ModelSelectionConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string selectedModel && parameter is string modelToCheck)
            {
                return selectedModel == modelToCheck ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
