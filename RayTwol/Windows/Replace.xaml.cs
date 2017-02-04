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
    /// Interaction logic for Replace.xaml
    /// </summary>
    public partial class Replace : Window
    {
        public Replace()
        {
            InitializeComponent();
        }

        void button_type_solid_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_solid);
        }
        void button_type_passthrough_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_passthrough);
        }
        void button_type_hill_steep_left_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_hill_steep_left);
        }
        void button_type_hill_steep_right_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_hill_steep_right);
        }
        void button_type_hill_slight_left_1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_hill_slight_left_1);
        }
        void button_type_hill_slight_left_2_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_hill_slight_left_2);
        }
        void button_type_hill_slight_right_2_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_hill_slight_right_2);
        }
        void button_type_hill_slight_right_1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_passthrough);
        }
        void button_type_slippery_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_slippery);
        }
        void button_type_slippery_steep_left_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_slippery_steep_left);
        }
        void button_type_slippery_steep_right_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_slippery_steep_right);
        }
        void button_type_slippery_slight_left_1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_slippery_slight_left_1);
        }
        void button_type_slippery_slight_left_2_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_slippery_slight_left_2);
        }
        void button_type_slippery_slight_right_2_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_slippery_slight_right_2);
        }
        void button_type_slippery_slight_right_1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_slippery_slight_right_1);
        }
        void button_type_bounce_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_bounce);
        }
        void button_type_climb_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_climb);
        }
        void button_type_damage_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_damage);
        }
        void button_type_spikes_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_spikes);
        }
        void button_type_water_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_water);
        }
        void button_type_cliff_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_cliff);
        }
        void button_type_event_Click(object sender, RoutedEventArgs e)
        {
            textbox_type1.Text = Enum.GetName(typeof(Collisions), Collisions.type_event);
        }



        void button_type_solid1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_solid);
        }
        void button_type_passthrough1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_passthrough);
        }
        void button_type_hill_steep_left1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_hill_steep_left);
        }
        void button_type_hill_steep_right1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_hill_steep_right);
        }
        void button_type_hill_slight_left_11_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_hill_slight_left_1);
        }
        void button_type_hill_slight_left_21_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_hill_slight_left_2);
        }
        void button_type_hill_slight_right_21_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_hill_slight_right_2);
        }
        void button_type_hill_slight_right_11_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_passthrough);
        }
        void button_type_slippery1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_slippery);
        }
        void button_type_slippery_steep_left1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_slippery_steep_left);
        }
        void button_type_slippery_steep_right1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_slippery_steep_right);
        }
        void button_type_slippery_slight_left_11_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_slippery_slight_left_1);
        }
        void button_type_slippery_slight_left_21_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_slippery_slight_left_2);
        }
        void button_type_slippery_slight_right_21_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_slippery_slight_right_2);
        }
        void button_type_slippery_slight_right_11_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_slippery_slight_right_1);
        }
        void button_type_bounce1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_bounce);
        }
        void button_type_climb1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_climb);
        }
        void button_type_damage1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_damage);
        }
        void button_type_spikes1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_spikes);
        }
        void button_type_water1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_water);
        }
        void button_type_cliff1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_cliff);
        }
        void button_type_event1_Click(object sender, RoutedEventArgs e)
        {
            textbox_type2.Text = Enum.GetName(typeof(Collisions), Collisions.type_event);
        }



        void button_replaceall_click(object sender, RoutedEventArgs e)
        {
            Collisions coll1 = (Collisions)Enum.Parse(typeof(Collisions), textbox_type1.Text);
            Collisions coll2 = (Collisions)Enum.Parse(typeof(Collisions), textbox_type2.Text);
            //DialogResult = false;

            for (int i = 0; i < Level.types.Length; i++)
                if (Level.types[i].collision == coll1)
                {
                    Level.types[i].collision = coll2;
                    //DialogResult = true;
                }
                
            Close();
        }
    }
}
