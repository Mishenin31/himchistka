using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Himchistka.Core.Enums;

namespace Himchistka.UI.Converters;

public class StatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not OrderStatus status)
            return Brushes.LightGray;

        return status switch
        {
            OrderStatus.Accepted => Brushes.DodgerBlue,
            OrderStatus.InProgress => Brushes.Gold,
            OrderStatus.Ready => Brushes.LimeGreen,
            OrderStatus.Issued => Brushes.Gray,
            OrderStatus.Cancelled => Brushes.IndianRed,
            _ => Brushes.LightGray
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}
