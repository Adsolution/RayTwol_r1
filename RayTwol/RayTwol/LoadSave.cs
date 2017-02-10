using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RayTwol
{
    public static partial class Editor
    {
        public static int off_events;
        public static int off_types;



        public static void OpenLevel(string map, string suffix = "")
        {
            Mouse.OverrideCursor = Cursors.Wait;

            // LEVEL
            currLevel = map.ToUpper();
            world = new string(new char[] { currLevel.ToCharArray()[0], currLevel.ToCharArray()[1], currLevel.ToCharArray()[2] });
            string fullName = string.Format("RAY\\{0}\\{1}.XXX", world, currLevel);

            ExtractTiles(world);

            if (!File.Exists(fullName + "_ORIG"))
                File.Copy(fullName, fullName + "_ORIG");

            FileStream mapFile = new FileStream(fullName + suffix, FileMode.Open);
            byte[] XXX = new byte[mapFile.Length];
            mapFile.Read(XXX, 0, (int)mapFile.Length);
            mapFile.Close();

            off_events = BitConverter.ToInt32(XXX, 0x8);
            off_types = BitConverter.ToInt32(XXX, 0xC);
            
            // types
            Scenes.Level.width = BitConverter.ToUInt16(XXX, off_types);
            Scenes.Level.height = BitConverter.ToUInt16(XXX, off_types + 2);
            Scenes.Level.types = new Type[Scenes.Level.width * Scenes.Level.height];
            types_screen = Scenes.Level.types.ToArray();
            levelName = map;
            
            int i = off_types + 4;

            for (int n = 0; n < Scenes.Level.width * Scenes.Level.height; n++)
            {
                Scenes.Level.types[n] = new Type();
                int graphic = XXX[i] + ((XXX[i + 1] & 3) << 8);
                Scenes.Level.types[n].graphic.X = graphic & 15;
                Scenes.Level.types[n].graphic.Y = graphic >> 4;
                Scenes.Level.types[n].collision = (Collisions)(XXX[i + 1] >> 2);
                i += 2;
            }

            // events
            mainWindow.objectsList.Items.Clear();
            Scenes.Level.events.Clear();
            for (int e = off_events + 16; XXX[e] > 0; e += 0x70)
            {
                var ev = new Event();

                // bytes
                for (int b = 0; b < 0x70; b++)
                    ev.bytes[b] = XXX[b + e];
                
                int off_pos = 0x1C;

                // position
                int x = BitConverter.ToUInt16(XXX, e + off_pos);
                int y = BitConverter.ToUInt16(XXX, e + off_pos + 2);
                ev.position = new System.Drawing.Point(x, y);
                if (x < Scenes.Level.width * 16 && y < Scenes.Level.height * 16)
                    Scenes.Level.events.Add(ev);

                mainWindow.objectsList.Items.Add(ev.position.ToString());
            }



            // TILESET
            Scenes.Tileset.width = 16;
            Scenes.Tileset.height = tileset.Height / 16;
            Scenes.Tileset.types = new Type[tileset.Height];
            for (int y = 0; y < Scenes.Tileset.height; y++)
                for (int x = 0; x < 16; x++)
                {
                    Scenes.Tileset.types[x + (y * Scenes.Tileset.width)].graphic = new System.Drawing.Point(x, y);
                    Scenes.Tileset.types[x + (y * Scenes.Tileset.width)].collision = Collisions.none;
                }

            // TEMPLATE
            Scenes.Template = new Scene(10, 10);
            if (File.Exists(world + "_template.txt"))
            {
                var template = new StreamReader(world + "_template.txt");
                var spl = new char[] { ' ' };
                string[] line = template.ReadLine().Split(spl);
                int tWidth = int.Parse(line[0]);
                int tHeight = int.Parse(line[1]);
                Scenes.Template = new Scene(tWidth, tHeight);
                for (int t = 0; t < tWidth * tHeight; t++)
                {
                    if (template.EndOfStream)
                        break;
                    var l = template.ReadLine().Split(spl);
                    Scenes.Template.types[t].graphic = new System.Drawing.Point(int.Parse(l[0]), int.Parse(l[1]));
                    Scenes.Template.types[t].collision = (Collisions)int.Parse(l[2]);
                }
                template.Close();
            }
            
            activeTypeGroup = Scenes.Level;

            Mouse.OverrideCursor = Cursors.Arrow;
        }





        public static void SaveLevel(string suffix = "")
        {
            Mouse.OverrideCursor = Cursors.Wait;

            // LEVEL
            if (activeTypeGroup == Scenes.Level)
            {
                string folderName = new string(new char[] { currLevel.ToCharArray()[0], currLevel.ToCharArray()[1], currLevel.ToCharArray()[2] });
                string fullName = string.Format("RAY\\{0}\\{1}.XXX", folderName, currLevel);
                FileStream mapFile = new FileStream(fullName + suffix, FileMode.Open);
                mapFile.Position = off_types + 4;

                foreach (Type t in Scenes.Level.types)
                {
                    int graphic = t.graphic.X + (t.graphic.Y << 4);
                    byte byte1 = (byte)graphic;
                    byte byte2 = (byte)(((byte)t.collision << 2) + (graphic >> 8));
                    mapFile.WriteByte(byte1);
                    mapFile.WriteByte(byte2);
                }
                mapFile.Close();
            }
            
            // TEMPLATE
            else if (activeTypeGroup == Scenes.Template)
            {
                var template = new StreamWriter(world + "_template.txt");
                template.WriteLine("{0} {1}", Scenes.Template.width, Scenes.Template.height);
                foreach (Type t in Scenes.Template.types)
                    template.WriteLine("{0} {1} {2}", t.graphic.X, t.graphic.Y, (int)t.collision);
                template.Close();
            }


            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
