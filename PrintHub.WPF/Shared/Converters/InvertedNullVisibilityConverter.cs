﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PrintHub.WPF.Shared.Converters;

public class InvertedNullVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value != null ? Visibility.Hidden : Visibility.Visible;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}