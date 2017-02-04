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
    partial class MainWindow : Window
    {
        DrawingGroup displayCollision = new DrawingGroup();
        DrawingGroup displayGraphics = new DrawingGroup();


        public MainWindow(string lvl)
        {
            InitializeComponent();
            Editor.GridToggle += GridToggle;
            Editor.UpdateViewport += UpdateViewport;
            KeyDown += KeyPressed;
            KeyUp += KeyReleased;

            Editor.brush_create.Color = System.Windows.Media.Color.FromArgb(200, 0, 255, 0);
            Editor.brush_delete.Color = System.Windows.Media.Color.FromArgb(200, 255, 0, 0);
            Editor.brush_hidden.Color = System.Windows.Media.Color.FromArgb(0, 0, 0, 0);
            Editor.selSquare.StrokeThickness = 3;
            Editor.selSquare.Stroke = Editor.brush_hidden;
            viewport_canvas.Children.Add(Editor.selSquare);

            Editor.OpenLevel(lvl);
        }

        private void UpdateViewport(object sender, EventArgs e)
        {
            Refresh();
        }

        void Refresh()
        {
            displayCollision.Children.Clear();
            
            var bg = new ImageDrawing();
            bg.Rect = new Rect(0, 0, Level.width, Level.height);
            bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/gfx/gen/bg.png", UriKind.Absolute));
            displayCollision.Children.Add(bg);

            // Display GRAPHICS
            int n = 0;
            if (Editor.displayGraphics)
                for (int y = 0; y < Level.height; y++)
                    for (int x = 0; x < Level.width; x++)
                    {
                        if (Level.types[n].graphic.X != 0 || Level.types[n].graphic.Y != 0)
                        {
                            var gfx = new ImageDrawing();
                            gfx.Rect = new Rect(x * 16, y * 16, 16, 16);
                            gfx.ImageSource = new BitmapImage(new Uri(string.Format(Editor.ExtractDir + "\\{0}\\{2}_{1}.png", Level.world, Level.types[n].graphic.X, Level.types[n].graphic.Y), UriKind.Relative));
                            displayCollision.Children.Add(gfx);
                        }
                        n++;
                    }

            // Display COLLISION
            n = 0;
            if (Editor.displayCollision)
                for (int y = 0; y < Level.height; y++)
                    for (int x = 0; x < Level.width; x++)
                    {
                        string typeName = Enum.GetName(typeof(Collisions), Level.types[n].collision);
                        if (Level.types[n].collision != Collisions.empty)
                        {
                            var coll = new ImageDrawing();
                            coll.Rect = new Rect(x * 16, y * 16, 16, 16);
                            coll.ImageSource = new BitmapImage(new Uri("pack://application:,,,/gfx/types/" + typeName + ".png", UriKind.Absolute));
                            displayCollision.Children.Add(coll);
                        }
                        n++;
                    }
            
            // Display TILE SET
            /*
            for (int y = 0; y < 33; y++)
                for (int x = 0; x < 16; x++)
                {
                    var gfx = new ImageDrawing();
                    gfx.Rect = new Rect(x * 16, y * 16, 16, 16);
                    gfx.ImageSource = new BitmapImage(new Uri(string.Format(Editor.ExtractDir + "\\MUS\\{0}_{1}.png", y, x), UriKind.Relative));
                    displayCollision.Children.Add(gfx);
                }*/
            

            if (Editor.gridEnabled)
                CalculateGrid();
            viewport.Source = new DrawingImage(displayCollision);
            label_mapsize.Content = Level.width + "×" + Level.height;
            label_mapname.Content = Editor.currLevel;
        }







        
        void SelectionStart(MouseButtonEventArgs e)
        {
            Input.Changed();
            Editor.selecting = true;
            Editor.selOrigin.X = ((int)(e.GetPosition(viewport).X + 8) / 16) * 16;
            Editor.selOrigin.Y = ((int)(e.GetPosition(viewport).Y + 8) / 16) * 16;
            Editor.selSquare.Width = 16;
            Editor.selSquare.Height = 16;
        }
        void SelectionMove(MouseEventArgs e)
        {
            Input.Changed();
            if (Editor.selecting)
            {
                Editor.selRange.X = ((int)(e.GetPosition(viewport).X + 8) / 16) * 16;
                Editor.selRange.Y = ((int)(e.GetPosition(viewport).Y + 8) / 16) * 16;

                if (Editor.selRange.X < Editor.selOrigin.X)
                {
                    Editor.sel1.X = Editor.selRange.X;
                    Editor.sel2.X = Editor.selOrigin.X;
                }
                else
                {
                    Editor.sel1.X = Editor.selOrigin.X;
                    Editor.sel2.X = Editor.selRange.X;
                }

                if (Editor.selRange.Y < Editor.selOrigin.Y)
                {
                    Editor.sel1.Y = Editor.selRange.Y;
                    Editor.sel2.Y = Editor.selOrigin.Y;
                }
                else
                {
                    Editor.sel1.Y = Editor.selOrigin.Y;
                    Editor.sel2.Y = Editor.selRange.Y;
                }

                Editor.selSquare.Margin = new Thickness(Editor.sel1.X, Editor.sel1.Y, 0, 0);
                Editor.selSquare.Width = Editor.sel2.X - Editor.sel1.X;
                Editor.selSquare.Height = Editor.sel2.Y - Editor.sel1.Y;
            }
        }
        void SelectionEnd(MouseButtonEventArgs e)
        {
            Input.Changed();
            Editor.selSquare.Stroke = Editor.brush_hidden;
            Editor.selecting = false;
            if (Editor.selectMode == selectModes.Create)
                Editor.AddTypes(Editor.selectedType, Editor.sel1.X, Editor.sel1.Y, Editor.sel2.X, Editor.sel2.Y);
            else
                Editor.AddTypes(0, Editor.sel1.X, Editor.sel1.Y, Editor.sel2.X, Editor.sel2.Y);
        }
        



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
            Editor.Update();
        }




        void GridToggle(object sender, EventArgs e)
        {
            Refresh();
        }
        void CalculateGrid()
        {
            for (int y = 0; y < Level.height; y++)
                for (int x = 0; x < Level.width; x++)
                {
                    var square = new ImageDrawing();
                    square.Rect = new Rect(x * 16, y * 16, 16, 16);
                    square.ImageSource = new BitmapImage(new Uri("gfx\\grid.png", UriKind.Relative));
                    displayCollision.Children.Add(square);
                }
        }
        void CalculateGridBytes()
        {
            byte[] ok = new byte[(Level.width * 64) * (Level.height * 64)];
            for (int my = 0; my < Level.height; my++)
                for (int mx = 0; mx < Level.width; mx++)
                {
                    for (int n = 0; n < 16; n += 4)
                    {
                        ok[(mx + (my * Level.width) * 64) + n + 0] = 200;
                        ok[(mx + (my * Level.width) * 64) + n + 1] = 200;
                        ok[(mx + (my * Level.width) * 64) + n + 2] = 200;
                        ok[(mx + (my * Level.width) * 64) + n + 3] = 255;
                    }
                    for (int n = 0; n < 16; n += 4)
                    {
                        ok[(mx + (my * Level.width) * 64) + (n * Level.width * 64) + 0] = 200;
                        ok[(mx + (my * Level.width) * 64) + (n * Level.width * 64) + 1] = 200;
                        ok[(mx + (my * Level.width) * 64) + (n * Level.width * 64) + 2] = 200;
                        ok[(mx + (my * Level.width) * 64) + (n * Level.width * 64) + 3] = 255;
                    }
                }
        }

        private void button_grid_click(object sender, RoutedEventArgs e)
        {/*
            if (Editor.gridEnabled)
                Editor.gridEnabled = false;
            else
                Editor.gridEnabled = true;*/
        }


        void viewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                SelectionStart(e);
        }
        void viewport_canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                SelectionStart(e);
        }

        void viewport_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                SelectionMove(e);

            int x = (int)e.GetPosition(viewport).X;
            int y = (int)e.GetPosition(viewport).Y;

            if (x < Level.width * 16 && y < Level.height * 16)
            {
                label_mgraphic.Margin = new Thickness(x, y, x + label_mgraphic.ActualWidth, y + label_mgraphic.ActualHeight);
                Type mouseType = Level.types[((x / 16) + ((y / 16) * Level.width))];
                label_mgraphic.Content = mouseType.graphic.X + ", " + mouseType.graphic.Y + "\n" + mouseType.collision;
            }

        }
        void viewport_canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                SelectionMove(e);
        }

        void viewport_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                SelectionEnd(e);
        }
        void viewport_canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                SelectionEnd(e);
        }



        void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z) Input.Z = true;

            Input.Changed();
        }
        void KeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z) Input.Z = false;

            Input.Changed();
        }



    }
}
