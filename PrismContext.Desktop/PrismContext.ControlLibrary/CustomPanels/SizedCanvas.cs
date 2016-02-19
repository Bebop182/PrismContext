using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PrismContext.ControlLibrary.CustomPanels
{
    public class SizedCanvas : Panel
    {
        #region Properties
        private int _childCount;
        #endregion

        #region Attached Properties
        public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached("Left", typeof(double), typeof(SizedCanvas),
                    new FrameworkPropertyMetadata(Double.NaN, OnPositioningChanged), IsNotInfiniteDouble);

        public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached("Top", typeof(double), typeof(SizedCanvas),
                    new FrameworkPropertyMetadata(Double.NaN, OnPositioningChanged), IsNotInfiniteDouble);

        public static readonly DependencyProperty RightProperty = DependencyProperty.RegisterAttached("Right", typeof(double), typeof(SizedCanvas),
                    new FrameworkPropertyMetadata(Double.NaN, OnPositioningChanged), IsNotInfiniteDouble);

        public static readonly DependencyProperty BottomProperty = DependencyProperty.RegisterAttached("Bottom", typeof(double), typeof(SizedCanvas),
                    new FrameworkPropertyMetadata(Double.NaN, OnPositioningChanged), IsNotInfiniteDouble);
        #endregion

        #region Getters / Setters
        //Getters
        [AttachedPropertyBrowsableForChildren()]
        public static double GetLeft(UIElement element)
        {
            return (double)element.GetValue(LeftProperty);
        }

        [AttachedPropertyBrowsableForChildren()]
        public static double GetTop(UIElement element)
        {
            return (double)element.GetValue(TopProperty);
        }

        [AttachedPropertyBrowsableForChildren()]
        public static double GetRight(UIElement element)
        {
            return (double)element.GetValue(LeftProperty);
        }

        [AttachedPropertyBrowsableForChildren()]
        public static double GetBottom(UIElement element)
        {
            return (double)element.GetValue(LeftProperty);
        }

        //Setters
        [AttachedPropertyBrowsableForChildren()]
        public static void SetLeft(UIElement element, double value)
        {
            element.SetValue(LeftProperty, value);
        }

        [AttachedPropertyBrowsableForChildren()]
        public static void SetTop(UIElement element, double value)
        {
            element.SetValue(TopProperty, value);
        }

        [AttachedPropertyBrowsableForChildren()]
        public static void SetRight(UIElement element, double value)
        {
            element.SetValue(RightProperty, value);
        }

        [AttachedPropertyBrowsableForChildren()]
        public static void SetBottom(UIElement element, double value)
        {
            element.SetValue(LeftProperty, value);
        }
        #endregion

        #region Layout Methods
        protected override Size MeasureOverride(Size availableSize)
        {
            _childCount = InternalChildren.Count;
            var totalChildSize = new Size();

            const float maxHeight = 200;
            const float maxWidth = 200;
            double curHeight = availableSize.Height / (_childCount == 0 ? 1 : _childCount);
            var childSize = new Size()
            {
                Height = Convert.ToInt32(maxHeight > curHeight ? curHeight : maxHeight),
                Width = Convert.ToInt32((maxHeight > curHeight ? curHeight : maxHeight) * (maxWidth / maxHeight))
            };

            foreach (UIElement child in InternalChildren)
            {
                if (child == null) continue;

                totalChildSize.Height += childSize.Height;
                totalChildSize.Width += childSize.Width;

                child.Measure(childSize);
            }
            return new Size(totalChildSize.Width, totalChildSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            const float maxHeight = 200;
            const float maxWidth = 200;
            double curHeight = finalSize.Height / (_childCount == 0 ? 1 : _childCount);
            var childSize = new Size()
            {
                Height = Convert.ToInt32(maxHeight > curHeight ? curHeight : maxHeight),
                Width = Convert.ToInt32((maxHeight > curHeight ? curHeight : maxHeight) * (maxWidth / maxHeight))
            };
            var offset = new Point(0, 0);
            //int cpt = 0;
            foreach (UIElement child in InternalChildren)
            {
                double x = 0d, y = 0d;

                double left = GetLeft(child);
                if (!Double.IsNaN(left))
                    x = left;
                else
                {
                    double right = GetRight(child);
                    if (!Double.IsNaN(right))
                        x = finalSize.Width - childSize.Width - right;
                }

                double top = GetTop(child);
                if (!Double.IsNaN(top))
                    y = top;
                else
                {
                    double bottom = GetBottom(child);
                    if (!Double.IsNaN(bottom))
                        y = finalSize.Height - childSize.Height - bottom;
                }
                child.Arrange(new Rect(new Point(Math.Max(x, 0), Math.Max(y, 0)), childSize));
            }

            return DesiredSize;
        }
        #endregion

        #region Methods & Events
        private static bool IsNotInfiniteDouble(object value)
        {
            var d = (double)value;
            return !(Double.IsInfinity(d));
        }

        private static void OnPositioningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            if (element == null) return;

            //Récupérer l'instance du canvas (event static can't use this)
            var sc = VisualTreeHelper.GetParent(element) as SizedCanvas;
            if (sc == null) return;

            //Demander le redraw
            sc.InvalidateVisual();
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            if (ClipToBounds)
                return new RectangleGeometry(new Rect(RenderSize));
            return null;
        }
        #endregion
    }
}
