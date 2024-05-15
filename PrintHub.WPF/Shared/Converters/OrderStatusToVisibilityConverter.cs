using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PrintHub.WPF.Shared.Converters;

public class OrderStatusToVisibilityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is null ? value : GetVisibility((OrderStatus)value);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is null ? value : GetOrderStatus((Visibility)value);

    private static Visibility GetVisibility(OrderStatus status)
        => status == OrderStatus.New ? Visibility.Visible : Visibility.Collapsed;

    private static OrderStatus GetOrderStatus(Visibility visibility)
        => visibility == Visibility.Visible ? OrderStatus.New : default;
}