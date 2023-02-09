using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.ComponentModel;
using DataAccess.Models;

namespace RTK_HMI.Views.Resourses.Converters
{
    class EnumToIntConverter: Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hh = (ConnectWays)value;
            return hh;
            
        }
    }
}
