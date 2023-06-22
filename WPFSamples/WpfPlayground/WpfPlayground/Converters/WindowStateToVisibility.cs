using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfPlayground.Converters;

/// <summary>
/// WindowStateToVisibility converter
/// </summary>
[ValueConversion(typeof(WindowState), typeof(Visibility))]
public class WindowStateToVisibility : IValueConverter
{
    /// <summary>
    /// /// This converter will take a given window state and compare it to the passed in parameter. If they equal then
    /// it will return Visibility.Visible. Otherwise it will return Visibility.Collapsed
    /// </summary>
    /// <param name="value">The current WindowState</param>
    /// <param name="targetType">Should be typeof(Visibility)</param>
    /// <param name="parameter">The state which causes the control to be Visible</param>
    /// <param name="culture">ignored</param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not WindowState || targetType != typeof(Visibility) || parameter is not WindowState)
        {
            return DependencyProperty.UnsetValue;
        }

        return (WindowState) value == (WindowState)parameter ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
