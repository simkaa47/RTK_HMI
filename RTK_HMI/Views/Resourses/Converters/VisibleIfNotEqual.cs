using System.Globalization;
using System.Windows;
using System;

namespace RTK_HMI.Views.Resourses.Converters
{
    class VisibleIfNotEqual : Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return Visibility.Collapsed;
            if (value.ToString() != parameter.ToString()) return Visibility.Visible;
            return Visibility.Collapsed;
        }
    }
}
