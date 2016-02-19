using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using PrismContext.ControlLibrary.Helpers;

namespace PrismContext.ControlLibrary.AttachedBehaviours
{
    // TODO: Handle data type filtering as lists
    // TODO: Create an adorner while moving
    // TODO: Create an adorner at the drop positon on hovering
    // TODO: Detect target position based on panel orientation instead of hardcoded Y axis
    public class DropBehavior : Behavior<ItemsControl>
    {
        #region Dependency Properties
        // callback prototype: void callback(object[] param) // param[0]: dropID, param[1]: Item
        public static readonly DependencyProperty DropCallbackProperty =
            DependencyProperty.Register("DropCallback", typeof(ICommand), typeof(DropBehavior));

        public static readonly DependencyProperty WhiteListProperty =
            DependencyProperty.Register("WhiteList", typeof(Type), typeof(DropBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty BlackListProperty =
            DependencyProperty.Register("BlackList", typeof(Type), typeof(DropBehavior), new PropertyMetadata(null));
        #endregion
        
        #region Public Properties
        public ICommand DropCallback
        {
            get { return (ICommand)GetValue(DropCallbackProperty); }
            set
            {
                SetValue(DropCallbackProperty, value);
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
        #endregion

        #region Private Properties
        private int _dropIndex;

        private bool IsItemsSourceBound
        {
            get
            {
                //return this.AssociatedObject.GetBindingExpression(ItemsControl.ItemsSourceProperty) != null;
                return AssociatedObject.ItemsSource != null;
            }
        }
        #endregion

        #region Init
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AllowDrop = true;
            AssociatedObject.DragEnter += OnEnter;
            AssociatedObject.DragLeave += OnLeave;
            AssociatedObject.DragOver += OnHover;
            AssociatedObject.Drop += OnDrop;
        }
        #endregion

        #region Events
        private void OnEnter(object sender, DragEventArgs e)
        {
            // TODO: Create drop adorner if valid drop target
        }

        private void OnLeave(object sender, DragEventArgs e)
        {
            // TODO: Destroy drop adorner
        }

        private void OnHover(object sender, DragEventArgs e)
        {
            e.Handled = true;
            e.Effects = DragDropEffects.None;

            if (IsValidDropTarget(e))
                e.Effects = DragDropEffects.Move;
            else
                return;

            // Get drop index
            _dropIndex = AssociatedObject.Items.Count;
            Point mousePosition = e.GetPosition(AssociatedObject);

            // Get hovered container
            var target = AssociatedObject.GetTargetContainer(mousePosition);
            if (target == null) return;

            int targetIndex = AssociatedObject.ItemContainerGenerator.IndexFromContainer(target);

            // Check if item should be dropped above or below current hoverred element
            bool isAbove = target.IsAboveElement(e.GetPosition(target));
            _dropIndex = isAbove ? targetIndex : targetIndex + 1;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            e.Handled = true;

            // Check if drop target is invalid
            if (e.Effects == DragDropEffects.None)
                return;

            // Get dragged data
            var item = e.Data.GetData(e.Data.GetFormats()[0]);

            if (IsItemsSourceBound)
            {
                if (DropCallback != null)
                {
                    object parameters = new[] { _dropIndex, item, AssociatedObject.DataContext };

                    DropCallback.Execute(parameters);
                }
            }
            else
            {
                AssociatedObject.Items.Insert(_dropIndex, item);
            }

            var container = AssociatedObject.ItemContainerGenerator.ContainerFromIndex(_dropIndex) as UIElement;
            if (container != null)
            {
                var dropPosition = e.GetPosition(AssociatedObject);
                Canvas.SetLeft(container, dropPosition.X);
                Canvas.SetTop(container, dropPosition.Y);
            }
        }
        #endregion

        #region Drop Logic
        private bool IsValidDropTarget(DragEventArgs e)
        {
            Debug.Assert(DropCallback != null, "DropCallback is null !");

            // If Data bound but no handler disable drop
            if (IsItemsSourceBound && DropCallback == null)
            {
                return false;
            }

            // If Whitelist active
            if (WhiteList != null)
            {
                if (!e.Data.GetDataPresent(WhiteList))
                    return false;
            }
            //If no Whitelist but Blacklist
            else if (BlackList != null)
            {
                if (e.Data.GetDataPresent(BlackList))
                    return false;
            }
            
            return true;
        }
        #endregion
    }
}
