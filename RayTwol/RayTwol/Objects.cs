using System.Drawing;

namespace RayTwol
{
    public static class Level
    {
        public static string world;
        public static string name;
        public static int width;
        public static int height;
        
        public static int off_types;

        public static Type[] types;
    }

    public struct Type
    {
        public Point graphic;
        public Collisions collision;
    }
}
