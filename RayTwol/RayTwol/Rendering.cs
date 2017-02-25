using System.Windows;
using System.Drawing;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Media;
using System.Linq;

namespace RayTwol
{
    public static class Rendering
    {
        public static byte[] buff;
        public static byte[] comp;
        public static WriteableBitmap display;
        public static System.Drawing.Color bgColour = System.Drawing.Color.FromArgb(21, 9, 18);
        
        public static void AddPixel(int x, int y, System.Drawing.Color colour)
        {
            if (colour.R == 0 && colour.G == 0 && colour.B == 0)
            {
                buff[(y * display.PixelWidth + x) * 3 + 0] = bgColour.R;
                buff[(y * display.PixelWidth + x) * 3 + 1] = bgColour.G;
                buff[(y * display.PixelWidth + x) * 3 + 2] = bgColour.B;
            }
            else
            {
                buff[(y * display.PixelWidth + x) * 3 + 0] = colour.R;
                buff[(y * display.PixelWidth + x) * 3 + 1] = colour.G;
                buff[(y * display.PixelWidth + x) * 3 + 2] = colour.B;
            }
        }

        public static void ClearBuffer()
        {
            for (int i = 0; i < display.PixelWidth * display.PixelHeight; i++)
            {
                buff[i * 3 + 0] = bgColour.R;
                buff[i * 3 + 1] = bgColour.G;
                buff[i * 3 + 2] = bgColour.B;
            }
        }

        public static void Render(bool overlay = false)
        {
            display.WritePixels(new Int32Rect(0, 0, display.PixelWidth, display.PixelHeight), buff, display.PixelWidth * 3, 0);
        }
    }






    public partial class MainWindow
    {
        public void RefreshEvents()
        {
            viewport_canvas.Children.Clear();
            viewport_canvas.Children.Add(Editor.selSquare);
            viewport_canvas.Children.Add(label_mgraphic);
            foreach (Event e in Editor.activeTypeGroup.events)
            {
                if (e.inLevel)
                {
                    var ev = new System.Windows.Shapes.Ellipse();
                    int size = 9;
                    ev.Width = size;
                    ev.Height = size;
                    ev.Margin = new Thickness(e.position.X - 4, e.position.Y - 4, 0, 0);

                    if (!e.selected)
                        ev.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 231, 148));
                    else
                        ev.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 100, 255));

                    ev.StrokeThickness = 2;

                    viewport_canvas.Children.Add(ev);


                    var lab = new System.Windows.Controls.TextBlock();
                    lab.Text = string.Format("{0}  {1}  {2}", e.ID.ToString("000"), e.type.ToString(), ((byte)e.type).ToString("X2"));
                    lab.Margin = new Thickness(e.position.X + 8, e.position.Y - 6, e.position.X + 100, e.position.Y + 15);
                    if (!e.selected)
                        lab.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 255, 231, 148));
                    else
                        lab.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 100, 255));
                    lab.FontSize = 9;
                    viewport_canvas.Children.Add(lab);
                }
            }
        }




        void UpdateViewport(object sender, EventArgs e)
        {
            Refresh();
        }
        
        void Refresh()
        {
            Refresh(0, 0, Editor.activeTypeGroup.width, Editor.activeTypeGroup.height);
        }
        void Refresh(bool force)
        {
            Refresh(0, 0, Editor.activeTypeGroup.width, Editor.activeTypeGroup.height, force);
        }
        void Refresh(int x1, int y1, int x2, int y2, bool force = false)
        {
            if (force)
            {
                Rendering.display = new WriteableBitmap(Editor.activeTypeGroup.width * 16, Editor.activeTypeGroup.height * 16, 96, 96, PixelFormats.Rgb24, null);
                Editor.types_screen = new Type[Editor.activeTypeGroup.types.Length];
            }
            
            // Types
            for (int y = y1; y < y2; y++)
                for (int x = x1; x < x2; x++)
                {
                    int t = x + (y * Editor.activeTypeGroup.width);
                    if (Editor.activeTypeGroup.types[t].graphic != Editor.types_screen[t].graphic || Editor.activeTypeGroup.types[t].collision != Editor.types_screen[t].collision)
                    {
                        for (int py = 0; py < 16; py++)
                            for (int px = 0; px < 16; px++)
                                Rendering.AddPixel((x * 16) + px, (y * 16) + py, Rendering.bgColour);

                        if (Editor.activeTypeGroup.types[t].graphic.X != 0 || Editor.activeTypeGroup.types[t].graphic.Y != 0)
                            for (int py = 0; py < 16; py++)
                                for (int px = 0; px < 16; px++)
                                    Rendering.AddPixel((x * 16) + px, (y * 16) + py, Editor.tileset.GetPixel(Editor.activeTypeGroup.types[t].graphic.X * 16 + px, Editor.activeTypeGroup.types[t].graphic.Y * 16 + py));

                        if (Editor.editMode == EditMode.Collision && Editor.activeTypeGroup.types[t].collision != Editor.types_screen[t].collision && Editor.activeTypeGroup.types[t].collision != Collisions.none)
                        {
                            string typeName = Enum.GetName(typeof(Collisions), Editor.activeTypeGroup.types[t].collision);
                            var bmp = new BitmapImage(new Uri("pack://application:,,,/gfx/types/" + typeName + ".png", UriKind.Absolute));
                            int stride = bmp.PixelWidth * 4;
                            byte[] pixels = new byte[bmp.PixelHeight * stride];
                            bmp.CopyPixels(pixels, stride, 0);

                            for (int py = 0; py < 16; py++)
                                for (int px = 0; px < 16; px++)
                                    if (pixels[((px * 8) + (py * stride * 2)) + 3] != 0)
                                        Rendering.AddPixel((x * 16) + px, (y * 16) + py, System.Drawing.Color.FromArgb(pixels[((px * 8) + (py * stride * 2)) + 2], pixels[((px * 8) + (py * stride * 2)) + 1], pixels[((px * 8) + (py * stride * 2)) + 0]));
                        }
                    }
                }

            RefreshEvents();
            Editor.types_screen = Editor.activeTypeGroup.types.ToArray();

            Rendering.Render();
            viewport.Source = Rendering.display;
        }
    }
}
