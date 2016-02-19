using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using PrismContext.ControlLibrary.Adorners;
using PrismContext.ControlLibrary.Helpers;

namespace PrismContext.ControlLibrary.AttachedBehaviours
{
    //TODO: Improve drag start detection when on axis constraint
    public class DragBehavior : Behavior<ItemsControl>
    {
        #region Dependency Properties
        // callback prototype: void callback(object[] param) // param[0]: ItemID, param[1]: Item, param[2]: DataContext
        public static readonly DependencyProperty DragCallbackProperty =
            DependencyProperty.Register("DragCallback", typeof(ICommand), typeof(DragBehavior));

        public static readonly DependencyProperty WhiteListProperty =
            DependencyProperty.Register("WhiteList", typeof(Type), typeof(DragBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty BlackListProperty =
            DependencyProperty.Register("BlackList", typeof(Type), typeof(DragBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty AxisConstraintProperty =
            DependencyProperty.Register("AxisConstraint", typeof(AxisConstraint), typeof(DragBehavior), new PropertyMetadata(AxisConstraint.None));
        #endregion

        #region Public Properties
        public ICommand DragCallback
        {
            get { return (ICommand)GetValue(DragCallbackProperty); }
            set
            {
                SetValue(DragCallbackProperty, value);
            }
        }

        public Type WhiteList
        {
            get { return (Type)GetValue(WhiteListProperty); }
            set
            {
                SetValue(WhiteListProperty, value);
            }
        }

        public Type BlackList
        {
            get { return (Type)GetValue(BlackListProperty); }
            set
            {
                SetValue(BlackListProperty, value);
            }
        }

        public AxisConstraint AxisConstraint
        {
            get { return (AxisConstraint)GetValue(AxisConstraintProperty); }
            set { SetValue(AxisConstraintProperty, value);}
        }

        #region Drags Events
        public delegate void DragEvent(object itemContainer);

        public static event DragEvent DragStart, DragEnd, DragCancel;

        public static void TriggerDragStart(object itemContainer)
        {
            if (DragStart != null)
                DragStart(itemContainer);
        }

        public static void TriggerDragEnd(object itemContainer)
        {
            if (DragEnd != null)
                DragEnd(itemContainer);
        }

        public static void TriggerDragCancel(object itemContainer)
        {
            if (DragCancel != null)
                DragCancel(itemContainer);
        }
        #endregion
        #endregion

        #region Private Properties
        private bool _isPressed;
        private Point _startPosition;
        private DragAdorner _dragFeedback;

        // Check if there's a active binding of the ItemsSource, if not, adding items directly to the items property is allowed
        private bool IsItemsSourceBound
        {
            get
            {
                return AssociatedObject.ItemsSource != null;
            }
        }
        #endregion

        #region Initialization
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseLeftButtonDown += OnPress;
            AssociatedObject.MouseLeftButtonUp += OnRelease;
            AssociatedObject.MouseMove += OnMove;
            AssociatedObject.GiveFeedback += GiveFeedBack;
        }
        #endregion
        
        #region Events
        private void OnPress(object sender, MouseButtonEventArgs e)
        {
            _isPressed = true;
            _startPosition = e.GetPosition(AssociatedObject);
        }

        private void OnRelease(object sender, MouseButtonEventArgs e)
        {
            _isPressed = false;
            _startPosition = new Point();
        }

        private void OnMove(object sender, MouseEventArgs e)
        {
            // Check if inputs meet drag requirements
            if (!DragConfirmed(e.GetPosition(AssociatedObject))) return;

            // Check if pointer over container
            var container = AssociatedObject.GetTargetContainer(_startPosition);
            if (container == null) return;

            var dragIndex = AssociatedObject.ItemContainerGenerator.IndexFromContainer(container);
            var item = AssociatedObject.Items[dragIndex];

            if (!IsDraggable(item))
                return;

            _isPressed = false;

            // TODO: Find better alternative: excluding FrameworkElements forbid the drag of xaml declared data.
            if (item is FrameworkElement)
                return;

            // Subscribe only if drag is confirmed on associated ItemsControl
            DragStart += OnDragStart;
            DragEnd += OnDragEnd;
            DragCancel += OnDragCancel;

            // Start Drag
            StartDrag(dragIndex, item);
        }

        private void OnDragStart(object itemContainer)
        {
            //Console.WriteLine("DRAG STARTED !!!");
            var container = (FrameworkElement) itemContainer;

            //CreateBitmap of dragged element
            var bmp = container.GenerateBitMap();
            _dragFeedback = new DragAdorner(AssociatedObject, bmp);
        }

        private void OnDragEnd(object itemContainer)
        {
            ClearDrag();
        }

        private void OnDragCancel(object itemContainer)
        {
            ClearDrag();
        }

        private void GiveFeedBack(object sender, GiveFeedbackEventArgs giveFeedbackEventArgs)
        {
            
        }
        #endregion

        #region Drag Logic
        private void StartDrag(int index, object item)
        {
            var container = AssociatedObject.ItemContainerGenerator.ContainerFromIndex(index);
            TriggerDragStart(container);

            var operation = DragDrop.DoDragDrop(AssociatedObject, item, DragDropEffects.Move);

            if (operation == DragDropEffects.None)
            {
                TriggerDragCancel(container);
                return;
            }

            // If item at index is not the expected one, correct index
            if (!item.Equals(AssociatedObject.Items[index]))
                index++;
            
            if (IsItemsSourceBound)
            {
                if (DragCallback != null)
                {
                    object parameters = new[] {index, item, AssociatedObject.DataContext};
                    DragCallback.Execute(parameters);
                }
            }
            else
            {
                AssociatedObject.Items.RemoveAt(index);
            }

            TriggerDragEnd(container);
        }

        private bool DragConfirmed(Point newPosition)
        {
            double deltaX = Math.Abs(_startPosition.X - newPosition.X),
                deltaY = Math.Abs(_startPosition.Y - newPosition.Y);

            bool isDeltaValid = false;
            switch (AxisConstraint)
            {
                case AxisConstraint.Vertical:
                    isDeltaValid = deltaY > SystemParameters.MinimumVerticalDragDistance &&
                                   deltaX <= SystemParameters.MinimumHorizontalDragDistance;
                    break;
                case AxisConstraint.Horizontal:
                    isDeltaValid = deltaY <= SystemParameters.MinimumVerticalDragDistance &&
                                   deltaX > SystemParameters.MinimumHorizontalDragDistance;
                    break;
                default:
                    isDeltaValid = deltaX > SystemParameters.MinimumHorizontalDragDistance ||
                                   deltaY > SystemParameters.MinimumVerticalDragDistance;
                    break;
            }

            return _isPressed && isDeltaValid;
        }

        private bool IsDraggable(object item)
        {
            // If Whitelist active
            if (WhiteList != null)
            {
                if (!WhiteList.IsInstanceOfType(item))
                    return false;
            }
            //If no Whitelist but Blacklist
            else if (BlackList != null)
            {
                if (BlackList.IsInstanceOfType(item))
                    return false;
            }

            return true;
        }

        private void ClearDrag()
        {
            if (_dragFeedback != null)
            {
                _dragFeedback.Destroy();
                _dragFeedback = null;
            }

            // Unsubscribe
            DragStart -= OnDragStart;
            DragEnd -= OnDragEnd;
            DragCancel -= OnDragCancel;
        }
        #endregion
    }

    public enum AxisConstraint
    {
        Vertical,
        Horizontal,
        None
    }
}
