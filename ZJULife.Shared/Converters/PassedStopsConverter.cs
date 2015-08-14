using System;
using Windows.UI.Xaml.Data;

namespace ZJULife.Converters
{
    internal class PassedStopsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.ToString().Length < 2)
            {
                return "不经过其他校区";
            }
            return "经过" + value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}