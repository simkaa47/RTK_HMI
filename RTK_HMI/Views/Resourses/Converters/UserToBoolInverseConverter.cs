using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RTK_HMI.Views.Resourses.Converters
{
    internal class UserToBoolInverseConverter:Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || parameter is null) return true;
            if (!(value is User user)) return true;
            if (!(parameter is UserLevel level)) return true;
            if (user.Level >= level) return false;
            return true;
        }
    }
}
