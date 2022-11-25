using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace RTK_HMI.Views.Resourses.Converters
{
    internal class BoolToColorConverter:Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is bool condition))return parameter;
            if(!condition)return parameter;
            if (parameter == null) return parameter;
            return Brushes.LightGreen;
        }
    }
}
