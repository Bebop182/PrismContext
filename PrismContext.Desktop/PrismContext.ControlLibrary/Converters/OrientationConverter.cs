using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using PrismContext.ControlLibrary.CustomPanels;

namespace PrismContext.ControlLibrary.Converters
{
    public class OrientationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(PanelOrientations))
            {
                return ((Orientation)value) == Orientation.Horizontal ? PanelOrientations.Horizontal : PanelOrientations.Vertical;
            }
            if (targetType == typeof(Orientation))
            {
                var orientation = (Orientation?) value;
                if (orientation == null) return Orientation.Vertical;
                return orientation.Value == Orientation.Horizontal ? Orientation.Horizontal : Orientation.Vertical;;
            }
            return Orientation.Vertical;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
