using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RayTwol
{
    public static class Input
    {
        public static bool A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
            CTRL, SHIFT, ALT, SPACE, TAB, DEL,
            MouseLeft, MouseRight, MouseMiddle;

        public static void Changed()
        {
            if (CTRL && Z)
            {
                if (Editor.undo != null)
                {
                    Type[] undoHold = Editor.activeTypeGroup.types.ToArray();
                    Editor.activeTypeGroup.types = Editor.undo;
                    Editor.undo = undoHold;
                    Editor.Refresh();
                }
            }

            if (MouseLeft && !Editor.selectingRight)
            {
                Editor.selectingLeft = true;
                Editor.SelectingLeft();
                Editor.hasSelection = false;
            }
            if (MouseRight && !Editor.selectingLeft)
            {
                Editor.selectingRight = true;
                Editor.SelectingRight();
                Editor.hasSelection = false;
            }
            
            if (Editor.hasSelection)
            {
                if (DEL)
                    Editor.Types_BlankOut(Editor.sel1.X / 16, Editor.sel1.Y / 16, Editor.sel2.X / 16, Editor.sel2.Y / 16);
                if (CTRL && C)
                    Editor.Types_Copy(Editor.sel1.X / 16, Editor.sel1.Y / 16, Editor.sel2.X / 16, Editor.sel2.Y / 16);
                if (CTRL && X)
                {
                    Editor.Types_Copy(Editor.sel1.X / 16, Editor.sel1.Y / 16, Editor.sel2.X / 16, Editor.sel2.Y / 16);
                    Editor.Types_BlankOut(Editor.sel1.X / 16, Editor.sel1.Y / 16, Editor.sel2.X / 16, Editor.sel2.Y / 16);
                }
            }
        }
    }



    public partial class MainWindow : Window
    {
        void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C) Input.C = true;
            if (e.Key == Key.E) Input.E = true;
            if (e.Key == Key.R) Input.R = true;
            if (e.Key == Key.T) Input.T = true;
            if (e.Key == Key.X) Input.X = true;
            if (e.Key == Key.Z) Input.Z = true;
            if (e.Key == Key.LeftCtrl) Input.CTRL = true;
            if (e.Key == Key.Delete) Input.DEL = true;
            if (e.Key == Key.Space) Input.SPACE = true;

            if (Input.C)
                Editor.viewingTemplate = !Editor.viewingTemplate;
            /*
            if (Input.T)
                Editor.activeTypeGroup = Editor.TypeGroups.Tileset;
            if (Input.R)
                Editor.activeTypeGroup = Editor.TypeGroups.Template;
            if (Input.E)
                Editor.activeTypeGroup = Editor.TypeGroups.Level;*/

            Input.Changed();
        }
        void KeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C) Input.C = false;
            if (e.Key == Key.E) Input.E = false;
            if (e.Key == Key.R) Input.R = false;
            if (e.Key == Key.T) Input.T = false;
            if (e.Key == Key.X) Input.X = false;
            if (e.Key == Key.Z) Input.Z = false;
            if (e.Key == Key.LeftCtrl) Input.CTRL = false;
            if (e.Key == Key.Delete) Input.DEL = false;
            if (e.Key == Key.Space) Input.SPACE = false;

            Input.Changed();
        }




        void viewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.RightButton == MouseButtonState.Released)
            {
                Input.MouseLeft = true;
                Editor.SetSelectionOrigin();
            }

            if (e.RightButton == MouseButtonState.Pressed && e.LeftButton == MouseButtonState.Released)
            {
                Input.MouseRight = true;
                Editor.SetSelectionOrigin();
            }
                
            if (e.MiddleButton == MouseButtonState.Pressed)
                Input.MouseMiddle = true;


            Input.Changed();
        }

        void viewport_MouseMove(object sender, MouseEventArgs e)
        {
            Editor.mouse = new System.Drawing.Point((int)e.GetPosition(viewport).X, (int)e.GetPosition(viewport).Y);

            int x = (int)e.GetPosition(viewport).X;
            int y = (int)e.GetPosition(viewport).Y;
            
            if (x < Editor.activeTypeGroup.width * 16 && y < Editor.activeTypeGroup.height * 16)
            {
                label_mgraphic.Margin = new Thickness(x + 10, y + 18, x + label_mgraphic.ActualWidth, y + label_mgraphic.ActualHeight);
                Type mouseType = Editor.activeTypeGroup.types[((x / 16) + ((y / 16) * Editor.activeTypeGroup.width))];
                label_mgraphic.Text = mouseType.graphic.X + ", " + mouseType.graphic.Y + "\n" + mouseType.collision;
            }

            Input.Changed();
        }

        void viewport_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Editor.selectingLeft)
                Editor.SelectionEndLeft();
            if (Editor.selectingRight)
                Editor.SelectionEndRight();

            if (e.LeftButton == MouseButtonState.Released)
                Input.MouseLeft = false;
            if (e.RightButton == MouseButtonState.Released)
                Input.MouseRight = false;
            if (e.MiddleButton == MouseButtonState.Released)
                Input.MouseMiddle = false;

            Input.Changed();
        }
    }
}
