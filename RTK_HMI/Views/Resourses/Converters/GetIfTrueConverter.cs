using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RTK_HMI.Views.Resourses.Converters
{
    class GetIfTrueConverter:Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is IEnumerable<object> list)) return null;
            if (list.Count() < 2) return null;
            if (!(value is bool condition)) return null;
            if (condition)
            {
                return list.ElementAt(1);
            }
            else return list.ElementAt(0);
        }
    }
}
