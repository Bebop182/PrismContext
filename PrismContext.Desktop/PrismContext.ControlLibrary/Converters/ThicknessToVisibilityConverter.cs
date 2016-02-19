using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PrismContext.ControlLibrary.Converters
{
    public class ThicknessToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = Int32.Parse(parameter.ToString());
            switch (index)
            {
                case 0:
                    return ((Thickness)value).Left > 0 ? Visibility.Visible : Visibility.Collapsed;
                case 1:
                    return ((Thickness)value).Top > 0 ? Visibility.Visible : Visibility.Collapsed;
                case 2: return ((Thickness)value).Right > 0 ? Visibility.Visible : Visibility.Collapsed;
                case 3: return ((Thickness)value).Bottom > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
