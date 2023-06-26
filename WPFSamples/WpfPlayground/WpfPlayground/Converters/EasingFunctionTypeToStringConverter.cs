using System.Globalization;
using System.Windows;
using System;
using System.Windows.Data;
using System.Windows.Media.Animation;
using JMWToolkit.MVVM.Helpers;

namespace WpfPlayground.Converters;

/// <summary>
/// Converts an EasingFunction type into a String
/// </summary>
[ValueConversion(typeof(Type), typeof(string))]
public class EasingFunctionTypeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Type || targetType != typeof(string))
        {
            return DependencyProperty.UnsetValue;
        }

        var valType = (Type)value;

        if (valType == typeof(BounceEase))
        {
            return StringResourceHelpers.LoadStringResource("BounceEaseDisplayName");
        }
        else if (valType == typeof(CubicEase))
        {
            return StringResourceHelpers.LoadStringResource("CubicEaseDisplayName");
        }
        else if (valType == typeof(QuarticEase))
        {
            return StringResourceHelpers.LoadStringResource("QuarticEaseDisplayName");
        }

        return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
