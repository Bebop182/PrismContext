using System;
using System.Windows;

namespace PrismContext.ControlLibrary.CustomPanels
{
    public class OverlapPanel : OrientedPanel
    {
        #region Layout
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count <= 0) return finalSize;

            Size lastChildSize = GetLastVisibleChildSize();

            double
                offsetXRange = finalSize.Width - lastChildSize.Width,
                offsetYRange = finalSize.Height - lastChildSize.Height,
                offsetXIncrement = offsetXRange / (Children.Count - 1),
                offsetYIncrement = offsetYRange / (Children.Count - 1),
                offsetX = 0, offsetY = 0;

            bool hasEnouthWidth = finalSize.Width >= ChildrenDesiredSize.Width;
            bool hasEnouthHeight = finalSize.Height >= ChildrenDesiredSize.Height;

            foreach (UIElement child in Children)
            {
                var childSize = new Size
                {
                    Width = IsVertical && !IsDiagonal ? finalSize.Width : child.DesiredSize.Width,
                    Height = IsHorizontal && !IsDiagonal ? finalSize.Height : child.DesiredSize.Height
                };
                child.Arrange(new Rect(new Point(offsetX, offsetY), childSize));

                if(IsHorizontal)
                    offsetX += hasEnouthWidth ? child.DesiredSize.Width : offsetXIncrement;
                if(IsVertical)
                    offsetY += hasEnouthHeight ? child.DesiredSize.Height : offsetYIncrement;
            }

            return finalSize;
        }
        #endregion

        private Size GetLastVisibleChildSize()
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                if (Children[i].Visibility != Visibility.Collapsed)
                    return Children[i].DesiredSize;
            }
            throw new InvalidOperationException("No visible child found");
        }
    }
}
