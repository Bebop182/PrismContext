using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using PrismContext.ControlLibrary.Helpers;

namespace PrismContext.ControlLibrary.Adorners
{
    // Todo: Expose styling property for the line between anchors
    // Todo: Make the adorner extendable
    public class AutoAdorner : Adorner
    {
        #region Static Context
        #region Attached Properties
        public static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached("Adorner",
            typeof(AutoAdorner), typeof(AutoAdorner), new PropertyMetadata(null));

        public static readonly DependencyProperty TemplateProperty = DependencyProperty.RegisterAttached("Template",
            typeof(DataTemplate), typeof(AutoAdorner), new UIPropertyMetadata(null, Template_PropertyChangedCallback));

        public static readonly DependencyProperty AdornedAnchorPointProperty =
            DependencyProperty.RegisterAttached("AdornedAnchorPoint", typeof(AnchorPoints), typeof(AutoAdorner),
                new PropertyMetadata(AnchorPoints.TopLeft, AdornedAnchorPoint_PropertyChangedCallback));

        public static readonly DependencyProperty ContentAnchorPointProperty =
            DependencyProperty.RegisterAttached("ContentAnchorPoint", typeof(AnchorPoints), typeof(AutoAdorner),
                new PropertyMetadata(AnchorPoints.TopLeft, ContentAnchorPoint_PropertyChangedCallback));

        public static readonly DependencyProperty ContentAnchorOffsetXProperty =
            DependencyProperty.RegisterAttached("ContentAnchorOffsetX", typeof(int), typeof(AutoAdorner),
                new PropertyMetadata(0, ContentAnchorOffsetX_PropertyChangedCallback));

        public static readonly DependencyProperty ContentAnchorOffsetYProperty =
            DependencyProperty.RegisterAttached("ContentAnchorOffsetY", typeof(int), typeof(AutoAdorner),
                new PropertyMetadata(0, ContentAnchorOffsetY_PropertyChangedCallback));

        public static readonly DependencyProperty ContentOffsetXProperty =
            DependencyProperty.RegisterAttached("ContentOffsetX", typeof(int), typeof(AutoAdorner),
                new PropertyMetadata(0, ContentOffsetX_PropertyChangedCallback));

        public static readonly DependencyProperty ContentOffsetYProperty =
            DependencyProperty.RegisterAttached("ContentOffsetY", typeof(int), typeof(AutoAdorner),
                new PropertyMetadata(0, ContentOffsetY_PropertyChangedCallback));

        public static readonly DependencyProperty IsContentVisibleProperty =
            DependencyProperty.RegisterAttached("IsContentVisible", typeof(bool), typeof(AutoAdorner),
                new PropertyMetadata(false, IsContentVisible_PropertyChangedCallback));

        public static readonly DependencyProperty UseTopMostLayerProperty =
            DependencyProperty.RegisterAttached("UseTopMostLayer", typeof(bool), typeof(AutoAdorner),
                new PropertyMetadata(true, UseTopMostLayer_PropertyChangedCallback));

        public static readonly DependencyProperty ShowAnchorLinkProperty =
            DependencyProperty.RegisterAttached("ShowAnchorLink", typeof(bool), typeof(AutoAdorner),
                new PropertyMetadata(false, ShowAnchorLink_PropertyChangedCallback));

        public static readonly DependencyProperty AutoAdornerPresetProperty =
            DependencyProperty.RegisterAttached("AutoAdornerPreset", typeof(AutoAdornerPreset), typeof(AutoAdorner),
                new PropertyMetadata(default(AutoAdornerPreset), AutoAdornerPreset_PropertyChangedCallback));
        #endregion

        #region Properties Accessors
        // Adorner property accessors
        protected static AutoAdorner GetAdorner(DependencyObject o)
        {
            return (AutoAdorner)o.GetValue(AdornerProperty);
        }
        protected static void SetAdorner(DependencyObject o, AutoAdorner value)
        {
            o.SetValue(AdornerProperty, value);
        }

        // Template property accessors
        public static DataTemplate GetTemplate(DependencyObject o)
        {
            return (DataTemplate)o.GetValue(TemplateProperty);
        }
        public static void SetTemplate(DependencyObject o, DataTemplate value)
        {
            o.SetValue(TemplateProperty, value);
        }

        // AdornedAnchorPoint property accessors
        public static AnchorPoints GetAdornedAnchorPoint(DependencyObject o)
        {
            return (AnchorPoints)o.GetValue(AdornedAnchorPointProperty);
        }
        public static void SetAdornedAnchorPoint(DependencyObject o, AnchorPoints value)
        {
            o.SetValue(AdornedAnchorPointProperty, value);
        }

        // ContentAnchorPoint property accessors
        public static AnchorPoints GetContentAnchorPoint(DependencyObject o)
        {
            return (AnchorPoints)o.GetValue(ContentAnchorPointProperty);
        }
        public static void SetContentAnchorPoint(DependencyObject o, AnchorPoints value)
        {
            o.SetValue(ContentAnchorPointProperty, value);
        }

