using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using PrismContext.ControlLibrary.CustomPanels;

namespace PrismContext.ControlLibrary.Converters
{
    class InvertOrientationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(PanelOrientations))
            {
                return ((Orientation)value) == Orientation.Horizontal ? PanelOrientations.Vertical : PanelOrientations.Horizontal;
            }
            if (targetType == typeof(Orientation))
            {
                var orientation = (Orientation?) value;
                if (orientation == null) return Orientation.Horizontal;
                return orientation.Value == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;;
            }
            return Orientation.Horizontal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
