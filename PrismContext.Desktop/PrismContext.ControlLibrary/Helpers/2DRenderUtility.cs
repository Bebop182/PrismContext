using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PrismContext.ControlLibrary.Helpers
{
    public static class ImageRenderUtility
    {
        public static RenderTargetBitmap GenerateBitMap(this FrameworkElement element)
        {
            double width = element.ActualWidth,
                height = element.ActualHeight;
            const int defaultDPI = 96;

            var bmpVisual = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), defaultDPI, defaultDPI, PixelFormats.Pbgra32);

            var drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                var vb = new VisualBrush(element);
                drawingContext.DrawRectangle(vb, null, new Rect(new Point(), new Size(width, height)));
            }
            bmpVisual.Render(drawingVisual);

            return bmpVisual;
        }

        public static void SaveAsPNG(this RenderTargetBitmap bitmap, string path)
        {
            var png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(bitmap));

            using (Stream stm = File.Create(path))
            {
                png.Save(stm);
            }
        }
    }
}