        // ContentAnchorOffsetX property accessors
        public static int GetContentAnchorOffsetX(DependencyObject o)
        {
            return (int)o.GetValue(ContentAnchorOffsetXProperty);
        }
        public static void SetContentAnchorOffsetX(DependencyObject o, int value)
        {
            o.SetValue(ContentAnchorOffsetXProperty, value);
        }

        // ContentAnchorOffsetY property accessors
        public static int GetContentAnchorOffsetY(DependencyObject o)
        {
            return (int)o.GetValue(ContentAnchorOffsetYProperty);
        }
        public static void SetContentAnchorOffsetY(DependencyObject o, int value)
        {
            o.SetValue(ContentAnchorOffsetYProperty, value);
        }

        // ContentOffsetX property accessors
        public static int GetContentOffsetX(DependencyObject o)
        {
            return (int)o.GetValue(ContentOffsetXProperty);
        }
        public static void SetContentOffsetX(DependencyObject o, int value)
        {
            o.SetValue(ContentOffsetXProperty, value);
        }

        // ContentOffsetY property accessors
        public static int GetContentOffsetY(DependencyObject o)
        {
            return (int)o.GetValue(ContentOffsetYProperty);
        }
        public static void SetContentOffsetY(DependencyObject o, int value)
        {
            o.SetValue(ContentOffsetYProperty, value);
        }

        // IsContentVisible property accessors
        public static bool GetIsContentVisible(DependencyObject o)
        {
            return (bool)o.GetValue(IsContentVisibleProperty);
        }
        public static void SetIsContentVisible(DependencyObject o, bool value)
        {
            o.SetValue(IsContentVisibleProperty, value);
        }

        // IsContentVisible property accessors
        public static bool GetUseTopMostLayer(DependencyObject o)
        {
            return (bool)o.GetValue(UseTopMostLayerProperty);
        }
        public static void SetUseTopMostLayer(DependencyObject o, bool value)
        {
            o.SetValue(UseTopMostLayerProperty, value);
        }

        // ShowAnchorLink property accessors
        public static bool GetShowAnchorLink(DependencyObject o)
        {
            return (bool)o.GetValue(ShowAnchorLinkProperty);
        }
        public static void SetShowAnchorLink(DependencyObject o, bool value)
        {
            o.SetValue(ShowAnchorLinkProperty, value);
        }

        // ShowAnchorLink property accessors
        public static AutoAdornerPreset GetAutoAdornerPreset(DependencyObject o)
        {
            return (AutoAdornerPreset)o.GetValue(AutoAdornerPresetProperty);
        }
        public static void SetAutoAdornerPreset(DependencyObject o, AutoAdornerPreset value)
        {
            o.SetValue(AutoAdornerPresetProperty, value);
        }
        #endregion

        #region Properties Callbacks
        private static void Template_PropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var adorner = GetAdorner(o);
            if (adorner == null) return;
            adorner.ContentTemplate = (DataTemplate)e.NewValue;
        }

