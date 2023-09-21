using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sysadmin.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Collapsed;

            switch ((bool?)value)
            {
                case null:
                    result = Visibility.Collapsed;
                    break;

                case true:
                    result = Visibility.Visible;
                    break;

                case false:
                    result = Visibility.Collapsed;
                    break;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
