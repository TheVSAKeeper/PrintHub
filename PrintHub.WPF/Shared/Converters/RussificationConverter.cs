using System.Globalization;
using System.Windows.Data;

namespace PrintHub.WPF.Shared.Converters;

public class RussificationConverter : IValueConverter
{
    private readonly Dictionary<string, string> _matching = new()
    {
        { "Administrator", "Администратор" },
        { "Doctor", "Врач" },
        { "Nurse", "Медсестра" },
        { "Male", "Мужской" },
        { "Female", "Женский" }
    };

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
            throw new NotSupportedException();

        KeyValuePair<string, string> valuePair = _matching.FirstOrDefault(pair => pair.Key == value?.ToString());
        return valuePair.Value ?? value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType != typeof(string))
            throw new NotSupportedException();

        KeyValuePair<string, string> valuePair = _matching.FirstOrDefault(pair => pair.Value == value?.ToString());
        return valuePair.Key ?? value;
    }
}