        private static void AdornedAnchorPoint_PropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var adorner = GetAdorner(o);
            if (adorner == null) return;
            adorner.InvalidateVisual();
        }

        private static void ContentAnchorPoint_PropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var adorner = GetAdorner(o);
            if (adorner == null) return;
            adorner.InvalidateVisual();
        }

        private static void ContentAnchorOffsetX_PropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var adorner = GetAdorner(o);
            if (adorner == null) return;
            adorner.InvalidateVisual();
        }

        private static void ContentAnchorOffsetY_PropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var adorner = GetAdorner(o);
            if (adorner == null) return;
            adorner.InvalidateVisual();
        }

        private static void ContentOffsetX_PropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var adorner = GetAdorner(o);
            if (adorner == null) return;
            adorner.InvalidateVisual();
        }

        private static void ContentOffsetY_PropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var adorner = GetAdorner(o);
            if (adorner == null) return;
            adorner.InvalidateVisual();
        }

        private static void UseTopMostLayer_PropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void ShowAnchorLink_PropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var adorner = GetAdorner(o);
            if (adorner == null) return;
            adorner.InvalidateVisual();
        }

        private static void IsContentVisible_PropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var adornedElement = o as FrameworkElement;
            if (adornedElement == null) throw new InvalidOperationException("The adorned element must be a FrameworkElement");

            if (adornedElement.IsLoaded == false)
            {
                adornedElement.Loaded += ShowAdorner;
                return;
            }
            ShowAdorner(adornedElement, null);
        }

        private static void AutoAdornerPreset_PropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var preset = (AutoAdornerPreset)e.NewValue;
            SetTemplate(o, preset.Template);
            SetAdornedAnchorPoint(o, preset.AdornedAnchorPoint);
            SetContentAnchorPoint(o, preset.ContentAnchorPoint);
            SetContentAnchorOffsetX(o, preset.ContentAnchorOffsetX);
            SetContentAnchorOffsetY(o, preset.ContentAnchorOffsetY);
            SetContentOffsetX(o, preset.ContentOffsetX);
            SetContentOffsetY(o, preset.ContentOffsetY);
            SetUseTopMostLayer(o, preset.UseTopMostLayer);
            SetShowAnchorLink(o, preset.ShowAnchorLink);
        }
        #endregion
        #endregion

        #region Local Context
        #region Protected Attributes
        protected readonly ContentPresenter ContentPresenter;
        protected Point AdornedAnchorPosition;
        protected Point AdornerPosition;
        #endregion

        #region Public Properties
        protected DataTemplate ContentTemplate
        {
            get { return ContentPresenter.ContentTemplate; }
            set { ContentPresenter.ContentTemplate = value; }
        }
        #endregion

        #region Constructors
        protected AutoAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            ContentPresenter = new ContentPresenter();
            var dataContextBinding = new Binding("DataContext") { Source = adornedElement };
            BindingOperations.SetBinding(ContentPresenter, ContentPresenter.ContentProperty, dataContextBinding);
            ContentTemplate = GetTemplate(adornedElement);
            AddVisualChild(ContentPresenter);
            AddLogicalChild(ContentPresenter);
        }
        #endregion

        #region Layout & Rendering
        protected override Size MeasureOverride(Size constraint)
        {
            ContentPresenter.Measure(constraint);
            return ContentPresenter.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var placement = new Rect(AdornerPosition, finalSize);
            ContentPresenter.Arrange(placement);

            return ContentPresenter.RenderSize;
        }
        
        private static Point GetElementAnchorPosition(UIElement element, AnchorPoints anchorPoint, Point origin = default(Point))
        {
            switch (anchorPoint)
            {
                case AnchorPoints.Left:
                    origin.Y += (int)(element.RenderSize.Height / 2);
                    break;
                case AnchorPoints.Top:
                    origin.X += (int)(element.RenderSize.Width / 2);
                    break;
                case AnchorPoints.TopRight:
                    origin.X += (int)(element.RenderSize.Width);
                    break;
                case AnchorPoints.Right:
                    origin.X += (int)(element.RenderSize.Width);
                    origin.Y += (int)(element.RenderSize.Height / 2);
                    break;
                case AnchorPoints.BottomRight:
                    origin.X += (int)(element.RenderSize.Width);
                    origin.Y += (int)(element.RenderSize.Height);
                    break;
                case AnchorPoints.Bottom:
                    origin.X += (int)(element.RenderSize.Width / 2);
                    origin.Y += (int)(element.RenderSize.Height);
                    break;
                case AnchorPoints.BottomLeft:
                    origin.Y += (int)(element.RenderSize.Height);
                    break;
            }
            return origin;
        }

        protected void ProcessPosition()
        {
            // Get AdornedAnchor setting then get its position
            var anchorPoint = GetAdornedAnchorPoint(AdornedElement);
            AdornedAnchorPosition = GetElementAnchorPosition(AdornedElement, anchorPoint);

            // Get adorner position starting from AdornedAnchor position
            AdornerPosition = AdornedAnchorPosition + new Vector(GetContentOffsetX(AdornedElement), GetContentOffsetY(AdornedElement));
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0)
                return ContentPresenter;
            throw new IndexOutOfRangeException();
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
        
        protected override void OnRender(DrawingContext drawingContext)
        {
            ProcessPosition();
            if (!GetShowAnchorLink(AdornedElement)) return;
            var contentAnchorPosition = GetElementAnchorPosition(ContentPresenter, GetContentAnchorPoint(AdornedElement), AdornerPosition) + new Vector(GetContentAnchorOffsetX(AdornedElement), GetContentAnchorOffsetY(AdornedElement));
            drawingContext.DrawLine(new Pen(Brushes.Black, 2), contentAnchorPosition, AdornedAnchorPosition);
        }
        #endregion

        private static void ShowAdorner(object sender, RoutedEventArgs routedEventArgs)
        {
            var adornedElement = sender as FrameworkElement;
            if (adornedElement == null) return;

            var layer = GetUseTopMostLayer(adornedElement) ? adornedElement.GetTopMostAdornerLayer() : AdornerLayer.GetAdornerLayer(adornedElement);
            if (layer == null) throw new InvalidOperationException("No AdornerLayer found");

            var adorner = GetAdorner(adornedElement);
            if (GetIsContentVisible(adornedElement))
            {
                if (adorner == null)
                    adorner = new AutoAdorner(adornedElement);
                SetAdorner(adornedElement, adorner);
                layer.Add(adorner);
                adorner.ProcessPosition();
            }
            else if (adorner != null)
            {
                layer.Remove(adorner);
                SetAdorner(adornedElement, null);
            }
        }
        #endregion
    }

    public struct AutoAdornerPreset
    {
        public DataTemplate Template { get; set; }
        public AnchorPoints AdornedAnchorPoint { get; set; }
        public AnchorPoints ContentAnchorPoint { get; set; }
        public int ContentAnchorOffsetX { get; set; }
        public int ContentAnchorOffsetY { get; set; }
        public int ContentOffsetX { get; set; }
        public int ContentOffsetY { get; set; }
        public bool UseTopMostLayer { get; set; }
        public bool ShowAnchorLink { get; set; }
    }

    public enum AnchorPoints
    {
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft,
        Left,
        Top,
        Right,
        Bottom
    }
}
