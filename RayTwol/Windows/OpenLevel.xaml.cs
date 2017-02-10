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
    /// Interaction logic for OpenLevel.xaml
    /// </summary>
    public partial class OpenLevel : Window
    {
        public OpenLevel()
        {
            InitializeComponent();

            list_world.Items.Add("The Dream Forest");
            list_world.Items.Add("Band Land");
            list_world.Items.Add("Blue Mountains");
            list_world.Items.Add("Picture City");
            list_world.Items.Add("The Caves of Skops");
            list_world.Items.Add("Candy Château");
        }

        void button_cancel_click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void button_open_click(object sender, RoutedEventArgs e)
        {
            Close();
            Editor.OpenLevel(textbox_levelname.Text);
        }

        void list_world_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            list_level.Items.Clear();
            switch (list_world.SelectedIndex)
            {
                case 0:
                    list_level.Items.Add("Pink Plant Woods");
                    list_level.Items.Add("Anguish Lagoon");
                    list_level.Items.Add("The Swamps of Forgetfulness");
                    list_level.Items.Add("Moskito's Nest");
                    list_level.Items.Add("--- Bonus");
                    break;
                case 1:
                    list_level.Items.Add("Bongo Hills");
                    list_level.Items.Add("Allegro Presto");
                    list_level.Items.Add("Gong Heights");
                    list_level.Items.Add("Mr. Sax's Hulaballoo");
                    list_level.Items.Add("--- Bonus");
                    break;
                case 2:
                    list_level.Items.Add("Twilight Glutch");
                    list_level.Items.Add("The Hard Rocks");
                    list_level.Items.Add("Mr. Stone's Peaks");
                    list_level.Items.Add("--- Bonus");
                    break;
                case 3:
                    list_level.Items.Add("Eraser Plains");
                    list_level.Items.Add("Pencil Pentathlon");
                    list_level.Items.Add("Space Mama's Crater");
                    list_level.Items.Add("--- Bonus");
                    break;
                case 4:
                    list_level.Items.Add("Crystal Palace");
                    list_level.Items.Add("Eat At Joe's");
                    list_level.Items.Add("Mr. Skops' Stalectites");
                    list_level.Items.Add("--- Bonus");
                    break;
                case 5:
                    list_level.Items.Add("Mr. Dark's Dare");
                    break;
            }
        }

        void list_level_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            list_area.Items.Clear();
            switch (list_world.SelectedIndex)
            {
                case 0:
                    switch (list_level.SelectedIndex)
                    {
                        case 0:
                            list_area.Items.Add("JUN01");
                            list_area.Items.Add("JUN02");
                            list_area.Items.Add("JUN03");
                            list_area.Items.Add("JUN04");
                            break;
                        case 1:
                            list_area.Items.Add("JUN05");
                            list_area.Items.Add("JUN06");
                            list_area.Items.Add("JUN07");
                            list_area.Items.Add("JUN08");
                            break;
                        case 2:
                            list_area.Items.Add("JUN09");
                            list_area.Items.Add("JUN10");
                            list_area.Items.Add("JUN11");
                            break;
                        case 3:
                            list_area.Items.Add("JUN12");
                            list_area.Items.Add("JUN13");
                            list_area.Items.Add("JUN14");
                            list_area.Items.Add("JUN15");
                            list_area.Items.Add("JUN16");
                            list_area.Items.Add("JUN17");
                            break;
                        case 4:
                            list_area.Items.Add("JUN18");
                            list_area.Items.Add("JUN19");
                            list_area.Items.Add("JUN20");
                            list_area.Items.Add("JUN21");
                            break;
                    }
                    break;
                case 1:
                    switch (list_level.SelectedIndex)
                    {
                        case 0:
                            list_area.Items.Add("MUS01");
                            list_area.Items.Add("MUS02");
                            list_area.Items.Add("MUS03");
                            list_area.Items.Add("MUS04");
                            list_area.Items.Add("MUS05");
                            list_area.Items.Add("MUS06");
                            break;
                        case 1:
                            list_area.Items.Add("MUS07");
                            list_area.Items.Add("MUS08");
                            list_area.Items.Add("MUS09");
                            list_area.Items.Add("MUS10");
                            list_area.Items.Add("MUS11");
                            break;
                        case 2:
                            list_area.Items.Add("MUS12");
                            list_area.Items.Add("MUS13");
                            break;
                        case 3:
                            list_area.Items.Add("MUS14");
                            list_area.Items.Add("MUS15");
                            list_area.Items.Add("MUS16");
                            break;
                        case 4:
                            list_area.Items.Add("MUS17");
                            list_area.Items.Add("MUS18");
                            break;
                    }
                    break;
                case 2:
                    switch (list_level.SelectedIndex)
                    {
                        case 0:
                            list_area.Items.Add("MON01");
                            list_area.Items.Add("MON02");
                            break;
                        case 1:
                            list_area.Items.Add("MON03");
                            list_area.Items.Add("MON04");
                            list_area.Items.Add("MON05");
                            break;
                        case 2:
                            list_area.Items.Add("MON06");
                            list_area.Items.Add("MON07");
                            list_area.Items.Add("MON08");
                            list_area.Items.Add("MON09");
                            list_area.Items.Add("MON10");
                            list_area.Items.Add("MON11");
                            break;
                        case 3:
                            list_area.Items.Add("MON12");
                            list_area.Items.Add("MON13");
                            break;
                    }
                    break;
                case 3:
                    switch (list_level.SelectedIndex)
                    {
                        case 0:
                            list_area.Items.Add("IMG01");
                            list_area.Items.Add("IMG02");
                            list_area.Items.Add("IMG03");
                            list_area.Items.Add("IMG04");
                            break;
                        case 1:
                            list_area.Items.Add("IMG05");
                            list_area.Items.Add("IMG06");
                            list_area.Items.Add("IMG07");
                            break;
                        case 2:
                            list_area.Items.Add("IMG08");
                            list_area.Items.Add("IMG09");
                            list_area.Items.Add("IMG10");
                            list_area.Items.Add("IMG11");
                            break;
                        case 3:
                            list_area.Items.Add("IMG12");
                            list_area.Items.Add("IMG13");
                            break;
                    }
                    break;
                case 4:
                    switch (list_level.SelectedIndex)
                    {
                        case 0:
                            list_area.Items.Add("CAV01");
                            list_area.Items.Add("CAV02");
                            break;
                        case 1:
                            list_area.Items.Add("CAV03");
                            list_area.Items.Add("CAV04");
                            list_area.Items.Add("CAV05");
                            list_area.Items.Add("CAV06");
                            list_area.Items.Add("CAV07");
                            list_area.Items.Add("CAV08");
                            break;
                        case 2:
                            list_area.Items.Add("CAV09");
                            list_area.Items.Add("CAV10");
                            list_area.Items.Add("CAV11");
                            break;
                        case 3:
                            list_area.Items.Add("CAV12");
                            break;
                    }
                    break;
                case 5:
                    switch (list_level.SelectedIndex)
                    {
                        case 0:
                            list_area.Items.Add("CAK01");
                            list_area.Items.Add("CAK02");
                            list_area.Items.Add("CAK03");
                            list_area.Items.Add("CAK04");
                            break;
                    }
                    break;
            }
        }

        void list_area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (list_area.SelectedIndex >= 0)
                textbox_levelname.Text = list_area.Items[list_area.SelectedIndex].ToString();
        }
    }
}
