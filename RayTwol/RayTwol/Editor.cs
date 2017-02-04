using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;

namespace RayTwol
{
    public enum selectModes
    {
        Create, Delete
    }
    public enum Collisions
    {
        empty = 0,
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
    

    public static class Editor
    {
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

        public static bool displayGraphics = true;
        public static bool displayCollision = true;

        public static string ExtractDir = "tiles";
        public static Collisions selectedType = Collisions.type_solid;
        public static string currLevel;


        public static void Update()
        {
            UpdateViewport(null, EventArgs.Empty);
        }
        
        

        public static void OpenLevel(string map, string suffix = "")
        {
            currLevel = map.ToUpper();
            string folderName = new string(new char[] { currLevel.ToCharArray()[0], currLevel.ToCharArray()[1], currLevel.ToCharArray()[2] });
            string fullName = string.Format("RAY\\{0}\\{1}.XXX", folderName, currLevel);

            if (!Directory.Exists("tiles\\" + folderName))
                ExtractTiles(folderName);

            if (!File.Exists(fullName + "_ORIG"))
                File.Copy(fullName, fullName + "_ORIG");

            FileStream mapFile = new FileStream(fullName + suffix, FileMode.Open);
            byte[] XXX = new byte[mapFile.Length];
            mapFile.Read(XXX, 0, (int)mapFile.Length);
            mapFile.Close();

            Level.off_types = BitConverter.ToInt32(XXX, 0x0C);
            Level.width = BitConverter.ToUInt16(XXX, Level.off_types);
            Level.height = BitConverter.ToUInt16(XXX, Level.off_types + 2);
            Level.types = new Type[Level.width * Level.height];
            Level.name = map;
            Level.world = folderName;

            int i = Level.off_types + 4;

            for (int n = 0; n < Level.width * Level.height; n++)
            {
                Level.types[n] = new Type();
                int graphic = XXX[i] + ((XXX[i + 1] & 3) << 8);
                Level.types[n].graphic.X = graphic & 15;
                Level.types[n].graphic.Y = graphic >> 4;
                Level.types[n].collision = (Collisions)(XXX[i + 1] >> 2);
                i += 2;
            }
            Update();
        }




        public static void SaveLevel(string suffix = "")
        {
            string folderName = new string(new char[] { currLevel.ToCharArray()[0], currLevel.ToCharArray()[1], currLevel.ToCharArray()[2] });
            string fullName = string.Format("RAY\\{0}\\{1}.XXX", folderName, currLevel);
            FileStream mapFile = new FileStream(fullName + suffix, FileMode.Open);
            mapFile.Position = Level.off_types + 5;

            foreach (Type t in Level.types)
            {
                // Convert type data back
            }

            mapFile.Close();
        }







        public static System.Windows.Shapes.Rectangle selSquare = new System.Windows.Shapes.Rectangle();
        public static SolidColorBrush brush_create = new SolidColorBrush();
        public static SolidColorBrush brush_delete = new SolidColorBrush();
        public static SolidColorBrush brush_hidden = new SolidColorBrush();

        public static bool selecting;
        public static selectModes selectMode = selectModes.Create;
        public static System.Drawing.Point selOrigin = new System.Drawing.Point();
        public static System.Drawing.Point selRange = new System.Drawing.Point();
        public static System.Drawing.Point sel1 = new System.Drawing.Point();
        public static System.Drawing.Point sel2 = new System.Drawing.Point();

        public static void AddTypes(Collisions type, int x1, int y1, int x2, int y2)
        {
            for (int y = y1/16; y < y2/16; y++)
                for (int x = x1/16; x < x2/16; x++)
                    Level.types[x + (y * Level.width)].collision = type;
            Update();
        }






        public static void ExtractTiles(string world)
        {
            var fileStream = new FileStream(string.Format("RAY\\{0}\\{0}.XXX", world), FileMode.Open);
            byte[] file = new byte[fileStream.Length];
            fileStream.Read(file, 0, (int)fileStream.Length);
            fileStream.Close();

            int off_tiles = BitConverter.ToInt32(file, 0x18);
            int off_palette = BitConverter.ToInt32(file, 0x1C);
            int off_assign = BitConverter.ToInt32(file, 0x20);

            // Palettes
            var palettes = new List<System.Windows.Media.Color[]>();
            for (int i = off_palette; i < off_assign;)
            {
                palettes.Add(new System.Windows.Media.Color[256]);
                for (int c = 0; c < 256; c++, i += 2)
                {
                    uint colour16 = BitConverter.ToUInt16(file, i); // BGR 1555
                    byte r = (byte)((colour16 & 0x1F) << 3);
                    byte g = (byte)(((colour16 & 0x3E0) >> 5) << 3);
                    byte b = (byte)(((colour16 & 0x7C00) >> 10) << 3);
                    palettes[palettes.Count - 1][c] = System.Windows.Media.Color.FromRgb(r, g, b);
                }
            }

            // Tiles
            Directory.CreateDirectory(string.Format("{0}\\{1}", Editor.ExtractDir, world));
            int buffPos = off_tiles;
            int tile = 0;
            for (int fy = 0; fy < 33; fy++, buffPos += 256 * 15)
                for (int fx = 0; fx < 16; fx++, buffPos -= 0xFF0, tile++)
                {
                    var bmp = new Bitmap(16, 16, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                    var bitmapData = bmp.LockBits(new Rectangle(System.Drawing.Point.Empty, bmp.Size), ImageLockMode.ReadWrite, bmp.PixelFormat);
                    byte[] buffer = new byte[bmp.Width * bmp.Height];

                    for (int y = 0; y < 16; y++, buffPos += 256 - 16)
                        for (int x = 0; x < 16; x++, buffPos++)
                            buffer[x + (y * 16)] = file[buffPos];

                    Marshal.Copy(buffer, 0, bitmapData.Scan0, buffer.Length);
                    bmp.UnlockBits(bitmapData);

                    ColorPalette palette = bmp.Palette;
                    System.Drawing.Color[] entries = palette.Entries;

                    // Assign palette
                    for (int c = 0; c < 256; c++)
                    {
                        byte r = palettes[file[off_assign + tile]][c].R;
                        byte g = palettes[file[off_assign + tile]][c].G;
                        byte b = palettes[file[off_assign + tile]][c].B;
                        entries[c] = System.Drawing.Color.FromArgb(r, g, b);
                        bmp.Palette = palette;
                    }

                    bmp.MakeTransparent(System.Drawing.Color.FromArgb(0, 0, 0));
                    bmp.Save(string.Format("{0}\\{1}\\{3}_{2}.png", Editor.ExtractDir, world, fx, fy), ImageFormat.Png);
                }
        }
    }
}
