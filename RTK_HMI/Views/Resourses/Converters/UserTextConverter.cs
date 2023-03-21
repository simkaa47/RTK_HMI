using DataAccess.Models;
using System;
using System.Globalization;

namespace RTK_HMI.Views.Resourses.Converters
{
    class UserTextConverter:Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "Пользователь не авторизован";
            var user = value as User;
            if (user == null) return "Пользователь не авторизован";
            return user.LastName + " " + user.FirstName + $"({user.Level})";
        }
    }
}
