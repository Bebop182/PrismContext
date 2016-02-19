using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PrismContext.ControlLibrary.CustomControls
{
    public class MatrixItem : ListBoxItem
    {
        public static readonly DependencyProperty IsDraggableProperty = DependencyProperty.Register("IsDraggable",
            typeof(bool), typeof(MatrixItem), new PropertyMetadata(false));

        public bool IsDraggable
        {
            get { return (bool)GetValue(IsDraggableProperty); }
            set { SetValue(IsDraggableProperty, value); }
        }
        static MatrixItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MatrixItem), new FrameworkPropertyMetadata(typeof(MatrixItem)));
        }

        #region event handling
        ///PREVIEW/////////////////////////////
        #region preview
        /// -- MOUSE
        #region mouse
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            return;
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            return;
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            return;
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            return;
        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            return;
        }

        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {
            return;
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            return;
        }
        #endregion

        /// -- TOUCH
        #region touch
        protected override void OnPreviewTouchDown(TouchEventArgs e)
        {
            IsSelected = true;
            return;
        }

        protected override void OnPreviewTouchUp(TouchEventArgs e)
        {
            //if (!DragManager.Instance.doingDrag || DragManager.Instance.DragCallBack == null) return;
            //DragManager.Instance.DragCallBack.Execute(this.DataContext);
            //DragManager.Instance.StopDrag();
            //e.Handled = true;
            //return;
        }

        protected override void OnPreviewTouchMove(TouchEventArgs e)
        {
            return;
        }
        #endregion

        /// -- STYLUS
        #region stylus
        protected override void OnPreviewStylusDown(StylusDownEventArgs e)
        {
            return;
        }

        protected override void OnPreviewStylusUp(StylusEventArgs e)
        {
            return;
        }

        protected override void OnPreviewStylusButtonDown(StylusButtonEventArgs e)
        {
            return;
        }

        protected override void OnPreviewStylusButtonUp(StylusButtonEventArgs e)
        {
            return;
        }

        protected override void OnPreviewStylusMove(StylusEventArgs e)
        {
            return;
        }
        #endregion
        #endregion

        ///BUBLING/////////////////////////////
        #region bubling
        /// -- MOUSE
        #region mouse
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            return;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            return;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            return;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            return;
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            return;
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            return;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            return;
        }
        #endregion

        /// -- TOUCH
        #region touch
        protected override void OnTouchDown(TouchEventArgs e)
        {
            return;
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            return;
        }

        protected override void OnTouchMove(TouchEventArgs e)
        {
            return;
        }

        #endregion

        /// -- STYLUS
        #region stylus
        protected override void OnStylusDown(StylusDownEventArgs e)
        {
            return;
        }

        protected override void OnStylusUp(StylusEventArgs e)
        {
            return;
        }

        protected override void OnStylusButtonDown(StylusButtonEventArgs e)
        {
            return;
        }

        protected override void OnStylusButtonUp(StylusButtonEventArgs e)
        {
            return;
        }

        protected override void OnStylusMove(StylusEventArgs e)
        {
            return;
        }

        #endregion
        #endregion
        #endregion

        #region layout
        //protected override Size MeasureOverride(Size constraint)
        //{
        //    if(VisualChildrenCount <= 0) return new Size(0,0);
        //    var child = (UIElement)GetVisualChild(0);
        //    child.Measure(constraint);
        //    return child.DesiredSize;
        //}
        //protected override Size ArrangeOverride(Size arrangeBounds)
        //{
        //    if (VisualChildrenCount > 0) {
        //        var child = (UIElement)GetVisualChild(0);
        //        if (child != null)
        //            child.Arrange(new Rect(arrangeBounds));
        //    }
        //    return arrangeBounds;
        //}
        #endregion
    }
}
