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
            if (objectsList.SelectedIndex > -1)
            {
                Event.DeselectAll();
                Editor.Scenes.Level.events[objectsList.SelectedIndex].selected = true;
                Editor.selectedEventsCount = 1;
                Editor.RefreshObjectInfo();
                RefreshEvents();
            }
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            new OpenLevel().ShowDialog();
        }





        void togglebutton_events_Click(object sender, RoutedEventArgs e)
        {
            Editor.editMode = EditMode.Events;
        }
        void togglebutton_graphics_Click(object sender, RoutedEventArgs e)
        {
            Editor.editMode = EditMode.Graphics;
        }
        void togglebutton_collision_Click(object sender, RoutedEventArgs e)
        {
            Editor.editMode = EditMode.Collision;
        }






        void textbox_ObjPosX_LostFocus(object sender, RoutedEventArgs e)
        {
            Event.SetTextboxPos();
        }
        void textbox_ObjPosY_LostFocus(object sender, RoutedEventArgs e)
        {
            Event.SetTextboxPos();
        }
        void textbox_ObjPosY_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Event.SetTextboxPos();
        }
        void textbox_ObjPosX_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Event.SetTextboxPos();
        }

        private void button_AddEvent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_EditEventBytes_click(object sender, RoutedEventArgs e)
        {
            if (Editor.selectedEvent != null)
                new EditBytes(Editor.selectedEvent).ShowDialog();
        }

        private void button_EditEventType_click(object sender, RoutedEventArgs e)
        {
            var eventSelect = new EventSelect();
            eventSelect.ShowDialog();
        }

        private void button_savenewtest(object sender, RoutedEventArgs e)
        {
            Editor.SaveLevelNew();
        }
    }
}
