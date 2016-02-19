using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PrismContext.ControlLibrary.CustomItemsControls
{
    public class OrganizationTree : TreeView
    {
        public interface IOrganizationTreeItem
        {
            void SetDrawProperties(Pen connectorPen, Orientation? orientation, bool linearConnectors, bool debug);
        }

        #region Dependency Properties
        public static readonly DependencyProperty ConnectorPenProperty = DependencyProperty.Register("ConnectorPen",
            typeof(Pen), typeof(OrganizationTree), new FrameworkPropertyMetadata(new Pen(Brushes.Black, 2), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LinearConnectorsProperty = DependencyProperty.Register("LinearConnectors",
            typeof(bool), typeof(OrganizationTree), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DebugProperty = DependencyProperty.Register("Debug",
            typeof(bool), typeof(OrganizationTree), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation?), typeof(OrganizationTree), new FrameworkPropertyMetadata(default(Orientation?), flags: FrameworkPropertyMetadataOptions.AffectsRender));

        #region DP accessors
        public Pen ConnectorPen
        {
            get { return (Pen)GetValue(ConnectorPenProperty); }
            set
            {
                SetValue(ConnectorPenProperty, value);
            }
        }

        public bool LinearConnectors
        {
            get { return (bool)GetValue(LinearConnectorsProperty); }
            set
            {
                SetValue(LinearConnectorsProperty, value);
            }
        }

        public bool Debug
        {
            get { return (bool)GetValue(DebugProperty); }
            set
            {
                SetValue(DebugProperty, value);
            }
        }

        public Orientation? Orientation
        {
            get
            {
                return (Orientation?)GetValue(OrientationProperty);
            }
            set
            {
                SetValue(OrientationProperty, value);
            }
        }
        #endregion
        #endregion

        #region ItemsControl Overrides
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new OrganizationTreeItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is OrganizationTreeItem;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            var container = element as IOrganizationTreeItem;
            if (container == null) return;
            container.SetDrawProperties(ConnectorPen, Orientation, LinearConnectors, Debug);
        }
        #endregion

        #region Constructor
        static OrganizationTree()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OrganizationTree), new FrameworkPropertyMetadata(typeof(OrganizationTree)));
        }
        #endregion
    }
}
