using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RTK_HMI.Views.Resourses.Converters
{
    class NumericConverter : Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            float temp = 0;
            //return !float.TryParse(value.ToString().Replace(".",","), out temp) ? "." : value;
            return IsValid(value) ? "," : value;
        }

        private bool IsValid(object value)
        {
            return value.ToString().EndsWith(",") ||
               (value.ToString().EndsWith("0") && value.ToString().Contains(","));
        }


    }
}
