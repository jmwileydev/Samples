using JMWToolkit.MVVM.Helpers;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;

namespace WpfPlayground.Converters;

/// <summary>
/// Converter for going from an EasingMode into a string
/// </summary>
[ValueConversion(typeof(EasingMode), typeof(string))]
public class EasingModeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not EasingMode || targetType == typeof(string))
        {
            return DependencyProperty.UnsetValue;
        }

        switch ((EasingMode)value)
        {
            case EasingMode.EaseIn:
                return StringResourceHelpers.LoadStringResource("EaseInDisplayName");

            case EasingMode.EaseOut:
                return StringResourceHelpers.LoadStringResource("EaseOutDisplayName");

            case EasingMode.EaseInOut:
                return StringResourceHelpers.LoadStringResource("EaseInOutDisplayName");
        }

        return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
