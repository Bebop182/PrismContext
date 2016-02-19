using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace PrismContext.ControlLibrary.Helpers
{
    internal static class UIHelper
    {
        #region ItemsControl
        public static UIElement GetTargetContainer(this ItemsControl element, Point position)
        {
            var target = (UIElement)element.InputHitTest(position);
            while (target != null)
            {
                // Test if current target is container (ItemFromContainer returns UnsetValue instead of null if no item are associated with target element)
                var test = element.ItemContainerGenerator.ItemFromContainer(target);
                if (test != DependencyProperty.UnsetValue)
                    return target;
                // else go up in the tree
                target = VisualTreeHelper.GetParent(target) as UIElement;
            }
            return null;
        }

        public static bool IsAboveElement(this UIElement targetElement, Point relativePosition)
        {
            // 0,0 is top left
            if (relativePosition.Y < (targetElement.RenderSize.Height / 2))
            {
                // Above
                return true;
            }
            return false;
        }
        #endregion

        #region Adorner
        public static AdornerLayer GetTopMostAdornerLayer(this UIElement element)
        {
            var window = GetTopMostElement(element);
            if (window == null) throw new InvalidOperationException("No parent window found !");

            var decorator = GetAdornerDecorator(window);
            return decorator != null ? decorator.AdornerLayer : null;
        }

        public static DependencyObject GetTopMostElement(DependencyObject element)
        {
            DependencyObject topMostElement = Window.GetWindow(element);
            if (topMostElement != null) return topMostElement;

            while (element != null)
            {
                topMostElement = element;
                element = VisualTreeHelper.GetParent(element);
            }
            return topMostElement;
        }

        public static AdornerDecorator GetAdornerDecorator(this DependencyObject obj)
        {
            var children = new Queue<DependencyObject>();
            children.Enqueue(obj);
            while (children.Count > 0)
            {
                var node = children.Dequeue();
                if (node is AdornerDecorator)
                    return node as AdornerDecorator;
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(node); i++)
                {
                    var child = VisualTreeHelper.GetChild(node, i);
                    if (child is FrameworkElement)
                        children.Enqueue(child);
                }
            }
            return null;
        }
        #endregion

        #region 2D Drawing
        public static Point GetPointFromLine(Point a, Point b, double dPercent)
        {
            dPercent = Clamp01(dPercent);
            return new Point
            {
                X = (int)(a.X + (b.X - a.X) * dPercent),
                Y = (int)(a.Y + (b.Y - a.Y) * dPercent)
            };
        }

        public static double GetDistance(Point a, Point b)
        {
            double ac = b.X - a.X;
            double bc = b.Y - a.Y;
            return Math.Sqrt(ac * ac + bc * bc);
        }

        public static double GetDistanceOnAxis(this Point a, Point b, string axis = "X")
        {
            return Math.Abs(GetDirectionOnAxis(a, b, axis));
        }

        public static double GetDirectionOnAxis(this Point a, Point b, string axis = "X")
        {
            double direction = 0d;
            switch (axis)
            {
                case "y":
                case "Y":
                    direction = b.Y - a.Y;
                    break;
                // case X
                default: 
                    direction = b.X - a.X;
                    break;
            }
            return direction;
        }

        public static double Clamp01(double value)
        {
            return Clamp(value, 0f, 1f);
        }

        public static T Clamp<T>(T value, T min, T max) where T : IComparable
        {
            value = value.CompareTo(min) <= 0 ? min : value;
            value = value.CompareTo(max) >= 1 ? max : value;
            return value;
        }

        public static double DegToRad(this double deg)
        {
            return deg*Math.PI/180d;
        }
        #endregion
    }
}
