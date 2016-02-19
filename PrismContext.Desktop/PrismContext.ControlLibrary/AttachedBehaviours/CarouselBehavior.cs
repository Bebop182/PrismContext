using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace PrismContext.ControlLibrary.AttachedBehaviours
{
    public class CarouselBehavior : Behavior<ItemsControl>
    {
        private UIElement _targetContainer;
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseMove += OnMouseMove;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var target = GetTargetContainer(e.GetPosition(AssociatedObject));

            // Make sure target element has changed before updating zIndexes
            if (target == null || target.Equals(_targetContainer)) return;
            _targetContainer = target;

            int focusIndex = AssociatedObject.ItemContainerGenerator.IndexFromContainer(_targetContainer);

            for (int i = 0, nbItem = AssociatedObject.Items.Count; i < nbItem; i++)
            {
                int zIndex = nbItem - (i >= focusIndex ? i - focusIndex : focusIndex - i);

                var container = (UIElement)AssociatedObject.ItemContainerGenerator.ContainerFromIndex(i);
                container.SetValue(Panel.ZIndexProperty, zIndex);
            }
        }

        private UIElement GetTargetContainer(Point position)
        {
            var target = (UIElement)AssociatedObject.InputHitTest(position);
            while (target != null)
            {
                // Test if current target is container (ItemFromContainer returns UnsetValue instead of null if no item are associated with target element)
                var test = AssociatedObject.ItemContainerGenerator.ItemFromContainer(target);
                if (test != DependencyProperty.UnsetValue)
                    return target;
                // else go up in the tree
                target = VisualTreeHelper.GetParent(target) as UIElement;
            }
            return null;
        }
    }
}
