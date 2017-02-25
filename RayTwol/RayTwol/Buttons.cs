using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Drawing;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using System.Timers;

namespace RayTwol
{
    partial class MainWindow
    {
        void LevelLoad(object sender, EventArgs e)
        {
            objectsList.SelectedIndex = -1;
            objectsList.Items.Clear();
        }



        
        // CLOSE MAIN WINDOW
        void MainWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }



        // CHANGE LEVEL
        void dropdown_Levels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        
        
        // FILE
        void button_Revert_Click(object sender, RoutedEventArgs e)
        {
            if (Editor.currLevel != null)
                Editor.OpenLevel(Editor.currLevel, "_ORIG");
        }
        void button_Fetch_Click(object sender, RoutedEventArgs e)
        {
            //if (Editor.currLevel != null && File.Exists(Editor.currLevel.FullName + "_HOLD"))
                //Editor.OpenLevel(Editor.currLevel, "_HOLD");
        }
        void button_Hold_Click(object sender, RoutedEventArgs e)
        {
            if (Editor.currLevel != null)
                Editor.SaveLevel("_HOLD");
        }
        void button_Save_click(object sender, RoutedEventArgs e)
        {
            if (Editor.currLevel != null)
                Editor.SaveLevel();
        }


        // RUN/SYNC GAME
        private void button_Run_Click(object sender, RoutedEventArgs e)
        {/*
            if (Process.GetProcessesByName("Rayman2").Length == 0)
            {
                var r2 = new ProcessStartInfo();
                r2.WorkingDirectory = Editor.cf_gameDir;
                r2.FileName = "Rayman2.exe";
                Memory.process = Process.Start(r2);
                if (Memory.canSync)
                    Memory.isSynced = true;
            }
            else
            {
                if (!Memory.isSynced)
                {
                    Memory.process = Process.GetProcessesByName("Rayman2")[0];
                    if (Memory.canSync)
                        Memory.isSynced = true;
                }
                else
                {
                    Memory.isSynced = false;
                }
            }*/
        }/*
        void CheckIfGameRunning(object sender, ElapsedEventArgs e)
        {
            if (Process.GetProcessesByName("Rayman2").Length == 0)
            {
                Memory.runButtonText = "RUN";
                if (Memory.isSynced)
                    Memory.isSynced = false;
            }
            else
                Memory.runButtonText = "SYNC";

            if (Memory.isSynced)
                Memory.runButtonText = "DESYNC";
        }
        */
        
        // OPEN HELP
        void button_Help_Click(object sender, RoutedEventArgs e)
        {/*
            if (!Global.viewingHelp)
            {
                Global.help = new Help();
                Global.help.Show();
            }
            else
                Global.help.Focus();*/
        }















        // TYPES
        void button_type_solid_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_solid;
        }
        void button_type_passthrough_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_passthrough;
        }
        void button_type_hill_steep_left_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_hill_steep_left;
        }
        void button_type_hill_steep_right_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_hill_steep_right;
        }
        void button_type_hill_slight_left_1_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_hill_slight_left_1;
        }
        void button_type_hill_slight_left_2_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_hill_slight_left_2;
        }
        void button_type_hill_slight_right_2_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_hill_slight_right_2;
        }
        void button_type_hill_slight_right_1_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_hill_slight_right_1;
        }
        void button_type_slippery_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_slippery;
        }
        void button_type_slippery_steep_left_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_slippery_steep_left;
        }
        void button_type_slippery_steep_right_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_slippery_steep_right;
        }
        void button_type_slippery_slight_left_1_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_slippery_slight_left_1;
        }
        void button_type_slippery_slight_left_2_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_slippery_slight_left_2;
        }
        void button_type_slippery_slight_right_2_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_slippery_slight_right_2;
        }
        void button_type_slippery_slight_right_1_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_slippery_slight_right_1;
        }
        void button_type_bounce_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_bounce;
        }
        void button_type_climb_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_climb;
        }
        void button_type_damage_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_damage;
        }
        void button_type_spikes_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_spikes;
        }
        void button_type_water_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_water;
        }
        void button_type_cliff_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_cliff;
        }
        void button_type_event_Click(object sender, RoutedEventArgs e)
        {
            Editor.selectedType = Collisions.type_event;
        }











        void button_replace_click(object sender, RoutedEventArgs e)
        {
            var replaceWindow = new Replace();
            replaceWindow.ShowDialog();
            Editor.Refresh();
        }



        private void button_grid_click(object sender, RoutedEventArgs e)
        {/*
            if (Editor.gridEnabled)
                Editor.gridEnabled = false;
            else
                Editor.gridEnabled = true;*/
        }
    }
}
