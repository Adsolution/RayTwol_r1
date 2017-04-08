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
        
        public static System.Drawing.Point GridSnap(System.Drawing.Point point)
        {
            return new System.Drawing.Point(((point.X + 8) / 16) * 16, ((point.Y + 8) / 16) * 16);
        }

        public static byte[] InsertBytes(byte[] inBytes, byte[] insertBytes, int insertInPos)
        {
            var insertLen = insertBytes.Length - 1;
            var outBytes = new byte[inBytes.Length + insertLen + 1];
            var outLen = outBytes.Length - 1;
            for (int i = 0, j = 0; i < outLen; ++i)
            {
                if (i < insertInPos)
                    outBytes[i] = inBytes[i];
                else if (i == insertInPos)
                    while (j <= insertLen)
                        outBytes[i + j] = insertBytes[j++];
                else
                    outBytes[i + insertLen] = inBytes[i - insertLen];
            }
            return outBytes;
        }
    }

    public enum EditMode
    {
        Events, Graphics, Collision
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

    public enum Behaviours
    {
        //_property = 0x3,

        SpriteAnim = 0x04,

        Ting = 0xA1,
        OneUp = 0x8E,
        SuperPower = 0x52,
        Photographer = 0x15,

        BendingPlant = 0x43,
        Plum = 0x08,
        Magician = 0x05,

        FistUpgrade = 0x5F,

        Cage = 0x3A,

        Antitoon = 0x7B,
        Hunter = 0x0C,
        Livingstone_Tall = 0x00,
        Livingstone_Short = 0x09,
        
        Platform = 0x01,
        Platform_Falling = 0x10,
        Cloud_Disappearing = 0x1A,
        Cloud_Bouncy = 0x1B,
        Cloud_Flashing = 0x1C,

        PricklyBall = 0x29,
        RedDrummer = 0x78,
        
        Ring = 0x8C,

        MapSign = 0x7C,
        ExitSign = 0x2A,
        Gendoor = 0xA4,

        YinYang = 0xB1,
        YinYang_Spiked = 0x06,

        Pencil_Up_Moving = 0xF1,
        Pencil_Up_Moving_Seq = 0xF2,
        Pencil_Down_Moving = 0xB2,
        Pencil_Down_Moving_Seq = 0xB3,
        Pen = 0xF3,

        
        Clown_WaterBalloon = 0x3D,
        Clown_Hammer = 0x3C,

        Spell = 0xD4,
        Spell_RemoveSpell = 0xEC
    }
    

    public static partial class Editor
    {
        public static MainWindow mainWindow;

        static EditMode _editMode;
        public static EditMode editMode
        {
            get
            {
                return _editMode;
            }
            set
            {
                _editMode = value;
                switch (value)
                {
                    case EditMode.Events:
                        mainWindow.togglebutton_events.IsChecked = true;
                        mainWindow.togglebutton_graphics.IsChecked = false;
                        mainWindow.togglebutton_collision.IsChecked = false;
                        break;
                    case EditMode.Graphics:
                        mainWindow.togglebutton_events.IsChecked = false;
                        mainWindow.togglebutton_graphics.IsChecked = true;
                        mainWindow.togglebutton_collision.IsChecked = false;
                        break;
                    case EditMode.Collision:
                        mainWindow.togglebutton_events.IsChecked = false;
                        mainWindow.togglebutton_graphics.IsChecked = false;
                        mainWindow.togglebutton_collision.IsChecked = true;
                        break;
                }
                selSquare.Stroke = brush_hidden;
                Refresh(true);
            }
        }

        public static bool hasSelection;
        public static string levelName;
        public static string world;
        public static Type[] types_screen;
        public static Bitmap tileset;

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
                    activeTypeGroup = Scenes.Template;
                else
                    activeTypeGroup = Scenes.Level;
            }
        }

        public static Type[] undo;
        public static List<Type[]> undos = new List<Type[]>();

        static Scene _activeTypeGroup;
        public static Scene activeTypeGroup
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

        public static List<Event> selectedEvents = new List<Event>();
        public static Event selectedEvent;
        public static int selectedEventsCount;

        public static class Scenes
        {
            public static Scene Level = new Scene();
            public static Scene Template = new Scene(0, 0);
            public static Scene Tileset = new Scene();
            public static Scene Clipboard = new Scene();
            public static Scene Temp = new Scene();
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
                types_screen = new Type[activeTypeGroup.types.Length];
            }

            UpdateViewport(null, EventArgs.Empty);
        }
        
    }
}
