using System;
using System.Globalization;
using System.Windows.Data;

namespace SepalWRFM.Modules.Copilot.Converters
{
    public class ApprovedToTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isApproved)
            {
                return isApproved ? "Approved" : "Pending Approval";
            }
            return "Unknown";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
