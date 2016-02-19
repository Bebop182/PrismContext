using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PrismContext.ControlLibrary.Adorners
{
    public class DragAdorner : Adorner
    {
        private RenderTargetBitmap _render;
        private AdornerLayer _layer;
        private Point _position;

        public DragAdorner(UIElement adornedElement, RenderTargetBitmap render) : base(adornedElement)
        {
            _render = render;
            _layer = AdornerLayer.GetAdornerLayer(AdornedElement);
            _position = new Point();

            IsHitTestVisible = false;

            _layer.Add(this);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var rect = new Rect(_position, new Size(_render.Width, _render.Height));
            var brush = new ImageBrush(_render);

            drawingContext.DrawRectangle(brush, null, rect);
        }

        public void UpdatePosition(double left, double top)
        {
            _position.X = left;
            _position.Y = top;

            _layer.Update(AdornedElement);
        }

        public void Destroy()
        {
            _layer.Remove(this);
        }
    }
}
