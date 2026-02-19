using DataAccess.Models;
using System.Globalization;
using System.Windows;
using System;

namespace RTK_HMI.Views.Resourses.Converters
{
    internal class UserToBoolConveterter:Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || parameter is null) return false;
            if (!(value is User user)) return false;
            if (!(parameter is UserLevel level))return false;
            if (user.Level >= level) return true;
            return false;
        }
    }
}
