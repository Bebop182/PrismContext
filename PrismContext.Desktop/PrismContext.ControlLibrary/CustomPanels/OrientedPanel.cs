using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PrismContext.ControlLibrary.CustomPanels
{
    public class OrientedPanel : StackPanel
    {
        #region Dependency Properties
        public new static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(PanelOrientations), typeof(OrientedPanel),
                new UIPropertyMetadata(PanelOrientations.Vertical, Orientation_PropertyChangedCallback));

        private static void Orientation_PropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var panel = o as FrameworkElement;
            if (panel == null) return;
            panel.InvalidateVisual();
        }

        public new PanelOrientations Orientation
        {
            get { return (PanelOrientations)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        protected new PanelOrientations LogicalOrientation
        {
            get { return Orientation; }
        }
        #endregion

        #region Protected Properties
        protected int VisibleChildrenCount
        {
            get
            {
                return Children.Cast<UIElement>().Count(child => child.Visibility != Visibility.Collapsed);
            }
        }

        protected Size ChildrenDesiredSize;

        protected Size MaxChildDesiredSize;

        protected bool IsHorizontal
        {
            get { return (Orientation & PanelOrientations.Horizontal) == PanelOrientations.Horizontal; }
        }

        protected bool IsVertical
        {
            get { return (Orientation & PanelOrientations.Vertical) == PanelOrientations.Vertical; }
        }

        protected bool IsDiagonal
        {
            get { return (Orientation & PanelOrientations.Diagonal) == PanelOrientations.Diagonal; }
        }
        #endregion

        #region Layout Logic
        protected override Size MeasureOverride(Size constraint)
        {
            MaxChildDesiredSize = new Size();
            ChildrenDesiredSize = new Size();

            foreach (UIElement child in Children)
            {
                if (child.Visibility == Visibility.Collapsed) continue;

                child.Measure(constraint);

                if (child.DesiredSize.Width >= MaxChildDesiredSize.Width)
                    MaxChildDesiredSize.Width = child.DesiredSize.Width;
                if (child.DesiredSize.Height >= MaxChildDesiredSize.Height)
                    MaxChildDesiredSize.Height = child.DesiredSize.Height;

                ChildrenDesiredSize.Width += child.DesiredSize.Width;
                ChildrenDesiredSize.Height += child.DesiredSize.Height;
            }

            var panelDesiredSize = new Size
            {
                Width = constraint.Width <= ChildrenDesiredSize.Width ? constraint.Width : ChildrenDesiredSize.Width,
                Height = constraint.Height <= ChildrenDesiredSize.Height ? constraint.Height : ChildrenDesiredSize.Height
            };

            // Make sure that children didn't ignore constraint
            if (!IsDiagonal)
            {
                if (IsHorizontal)
                    panelDesiredSize.Height = !constraint.Height.Equals(double.PositiveInfinity) && constraint.Height >= MaxChildDesiredSize.Height
                        ? constraint.Height
                        : MaxChildDesiredSize.Height;
                if (IsVertical)
                    panelDesiredSize.Width = !constraint.Width.Equals(double.PositiveInfinity) && constraint.Width >= MaxChildDesiredSize.Width
                        ? constraint.Width
                        : MaxChildDesiredSize.Width;
            }

            return panelDesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double offsetX = 0, offsetY = 0;

            foreach (UIElement child in Children)
            {
                if (child.Visibility == Visibility.Collapsed) continue;

                var childSize = new Size
                {
                    Width = IsVertical ? finalSize.Width : child.DesiredSize.Width,
                    Height = IsHorizontal ? finalSize.Height : child.DesiredSize.Height
                };
                child.Arrange(new Rect(new Point(offsetX, offsetY), childSize));

                offsetX += IsHorizontal ? child.DesiredSize.Width : 0;
                offsetY += IsVertical ? child.DesiredSize.Height : 0;
            }
            return finalSize;
        }
        #endregion
    }

    [Flags]
    public enum PanelOrientations
    {
        Horizontal = 1,
        Vertical = 2,
        Diagonal = Horizontal | Vertical
    }
}
