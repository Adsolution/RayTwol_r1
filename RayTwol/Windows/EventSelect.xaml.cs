using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RayTwol
{
    /// <summary>
    /// Interaction logic for EventSelect.xaml
    /// </summary>
    public partial class EventSelect : Window
    {
        public Behaviours behaviour;
        public bool changed;

        public EventSelect()
        {
            InitializeComponent();

            foreach (Behaviours behaviour in Enum.GetValues(typeof(Behaviours)))
                eventsList.Items.Add(behaviour);
        }

        private void objectsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            behaviour = (Behaviours)eventsList.SelectedItem;
            changed = true;
            Close();
        }

        private void button_cancel_click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
