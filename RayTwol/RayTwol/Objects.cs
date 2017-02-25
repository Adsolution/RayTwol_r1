using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RayTwol
{
    public class Scene
    {
        public int width;
        public int height;
        public List<Event> events = new List<Event>();
        public Type[] types;
        public Type[] undo;

        public Type GetType(int x, int y)
        {
            return types[x + (y * width)];
        }

        public Scene()
        {
        }

        public Scene(int width, int height)
        {
            this.width = width;
            this.height = height;
            types = new Type[width * height];
        }
    }

    public struct Type
    {
        public Point graphic;
        public Collisions collision;



        public static void BlankOut(int x1, int y1, int x2, int y2)
        {
            Editor.undo = Editor.activeTypeGroup.types.ToArray();
            int selWidth = x2 - x1;
            var clip = Editor.Scenes.Clipboard;
            for (int y = 0; y < y2 - y1; y++)
                for (int x = 0; x < x2 - x1; x++)
                    Editor.activeTypeGroup.types[(x + x1) + ((y + y1) * Editor.activeTypeGroup.width)] = new Type();
            Editor.Refresh();
        }


        public static void SetCollision(Collisions type, int x1, int y1, int x2, int y2)
        {
            Editor.undo = Editor.activeTypeGroup.types.ToArray();
            for (int y = y1; y < y2; y++)
                for (int x = x1; x < x2; x++)
                    Editor.activeTypeGroup.types[x + (y * Editor.activeTypeGroup.width)].collision = type;
            Editor.Refresh();
        }


        public static void Copy(int x1, int y1, int x2, int y2)
        {
            int cWidth = x2 - x1;
            int cHeight = y2 - y1;
            Editor.Scenes.Clipboard.width = cWidth;
            Editor.Scenes.Clipboard.height = cHeight;

            Editor.Scenes.Clipboard.types = new Type[cWidth * cHeight];
            for (int y = 0; y < y2 - y1; y++)
                for (int x = 0; x < x2 - x1; x++)
                    Editor.Scenes.Clipboard.types[x + (y * cWidth)] = Editor.activeTypeGroup.types[(x1 + (y1 * Editor.activeTypeGroup.width)) + (x + (y * Editor.activeTypeGroup.width))];
        }


        public static void SetFromClipboard(int x1, int y1, int x2, int y2)
        {
            if (Editor.Scenes.Clipboard.types != null)
            {
                Editor.undo = Editor.activeTypeGroup.types.ToArray();
                int selWidth = x2 - x1;
                var clip = Editor.Scenes.Clipboard;
                for (int y = 0; y < y2 - y1; y++)
                    for (int x = 0; x < x2 - x1; x++)
                    {
                        var type = clip.GetType(Func.Wrap(x, 0, clip.width), Func.Wrap(y, 0, clip.height));
                        if (type.graphic.X != 0 || type.graphic.Y != 0 || type.collision != Collisions.none)
                            Editor.activeTypeGroup.types[(x + x1) + ((y + y1) * Editor.activeTypeGroup.width)] = type;
                    }
                Editor.Refresh();
            }
        }


        public static void SetFromClipboardTemp(int x1, int y1, int x2, int y2)
        {
            if (Editor.Scenes.Clipboard.types != null)
            {
                Editor.Scenes.Temp.types = Editor.activeTypeGroup.types.ToArray();
                Editor.Scenes.Temp.width = Editor.activeTypeGroup.width;
                Editor.Scenes.Temp.height = Editor.activeTypeGroup.height;
                int selWidth = x2 - x1;
                var clip = Editor.Scenes.Clipboard;

                for (int y = 0; y < y2 - y1; y++)
                    for (int x = 0; x < x2 - x1; x++)
                    {
                        var type = clip.GetType(Func.Wrap(x, 0, clip.width), Func.Wrap(y, 0, clip.height));
                        if (type.graphic.X != 0 || type.graphic.Y != 0 || type.collision != Collisions.none)
                            Editor.activeTypeGroup.types[(x + x1) + ((y + y1) * Editor.activeTypeGroup.width)] = type;
                    }
                Editor.Refresh();
                Editor.activeTypeGroup.types = Editor.Scenes.Temp.types.ToArray();
                Editor.activeTypeGroup.width = Editor.Scenes.Temp.width;
                Editor.activeTypeGroup.height = Editor.Scenes.Temp.height;
            }
        }
    }

    public class Event
    {
        public Event()
        {
        }
        public Event(int x, int y)
        {
            if ((x == 0  && y == 0) || (x == 100 && y == 100) || x > Editor.Scenes.Level.width * 16 || y > Editor.Scenes.Level.width * 16)
                inLevel = false;
            else
                position = new Point(x, y);
        }

        public int ID;

        public bool selected;

        public bool inLevel = true;
        public Point position = new Point(0, 0);
        public Behaviours type;
        public byte[] bytes = new byte[112];

        
        

        public static void Select(int x1, int y1, int x2, int y2)
        {
            x1 -= 6;
            y1 -= 6;
            x2 += 6;
            y2 += 6;
            foreach (Event e in Editor.activeTypeGroup.events)
            {
                if (!Input.CTRL)
                    e.selected = false;
                if (e.position.X > x1 && e.position.X < x2 && e.position.Y > y1 && e.position.Y < y2)
                    e.selected = true;
            }
            Editor.RefreshObjectInfo();
            Editor.mainWindow.RefreshEvents();
        }

        public static void DeselectAll()
        {
            foreach (Event e in Editor.activeTypeGroup.events)
                e.selected = false;
            Editor.mainWindow.RefreshEvents();
        }

        public static void SetTextboxPos()
        {
            try
            {
                int x = int.Parse(Editor.mainWindow.textbox_ObjPosX.Text);
                if (x < 0)
                    Editor.mainWindow.textbox_ObjPosX.Text = Editor.selectedEvent.position.X.ToString();
                else
                {
                    Editor.mainWindow.textbox_ObjPosX.Text = x.ToString();
                    Editor.selectedEvent.position.X = x;
                }
            }
            catch
            {
                Editor.mainWindow.textbox_ObjPosX.Text = Editor.selectedEvent.position.X.ToString();
            }

            try
            {
                int y = int.Parse(Editor.mainWindow.textbox_ObjPosY.Text);
                if (y < 0)
                    Editor.mainWindow.textbox_ObjPosY.Text = Editor.selectedEvent.position.Y.ToString();
                else
                {
                    Editor.mainWindow.textbox_ObjPosY.Text = y.ToString();
                    Editor.selectedEvent.position.Y = y;
                }
            }
            catch
            {
                Editor.mainWindow.textbox_ObjPosY.Text = Editor.selectedEvent.position.Y.ToString();
            }
        }
    }
}
