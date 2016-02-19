using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PrismContext.ControlLibrary.CustomContentControls
{
    public class ZoomPanScrollViewer : ScrollViewer
    {
        #region DependencyProperties

        public static readonly DependencyProperty ZoomValueProperty = DependencyProperty.Register("ZoomValue",
            typeof(double), typeof(ZoomPanScrollViewer), new UIPropertyMetadata(1d));
        public double ZoomValue
        {
            get { return (double)GetValue(ZoomValueProperty); }
            set
            {
                if (value > MinZoomValue && value < MaxZoomValue)
                    SetValue(ZoomValueProperty, value);
            }
        }

        public static readonly DependencyProperty MaxZoomValueProperty = DependencyProperty.Register("MaxZoomValue",
            typeof(double), typeof(ZoomPanScrollViewer), new UIPropertyMetadata(3d));
        public double MaxZoomValue
        {
            get { return (double)GetValue(MaxZoomValueProperty); }
            set { SetValue(MaxZoomValueProperty, value); }
        }

        public static readonly DependencyProperty MinZoomValueProperty = DependencyProperty.Register("MinZoomValue",
            typeof(double), typeof(ZoomPanScrollViewer), new UIPropertyMetadata(0.5d));
        public double MinZoomValue
        {
            get { return (double)GetValue(MinZoomValueProperty); }
            set { SetValue(MinZoomValueProperty, value); }
        }

        public static readonly DependencyProperty ZoomIncrementProperty = DependencyProperty.Register("ZoomIncrement",
            typeof(double), typeof(ZoomPanScrollViewer), new UIPropertyMetadata(0.1d));
        public double ZoomIncrement
        {
            get { return (double)GetValue(ZoomIncrementProperty); }
            set { SetValue(ZoomIncrementProperty, value); }
        }

        public static readonly DependencyProperty PanSensitivityProperty = DependencyProperty.Register("PanSensitivity",
            typeof(double), typeof(ZoomPanScrollViewer), new UIPropertyMetadata(1d));
        public double PanSensitivity
        {
            get { return (double)GetValue(PanSensitivityProperty) / ZoomValue; }
            set { SetValue(PanSensitivityProperty, value); }
        }

        #endregion

        #region Properties

        //Pan Properties
        private Point _lastMousePosition;
        private Point _newMousePosition;
        private Point _mouseDelta;

        //Zoom Properties
        private List<TouchPoint> touches = new List<TouchPoint>();

        private DependencyObject _contentPresenter;
        private FrameworkElement _child;

        #endregion

        #region Init

        static ZoomPanScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZoomPanScrollViewer), new FrameworkPropertyMetadata(typeof(ZoomPanScrollViewer)));
        }

        public ZoomPanScrollViewer()
        {
            IsManipulationEnabled = false;
            Loaded += ZoomPanScrollViewer_Loaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _contentPresenter = GetTemplateChild("PART_ScrollContentPresenter");
        }

        protected void ZoomPanScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            _child = (FrameworkElement)VisualTreeHelper.GetChild(_contentPresenter, 0);
        }

        // Stop window reaction to touch
        protected override void OnManipulationBoundaryFeedback(ManipulationBoundaryFeedbackEventArgs e)
        {
            base.OnManipulationBoundaryFeedback(e);
            e.Handled = true;
        }

        #endregion

        #region EventHandling

        protected override void OnPreviewTouchDown(TouchEventArgs e)
        {
            //new touch
            touches.Add(e.GetTouchPoint(this));
        }

        protected override void OnPreviewTouchUp(TouchEventArgs e)
        {
            //remove touch
            touches.Remove(e.GetTouchPoint(this));
        }

        //Pan
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            //if (!Properties.Settings.Default.PanAllowed) return;
            _newMousePosition = e.GetPosition(this);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (ScrollableHeight.Equals(0) || ScrollableWidth.Equals(0))
                    return;
                //e.Handled = true;
                var panCursor = new Point
                {
                    X = ContentHorizontalOffset,
                    Y = ContentVerticalOffset
                };

                //Calculate mouseDelta
                _mouseDelta = new Point(_lastMousePosition.X - _newMousePosition.X, _lastMousePosition.Y - _newMousePosition.Y); // S+ W+ inverted scrolling

                if (!_mouseDelta.X.Equals(0))
                    panCursor.X += (PanSensitivity * _mouseDelta.X) * ZoomValue;
                if (!_mouseDelta.Y.Equals(0))
                    panCursor.Y += PanSensitivity * _mouseDelta.Y * ZoomValue;

                if (!panCursor.X.Equals(ContentHorizontalOffset))
                    ScrollToHorizontalOffset(panCursor.X);
                if (!panCursor.Y.Equals(ContentVerticalOffset))
                    ScrollToVerticalOffset(panCursor.Y);
            }
            _lastMousePosition = _newMousePosition;
        }

        //Zoom
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            if (_child == null) return;
            e.Handled = true;

            if (e.Delta > 0) //zoomIn
            {
                // todo: Zoom start from center to target as zoomValue increase
                ZoomValue = Clamp(ZoomValue + ZoomIncrement, MinZoomValue, MaxZoomValue);
            }
            else if (e.Delta < 0) //zoom out
            {
                ZoomValue = Clamp(ZoomValue - ZoomIncrement, MinZoomValue, MaxZoomValue);
            }
            else return;

            _child.LayoutTransform = new ScaleTransform(ZoomValue, ZoomValue);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (ScrollInfo.ExtentHeight > 0 || ScrollInfo.ExtentWidth > 0)
                ZoomValue -= ZoomIncrement;
            return base.ArrangeOverride(arrangeSize);
        }

        #endregion

        #region helpers

        //Methods
        protected T Clamp<T>(T v, T min, T max) where T : IComparable<T>
        {
            if (v.CompareTo(min) >= 0 && v.CompareTo(max) <= 0)
                return v;
            return v.CompareTo(min) < 0 ? min : max;
        }

        protected Point ClampPointToRect(Point p, Rect a)
        {
            p.X = Clamp(p.X, a.Left, a.Width);
            p.Y = Clamp(p.Y, a.Top, a.Height);
            return p;
        }

        protected double Normalize(double v)
        {
            return Clamp(v, -1, 1);
        }

        protected Point NormalizePoint(Point p)
        {
            p.X = Normalize(p.X);
            p.Y = Normalize(p.Y);
            return p;
        }

        protected double DistanceToPercent(double value, double sValue, double eValue)
        {
            return Math.Abs((value - sValue) / (eValue - sValue));
        }

        protected double Distance(Point start, Point end)
        {
            double x = Math.Abs(end.X - start.X);
            double y = Math.Abs(end.Y - start.Y);
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        #endregion
    }
}
