using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace SysAdmin.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
