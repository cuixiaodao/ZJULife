using System;
using Windows.UI.Xaml.Data;

namespace ZJULife.Converters
{
    internal class SourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return "ms-appx:///Assets/MapIcons/" + value + ".png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}