using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace WpfPlayground.Converters;

/// <summary>
/// Converts a Boolean into a Visibility type. The System.Windows.Controls.BooleanToVisibility converter uses
/// collapsed for false and I want to use Hidden so the space for the controls is maintained when not visible.
/// </summary>
public class MyBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool || targetType != typeof(Visibility))
        {
            return DependencyProperty.UnsetValue;
        }

        return (bool)value ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
