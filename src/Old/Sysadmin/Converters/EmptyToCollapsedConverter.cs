using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace SysAdmin.Converters
{
    public class EmptyToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            if (value == null)
                return Visibility.Collapsed;

            if (value is string)
                if (string.IsNullOrEmpty(value.ToString()))
                    return Visibility.Collapsed;

            return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
