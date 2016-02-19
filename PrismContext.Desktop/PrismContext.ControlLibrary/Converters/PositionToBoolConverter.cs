using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PrismContext.ControlLibrary.Converters
{
    public class PositionToBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pos = (Point) value;
            return pos.X.Equals(pos.Y);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}