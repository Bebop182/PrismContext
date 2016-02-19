using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PrismContext.ControlLibrary.Helpers;

namespace PrismContext.ControlLibrary.CustomPanels
{
    public class CircularPanel : Panel
    {
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Radius", typeof(double), typeof(CircularPanel), new FrameworkPropertyMetadata(10d, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            "Angle", typeof (double), typeof (CircularPanel), new FrameworkPropertyMetadata(360d, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double Angle
        {
            get { return (double) GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        private Point _center;
        static CircularPanel()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(CircularPanel), new FrameworkPropertyMetadata(typeof(CircularPanel)));
        }

        public CircularPanel()
        {
            RenderTransformOrigin = new Point(0.5d, 0.5d);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            //dc.DrawEllipse(Brushes.Black, null, TranslatePoint(_center, this), Radius, Radius);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double maxWidth = 0d, maxHeight = 0d;
            foreach (FrameworkElement child in Children)
            {
                child.RenderTransformOrigin = new Point(0.5d, 0.5d);

                child.Measure(availableSize);
                if (child.DesiredSize.Width >= maxWidth)
                    maxWidth = child.DesiredSize.Width;
                if (child.DesiredSize.Height >= maxHeight)
                    maxHeight = child.DesiredSize.Height;
            }
            return new Size(maxWidth + 2 * Radius, maxHeight + 2 * Radius);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double currentAngle = 0;
            double angleStep = (Angle/Children.Count).DegToRad();
            
            _center = new Point(finalSize.Width/2f, finalSize.Height/2f);
            foreach (FrameworkElement child in Children)
            {
                currentAngle += angleStep;
                var x = Radius*Math.Sin(currentAngle);
                var position = _center + new Vector(x - (child.DesiredSize.Width / 2f), (x / Math.Tan(currentAngle) - (child.DesiredSize.Height / 2f)));
                Console.WriteLine();
                child.Arrange(new Rect(position, child.DesiredSize));
            }
            Console.WriteLine("FinalSize : " + finalSize);
            return finalSize;
        }
    }
}
