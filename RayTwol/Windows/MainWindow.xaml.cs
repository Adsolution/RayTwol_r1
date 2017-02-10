using System.Windows;
using System.Windows.Controls;

namespace RayTwol
{
    public partial class MainWindow : Window
    {
        public MainWindow()
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
        }

        void button_openlevel_click(object sender, RoutedEventArgs e)
        {
            new OpenLevel().ShowDialog();
        }

        void objectsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            new OpenLevel().ShowDialog();
        }
    }
}
