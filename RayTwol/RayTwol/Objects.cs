using System.Drawing;

namespace RayTwol
{
    public class TypeGroup
    {
        public int width;
        public int height;
        public Type[] types;
        public Type[] undo;

        public Type GetType(int x, int y)
        {
            return types[x + (y * width)];
        }

        public TypeGroup()
        {
        }

        public TypeGroup(int width, int height)
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
}
