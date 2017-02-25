using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RayTwol
{
    /// <summary>
    /// Interaction logic for EditBytes.xaml
    /// </summary>
    public partial class EditBytes : Window
    {
        Event e;
        int bWidth = 30;
        int bHeight = 20;
        int bPadX = 6;
        int bPadY = 2;

        public EditBytes(Event e)
        {
            InitializeComponent();
            this.e = e;

            for (int y = 0; y < 7; y++)
                for (int x = 0; x < 16; x++)
                {
                    byte disp = e.bytes[x + (y * 16)];

                    var label = new TextBlock();

                    label.Text = disp.ToString("X2");

                    label.Margin = new Thickness(bPadX + x * bWidth, bPadY + y * bHeight, bPadX + x * bWidth + bWidth, bPadY + y * bHeight + bHeight);

                    label.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 162, 0));

                    label.FontSize = 14;
                    canvas_bytes.Children.Add(label);
                }
        }

        private void button_cancel_click(object sender, RoutedEventArgs e)
        {

        }

        private void button_savenew_click(object sender, RoutedEventArgs e)
        {

        }

        private void button_apply_click(object sender, RoutedEventArgs e)
        {

        }
    }
}
