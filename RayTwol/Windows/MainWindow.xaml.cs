using System;
using System.Diagnostics;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Forms.Integration;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Drawing;
using System.Windows.Shapes;
using System.Linq;
using System.Windows.Input;

namespace RayTwol
{
    public partial class MainWindow : Window
    {
        public MainWindow(string lvl)
        {
            InitializeComponent();
            Editor.UpdateViewport += UpdateViewport;
            KeyDown += KeyPressed;
            KeyUp += KeyReleased;

            Editor.brush_select.Color = System.Windows.Media.Color.FromArgb(200, 255, 200, 0);
            Editor.brush_create.Color = System.Windows.Media.Color.FromArgb(200, 0, 255, 0);
            Editor.brush_delete.Color = System.Windows.Media.Color.FromArgb(200, 255, 0, 0);
            Editor.brush_hidden.Color = System.Windows.Media.Color.FromArgb(0, 0, 0, 0);
            Editor.selSquare.StrokeThickness = 3;
            Editor.selSquare.Stroke = Editor.brush_hidden;
            viewport_canvas.Children.Add(Editor.selSquare);

            Editor.OpenLevel(lvl);
        }
    }
}
