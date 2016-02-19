using System;
using System.Windows;
using System.Windows.Controls;

namespace PrismContext.ControlLibrary.CustomPanels
{
    public class N2Panel : Panel
    {
        public int SquaredCount
        {
            get { return (int)Math.Ceiling(Math.Sqrt(Children.Count)); }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            double maxDesiredWidth = 0, maxDesiredHeight = 0;
            foreach (UIElement child in Children)
            {
                child.Measure(constraint);
                //if (child.Visibility == Visibility.Collapsed) continue;

                if (child.DesiredSize.Width >= maxDesiredWidth)
                {
                    maxDesiredWidth = child.DesiredSize.Width;
                }
                if (child.DesiredSize.Height >= maxDesiredHeight)
                {
                    maxDesiredHeight = child.DesiredSize.Height;
                }
            }
            return new Size(maxDesiredWidth * SquaredCount, maxDesiredHeight * SquaredCount);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var childSize = new Size()
            {
                Width = finalSize.Width/SquaredCount,
                Height = finalSize.Height/SquaredCount
            };

            int squaredCount = SquaredCount;
            double xOffset = 0d, yOffset = 0d;
            for (int i=0, y = 0; y < squaredCount; y++)
            {
                for (int x = 0; x < squaredCount; x++, i++)
                {
                    if (i >= Children.Count) break;
                    Children[i].Arrange(new Rect(new Point(xOffset, yOffset), childSize));
                    xOffset += childSize.Width;
                }
                xOffset = 0d;
                yOffset += childSize.Height;
                //Console.WriteLine("Total height: {0} / X offset {1}, Y offset {2}", finalSize.Height, xOffset, yOffset);
            }
            return finalSize;
        }
    }
}
