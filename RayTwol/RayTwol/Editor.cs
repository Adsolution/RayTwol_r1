using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RayTwol
{
    public static class Func
    {
        public static int Wrap(int value, int min, int max)
        {
            if (min != 0 || max != 0)
                return (((value - min) % (max - min)) + (max - min)) % (max - min) + min;
            else
                return 0;
        }
    }

    public enum EditMode
    {
        Graphics, Collision
    }
    public enum SelectModes
    {
        Select, Create
    }
    public enum Collisions
    {
        none = 0,
        type_solid = 15,
        type_passthrough = 14,
        type_hill_steep_left = 2,
        type_hill_steep_right = 3,
        type_hill_slight_left_1 = 4,
        type_hill_slight_left_2 = 5,
        type_hill_slight_right_2 = 6,
        type_hill_slight_right_1 = 7,
        type_slippery = 30,
        type_slippery_steep_left = 18,
        type_slippery_steep_right = 19,
        type_slippery_slight_left_1 = 20,
        type_slippery_slight_left_2 = 21,
        type_slippery_slight_right_2 = 22,
        type_slippery_slight_right_1 = 23,
        type_bounce = 9,
        type_climb = 12,
        type_damage = 8,
        type_spikes = 24,
        type_cliff = 25,
        type_water = 10,
        type_event = 1
    }
    

    public static partial class Editor
    {
        public static EditMode editMode = EditMode.Graphics;

        public static bool hasSelection;
        public static string levelName;
        public static string world;
        public static Type[] types_screen;
        public static Bitmap tileset;
        public static int off_types;

        static bool _viewingTemplate;
        public static bool viewingTemplate
        {
            get
            {
                return _viewingTemplate;
            }
            set
            {
                _viewingTemplate = value;
                if (value)
                    activeTypeGroup = TypeGroups.Template;
                else
                    activeTypeGroup = TypeGroups.Level;
            }
        }

        public static Type[] undo;
        public static List<Type[]> undos = new List<Type[]>();

        static TypeGroup _activeTypeGroup;
        public static TypeGroup activeTypeGroup
        {
            get
            {
                return _activeTypeGroup;
            }
            set
            {
                _activeTypeGroup = value;

                Rendering.buff = new byte[(value.width * 16) * (value.height * 16) * 3];
                Rendering.display = new WriteableBitmap(value.width * 16, value.height * 16, 96, 96, PixelFormats.Rgb24, null);
                Rendering.ClearBuffer();
                Refresh(true);
            }
        }

        public static class TypeGroups
        {
            public static TypeGroup Level = new TypeGroup();
            public static TypeGroup Template = new TypeGroup(70, 40);
            public static TypeGroup Tileset = new TypeGroup();
            public static TypeGroup Clipboard = new TypeGroup();
            public static TypeGroup Temp = new TypeGroup();
        }

        public static string ExtractDir = "r1\\tiles";
        public static Collisions selectedType = Collisions.type_solid;
        public static string currLevel;

        public static EventHandler UpdateViewport;
        public static EventHandler GridToggle;
        static bool _gridEnabled = false;
        public static bool gridEnabled
        {
            get
            {
                return _gridEnabled;
            }
            set
            {
                _gridEnabled = value;
                GridToggle(null, EventArgs.Empty);
            }
        }

        


        public static void Refresh(bool force = false)
        {
            if (force)
            {
                Rendering.display = new WriteableBitmap(Editor.activeTypeGroup.width * 16, Editor.activeTypeGroup.height * 16, 96, 96, PixelFormats.Rgb24, null);
                Editor.types_screen = new Type[Editor.activeTypeGroup.types.Length];
            }

            UpdateViewport(null, EventArgs.Empty);
        }
        
    }
}
