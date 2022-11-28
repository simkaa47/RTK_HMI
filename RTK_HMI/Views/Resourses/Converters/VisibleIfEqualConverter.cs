using System;
using System.Globalization;
using System.Windows;

namespace RTK_HMI.Views.Resourses.Converters
{
    internal class VisibleIfEqualConverter:Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return Visibility.Collapsed;
            if (value.ToString() != parameter.ToString()) return Visibility.Visible;
            return Visibility.Collapsed;
        }
    }
}
