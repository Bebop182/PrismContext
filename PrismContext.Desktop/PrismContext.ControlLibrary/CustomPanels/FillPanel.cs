using System.Windows;

namespace PrismContext.ControlLibrary.CustomPanels
{
    /// <summary>
    /// Fill pannel dispatch available space equaly amongst all its children,
    /// it takes the largest child as reference and account for Orientation property,
    /// however setting DisplayDiagonaly will ignore the former.
    /// </summary>

    //Todo: Allow to chose between fill proportionnally and fill equally (Fill equally is current behavior)
    public class FillPanel : OrientedPanel
    {
        protected override Size ArrangeOverride(Size finalSize)
        {
            var childSize = new Size
            {
                Width = IsHorizontal ? finalSize.Width / VisibleChildrenCount : finalSize.Width,
                Height = IsVertical ? finalSize.Height / VisibleChildrenCount : finalSize.Height
            };

            double offsetX = 0, offsetY = 0;

            foreach (UIElement child in Children)
            {
                if (child.Visibility == Visibility.Collapsed) continue;
                
                child.Arrange(new Rect(new Point(offsetX, offsetY), childSize));
                offsetX += IsHorizontal ? childSize.Width : 0;
                offsetY += IsVertical ? childSize.Height : 0;
            }
            return finalSize;
        }
    }
}
