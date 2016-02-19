using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PrismContext.ControlLibrary.CustomControls;

namespace PrismContext.ControlLibrary.CustomItemsControls
{
    public class MatrixN2 : ListBox
    {
        #region properties
        private Point _lastClickedPosition;
        #endregion

        #region DependencyProperties
        public static readonly DependencyProperty HeaderListProperty =
            DependencyProperty.Register("HeaderList", typeof(IEnumerable), typeof(MatrixN2), new UIPropertyMetadata(new List<string>()));
        public IEnumerable HeaderList
        {
            get { return (IEnumerable)GetValue(HeaderListProperty); }
            set { SetValue(HeaderListProperty, value); }
        }

        public static readonly DependencyProperty HeaderDataTemplateProperty =
            DependencyProperty.Register("HeaderDataTemplate", typeof(DataTemplate), typeof(MatrixN2), new PropertyMetadata(null));
        public DataTemplate HeaderDataTemplate
        {
            get { return (DataTemplate) GetValue(HeaderDataTemplateProperty); }
            set { SetValue(HeaderDataTemplateProperty, value);}
        }

        public static readonly DependencyProperty HeadersVisibilityProperty =
            DependencyProperty.Register("HeadersVisibility", typeof (Thickness), typeof (MatrixN2), new UIPropertyMetadata(new Thickness(1)));
        public Thickness HeadersVisibility
        {
            get { return (Thickness) GetValue(HeadersVisibilityProperty); }
            set { SetValue(HeadersVisibilityProperty, value);}
        }

        public static readonly DependencyProperty DragCallBackCommandProperty =
            DependencyProperty.Register("DragCallBackCommand", typeof(ICommand), typeof(MatrixN2), new PropertyMetadata(null));
        public ICommand DragCallBackCommand
        {
            get { return (ICommand) GetValue(DragCallBackCommandProperty); }
            set { SetValue(DragCallBackCommandProperty, value); }
        }

        public static readonly DependencyProperty EditEnabledProperty = DependencyProperty.Register("EditEnabled",
            typeof(bool), typeof(MatrixN2), new UIPropertyMetadata(true));
        public bool EditEnabled
        {
            get
            {
                return (bool) GetValue(EditEnabledProperty);
                
            }
            set { SetValue(EditEnabledProperty, value);}
        }
        #endregion

        #region Init
        static MatrixN2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MatrixN2), new FrameworkPropertyMetadata(typeof(MatrixN2)));
        }

        public MatrixN2()
        {
            _lastClickedPosition = new Point(-1, -1);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MatrixItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MatrixItem;
        }
        #endregion

        #region LayoutHandlers
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            //1st Child is GRID
            if (VisualChildrenCount == 0) return new Size();
            var child = (UIElement)GetVisualChild(0);
            if (child != null)
            {
                child.Measure(constraint);

                return child.DesiredSize;
            }
            return new Size();
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            if (VisualChildrenCount == 0) return DesiredSize;
            var child = (UIElement)GetVisualChild(0);
            if (child != null)
            {
                child.Arrange(new Rect(arrangeBounds));
            }
            return arrangeBounds;
        }
        #endregion

        #region Events
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
            _lastClickedPosition = e.GetTouchPoint(this).Position;
            return;
        }

        protected override void OnPreviewTouchUp(TouchEventArgs e)
        {
            _lastClickedPosition = new Point(-1, -1);
            return;
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
            //if (DragManager.Instance.doingDrag)
            //{
            //    DragManager.Instance.StopDrag();
            //}
            return;
        }

        protected override void OnTouchMove(TouchEventArgs e)
        {
            //if (!IsDragConfirmed(e.GetTouchPoint(this).Position)) return;
            //var itemContainer = (MatrixItem)ItemContainerGenerator.ContainerFromItem(SelectedItem);
            //if (itemContainer != null && itemContainer.IsDraggable)
            //{
            //    if (!DragManager.Instance.doingDrag)
            //        DragManager.Instance.StartDrag(this, itemContainer, DragCallBackCommand);
            //    else DragManager.Instance.UpdateVisual(e.GetTouchPoint(this).Position);
            //}
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

        #region Helpers
        private bool IsDragConfirmed(Point currentCursorPosition)
        {
            if (_lastClickedPosition.X < 0 || _lastClickedPosition.Y < 0) return false;
            bool horizontalMovement = Math.Abs(currentCursorPosition.X - _lastClickedPosition.X) > SystemParameters.MinimumHorizontalDragDistance,
                verticalMovement = Math.Abs(currentCursorPosition.Y - _lastClickedPosition.Y) > SystemParameters.MinimumVerticalDragDistance;
            return (horizontalMovement | verticalMovement);
        }

        bool InContact(MouseEventArgs e)
        {
            bool isStylusTouching = e.StylusDevice != null && e.StylusDevice.InRange && !e.StylusDevice.InAir;
            if(!isStylusTouching)
                if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
                    return true;
            return isStylusTouching;
        }
        #endregion
    }
}
