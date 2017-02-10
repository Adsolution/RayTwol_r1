using System.Collections.Generic;
using System.Drawing;

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
    }

    public class Event
    {
        public Point position = new Point(0, 0);
        public byte[] bytes = new byte[112];
    }
}
