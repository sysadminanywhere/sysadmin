using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace SysAdmin.Converters
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is List<string>)
            {
                List<string> list = (List<string>)value;
                return string.Join(", ", list);
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}