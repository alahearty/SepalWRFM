using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SepalWRFM.Modules.Copilot.ViewModels;

namespace SepalWRFM.Modules.Copilot.Converters
{
    public class MessageStyleConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is MessageSender sender)
            {
                return sender switch
                {
                    MessageSender.User => Application.Current.FindResource("UserMessageStyle"),
                    MessageSender.Assistant => Application.Current.FindResource("AssistantMessageStyle"),
                    MessageSender.System => Application.Current.FindResource("SystemMessageStyle"),
                    _ => Application.Current.FindResource("AssistantMessageStyle")
                };
            }
            return Application.Current.FindResource("AssistantMessageStyle");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
