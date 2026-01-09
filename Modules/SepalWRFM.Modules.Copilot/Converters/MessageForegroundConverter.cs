using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SepalWRFM.Modules.Copilot.ViewModels;

namespace SepalWRFM.Modules.Copilot.Converters
{
    public class MessageForegroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is MessageSender sender)
            {
                return sender switch
                {
                    MessageSender.User => new SolidColorBrush(Colors.White),
                    MessageSender.Assistant => new SolidColorBrush(Color.FromRgb(51, 51, 51)),
                    MessageSender.System => new SolidColorBrush(Color.FromRgb(102, 51, 0)),
                    _ => new SolidColorBrush(Color.FromRgb(51, 51, 51))
                };
            }
            return new SolidColorBrush(Color.FromRgb(51, 51, 51));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
