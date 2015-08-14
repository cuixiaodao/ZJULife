using System;
using Windows.UI.Xaml.Data;

namespace ZJULife.Converters
{
    internal class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string time = ((TimeSpan)value).ToString(@"hh\:mm");
            if (time == "00:00")
            {
                return "*";
            }
            return time;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}