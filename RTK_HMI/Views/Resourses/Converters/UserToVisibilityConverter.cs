using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace RTK_HMI.Views.Resourses.Converters
{
    class UserToVisibilityConverter:Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || parameter is null) return Visibility.Collapsed;
            if(!(value is User user)) return Visibility.Collapsed;
            if(user.Level.ToString()==parameter.ToString())return Visibility.Visible;
            return Visibility.Collapsed;
        }
    }
}
