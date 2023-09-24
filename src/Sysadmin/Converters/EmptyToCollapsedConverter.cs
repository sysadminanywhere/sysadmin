using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sysadmin.Converters
{
    public class EmptyToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            if (value is string)
                if (string.IsNullOrEmpty(value.ToString()))
                    return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
