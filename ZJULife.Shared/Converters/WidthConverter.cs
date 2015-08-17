using System;
using Windows.UI.Xaml.Data;

namespace ZJULife.Converters
{
    internal class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return double.Parse((string)parameter) * (double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}