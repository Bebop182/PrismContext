using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PrismContext.ControlLibrary.Helpers;

namespace PrismContext.ControlLibrary.CustomItemsControls
{
    public class OrganizationTreeItem : TreeViewItem, OrganizationTree.IOrganizationTreeItem
    {
        #region Constructor
        static OrganizationTreeItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OrganizationTreeItem), new FrameworkPropertyMetadata(typeof(OrganizationTreeItem)));
        }
        #endregion

        #region ItemsControl Overrides
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var container = element as OrganizationTree.IOrganizationTreeItem;
            if (container == null) return;
            container.SetDrawProperties(ConnectorPen, Orientation, LinearConnectors, Debug);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is OrganizationTreeItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new OrganizationTreeItem();
        }
        #endregion

        #region Private Properties
        private Pen ConnectorPen { get; set; }

        private Orientation? Orientation { get; set; }

        private bool LinearConnectors { get; set; }

        private bool Debug { get; set; }

        private bool Inverted { get; set; }
        #endregion

        #region Runtime Events
        protected override void OnRender(DrawingContext drawingContext)
        {
            DrawTreeConnectors(drawingContext);
            base.OnRender(drawingContext);

        }
        #endregion

        #region IOrganizationTreeItem
        public void SetDrawProperties(Pen connectorPen, Orientation? orientation, bool linearConnectors, bool debug)
        {
            ConnectorPen = connectorPen;
            Orientation = orientation;
            LinearConnectors = linearConnectors;
            Debug = debug;
        }
        #endregion

        #region ConnectorDrawing Methods
        private void DrawTreeConnectors(DrawingContext drawingContext)
        {
            var children = GetChildrenOfParentType(this);
            foreach (FrameworkElement child in children)
            {
                CheckLayout(this, child);
                Point a = GetParentAnchor(this);
                Point b = GetChildAnchor(child);
                DrawConnector(drawingContext, a, b);
            }
        }

        private void CheckOrientation(Point a, Point b)
        {
            if (Orientation != null) return;

            var x = UIHelper.GetDistanceOnAxis(a, b, "X");
            var y = UIHelper.GetDistanceOnAxis(a, b, "Y");

            Orientation = x >= y ? System.Windows.Controls.Orientation.Horizontal : System.Windows.Controls.Orientation.Vertical;
        }

        private void CheckInverted(Point a, Point b)
        {
            Inverted = Orientation == System.Windows.Controls.Orientation.Vertical ? a.Y - b.Y >= 0 : a.X - b.X >= 0;
        }

        private void CheckLayout(FrameworkElement elementA, FrameworkElement elementB)
        {
            var parentHeader = GetHeader(elementA);
            var childHeader = GetHeader(elementB);
            var a = parentHeader.TranslatePoint(parentHeader.RenderTransformOrigin, this);
            var b = childHeader.TranslatePoint(childHeader.RenderTransformOrigin, this);

            CheckOrientation(a, b);
            CheckInverted(a, b);
        }

        private IEnumerable<FrameworkElement> GetChildrenOfParentType(DependencyObject parent)
        {
            var parentType = parent.GetType();
            List<FrameworkElement> childrenOfType = GetChildrenOfType(parentType, parent).ToList();
            return childrenOfType;
        }

        private IEnumerable<FrameworkElement> GetChildrenOfType(Type type, DependencyObject parent)
        {
            var children = new List<FrameworkElement>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (child != null && child.GetType() == type)
                {
                    children.Add(child);
                }
                else
                {
                    children.AddRange(GetChildrenOfType(type, child));
                }
            }
            return children;
        }

        private Point GetParentAnchor(FrameworkElement parent)
        {
            var header = GetHeader(parent);
            double x, y;
            if (Orientation == System.Windows.Controls.Orientation.Vertical)
            {
                x = header.Width / 2f;
                y = Inverted ? 0f : header.Height;
            }
            else
            {
                x = Inverted ? 0f : header.Width;
                y = header.Height / 2f;
            }

            return header.TranslatePoint(new Point(x, y), this);
        }

        private Point GetChildAnchor(FrameworkElement child)
        {
            var header = GetHeader(child);
            double x, y;
            if (Orientation == System.Windows.Controls.Orientation.Vertical)
            {
                x = header.Width / 2f;
                y = Inverted ? header.Height : 0f;
            }
            else
            {
                x = Inverted ? header.Width : 0f;
                y = header.Height / 2f;
            }

            return header.TranslatePoint(new Point(x, y), this);
        }

        private FrameworkElement GetHeader(FrameworkElement element)
        {
            var firstChild = VisualTreeHelper.GetChild(element, 0) as FrameworkElement;
            if (firstChild == null)
                throw new InvalidOperationException("Provided ItemsContainer is missing a PART_Header in its template");

            return firstChild.FindName("PART_Header") as FrameworkElement;
        }

        private void DrawConnector(DrawingContext drawingContext, Point a, Point b)
        {
            // If linear draw and return
            if (LinearConnectors)
            {
                drawingContext.DrawLine(ConnectorPen, a, b);
                return;
            }

            // Define control points origin
            Point
                ctrlP1,
                ctrlP2;

            // Apply control point distance relative to line direction
            if (Orientation == System.Windows.Controls.Orientation.Horizontal)
            {
                ctrlP1 = new Point
                {
                    X = b.X,
                    Y = a.Y
                };
                ctrlP2 = new Point
                {
                    X = a.X,
                    Y = b.Y
                };
            }
            else
            {
                ctrlP1 = new Point
                {
                    X = a.X,
                    Y = b.Y
                };
                ctrlP2 = new Point
                {
                    X = b.X,
                    Y = a.Y
                };
            }

            if (Debug)
            {
                drawingContext.DrawEllipse(Brushes.YellowGreen, new Pen(), a, 5d, 5d);
                drawingContext.DrawEllipse(Brushes.DarkGreen, new Pen(), b, 5d, 5d);
                drawingContext.DrawEllipse(Brushes.Red, new Pen(), ctrlP1, 2d, 2d);
                drawingContext.DrawEllipse(Brushes.Blue, new Pen(), ctrlP2, 2d, 2d);
            }

            // Draw the bezier curve
            var pathString = String.Format("M {0},{1} C {4},{5} {6},{7} {2},{3}", (int)a.X, (int)a.Y, (int)b.X, (int)b.Y, (int)ctrlP1.X, (int)ctrlP1.Y, (int)ctrlP2.X, (int)ctrlP2.Y);
            drawingContext.DrawGeometry(null, ConnectorPen, Geometry.Parse(pathString));
        }
        #endregion
    }
}
