using System.Globalization;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using PrintHub.Domain;

namespace PrintHub.WPF.Shared.Converters;

public class OrderStatusToIconConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not OrderStatus orderStatus)
            return PackIconKind.QuestionMark;

        return orderStatus switch
        {
            OrderStatus.None => PackIconKind.QuestionMark,
            OrderStatus.New => PackIconKind.FlagVariant,
            OrderStatus.InProgress => PackIconKind.ClockOutline,
            OrderStatus.Ready => PackIconKind.CheckBold,
            OrderStatus.Delivered => PackIconKind.TruckDelivery,
            OrderStatus.Completed => PackIconKind.CheckCircle,
            var _ => PackIconKind.QuestionMark
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not PackIconKind iconKind)
            return OrderStatus.None;

        return iconKind switch
        {
            PackIconKind.QuestionMark => OrderStatus.None,
            PackIconKind.FlagVariant => OrderStatus.New,
            PackIconKind.ClockOutline => OrderStatus.InProgress,
            PackIconKind.CheckBold => OrderStatus.Ready,
            PackIconKind.TruckDelivery => OrderStatus.Delivered,
            PackIconKind.CheckCircle => OrderStatus.Completed,
            var _ => OrderStatus.None
        };
    }
}