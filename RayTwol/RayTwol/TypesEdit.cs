using System;
using System.Collections.Generic;
using System.Linq;

namespace RayTwol
{
    public static partial class Editor
    {
        public static void Types_BlankOut(int x1, int y1, int x2, int y2)
        {
            undo = activeTypeGroup.types.ToArray();
            int selWidth = x2 - x1;
            var clip = TypeGroups.Clipboard;
            for (int y = 0; y < y2 - y1; y++)
                for (int x = 0; x < x2 - x1; x++)
                    activeTypeGroup.types[(x + x1) + ((y + y1) * activeTypeGroup.width)] = new Type();
            Refresh();
        }


        public static void Types_SetCollision(Collisions type, int x1, int y1, int x2, int y2)
        {
            undo = activeTypeGroup.types.ToArray();
            for (int y = y1; y < y2; y++)
                for (int x = x1; x < x2; x++)
                    activeTypeGroup.types[x + (y * activeTypeGroup.width)].collision = type;
            Refresh();
        }


        public static void Types_Copy(int x1, int y1, int x2, int y2)
        {
            int cWidth = x2 - x1;
            int cHeight = y2 - y1;
            TypeGroups.Clipboard.width = cWidth;
            TypeGroups.Clipboard.height = cHeight;

            TypeGroups.Clipboard.types = new Type[cWidth * cHeight];
            for (int y = 0; y < y2 - y1; y++)
                for (int x = 0; x < x2 - x1; x++)
                    TypeGroups.Clipboard.types[x + (y * cWidth)] = activeTypeGroup.types[(x1 + (y1 * activeTypeGroup.width)) + (x + (y * activeTypeGroup.width))];
        }


        public static void Types_SetFromClipboard(int x1, int y1, int x2, int y2)
        {
            if (TypeGroups.Clipboard.types != null)
            {
                undo = activeTypeGroup.types.ToArray();
                int selWidth = x2 - x1;
                var clip = TypeGroups.Clipboard;
                for (int y = 0; y < y2 - y1; y++)
                    for (int x = 0; x < x2 - x1; x++)
                    {
                        var type = clip.GetType(Func.Wrap(x, 0, clip.width), Func.Wrap(y, 0, clip.height));
                        if (type.graphic.X != 0 || type.graphic.Y != 0 || type.collision != Collisions.none)
                            activeTypeGroup.types[(x + x1) + ((y + y1) * activeTypeGroup.width)] = type;
                    }

                Refresh();
            }
        }


        public static void Types_SetFromClipboardTemp(int x1, int y1, int x2, int y2)
        {
            if (TypeGroups.Clipboard.types != null)
            {
                TypeGroups.Temp.types = activeTypeGroup.types.ToArray();
                TypeGroups.Temp.width = activeTypeGroup.width;
                TypeGroups.Temp.height = activeTypeGroup.height;
                int selWidth = x2 - x1;
                var clip = TypeGroups.Clipboard;

                for (int y = 0; y < y2 - y1; y++)
                    for (int x = 0; x < x2 - x1; x++)
                    {
                        var type = clip.GetType(Func.Wrap(x, 0, clip.width), Func.Wrap(y, 0, clip.height));
                        if (type.graphic.X != 0 || type.graphic.Y != 0 || type.collision != Collisions.none)
                            activeTypeGroup.types[(x + x1) + ((y + y1) * activeTypeGroup.width)] = type;
                    }
                Refresh();
                activeTypeGroup.types = TypeGroups.Temp.types.ToArray();
                activeTypeGroup.width = TypeGroups.Temp.width;
                activeTypeGroup.height = TypeGroups.Temp.height;
            }
        }
    }
}
