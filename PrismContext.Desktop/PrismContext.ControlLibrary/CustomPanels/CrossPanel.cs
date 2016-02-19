using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PrismContext.ControlLibrary.CustomPanels
{
    public class CrossPanel : StackPanel
    {
        static CrossPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CrossPanel), new FrameworkPropertyMetadata(typeof(CrossPanel)));
        }

        public static readonly DependencyProperty SpreadProperty = DependencyProperty.Register("Spread", typeof (double),
            typeof (CrossPanel),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));

        public double Spread
        {
            get { return (double) GetValue(SpreadProperty); }
            set { SetValue(SpreadProperty, value); }
        }
        
        protected override Size MeasureOverride(Size constraint)
        {
            double maxChildWidth = 0d, maxChildHeight = 0d;
            foreach (FrameworkElement child in Children)
            {
                child.Measure(constraint);
                if (child.DesiredSize.Width >= maxChildWidth)
                    maxChildWidth = child.DesiredSize.Width;
                if (child.DesiredSize.Height >= maxChildHeight)
                    maxChildHeight = child.DesiredSize.Height;
            }

            var panelSize = new Size
            {
                Width = 2 * (Spread + maxChildWidth),
                Height = 2 * (Spread + maxChildHeight)
            };

            return panelSize;
        }

        //private Point _panelCenter, _originLeft, _originRight;
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var panelCenter = new Point
            {
                X = arrangeSize.Width/2d,
                Y = arrangeSize.Height/2d
            };

            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i] as FrameworkElement;

                var origin = new Point
                {
                    X = (i % 1 == 0) ? panelCenter.X + Spread : panelCenter.X - Spread,
                    Y = (i % 1 == 0) ? panelCenter.Y + Spread : panelCenter.Y - Spread
                };

                child.Arrange(new Rect(origin + new Vector(0d, ((i+1)/2) * child.DesiredSize.Height), child.DesiredSize));
            }

            return arrangeSize;
        }
    }
}
