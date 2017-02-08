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

            off_types = BitConverter.ToInt32(XXX, 0x0C);
            TypeGroups.Level.width = BitConverter.ToUInt16(XXX, off_types);
            TypeGroups.Level.height = BitConverter.ToUInt16(XXX, off_types + 2);
            TypeGroups.Level.types = new Type[TypeGroups.Level.width * TypeGroups.Level.height];
            types_screen = TypeGroups.Level.types.ToArray();
            levelName = map;

            int i = off_types + 4;

            for (int n = 0; n < TypeGroups.Level.width * TypeGroups.Level.height; n++)
            {
                TypeGroups.Level.types[n] = new Type();
                int graphic = XXX[i] + ((XXX[i + 1] & 3) << 8);
                TypeGroups.Level.types[n].graphic.X = graphic & 15;
                TypeGroups.Level.types[n].graphic.Y = graphic >> 4;
                TypeGroups.Level.types[n].collision = (Collisions)(XXX[i + 1] >> 2);
                i += 2;
            }

            // TILESET
            TypeGroups.Tileset.width = 16;
            TypeGroups.Tileset.height = tileset.Height / 16;
            TypeGroups.Tileset.types = new Type[tileset.Height];
            for (int y = 0; y < TypeGroups.Tileset.height; y++)
                for (int x = 0; x < 16; x++)
                {
                    TypeGroups.Tileset.types[x + (y * TypeGroups.Tileset.width)].graphic = new System.Drawing.Point(x, y);
                    TypeGroups.Tileset.types[x + (y * TypeGroups.Tileset.width)].collision = Collisions.none;
                }

            // TEMPLATE
            var template = new StreamReader(world + "_template.txt");
            var spl = new char[] { ' ' };
            string[] line = template.ReadLine().Split(spl);
            int tWidth = int.Parse(line[0]);
            int tHeight = int.Parse(line[1]);
            TypeGroups.Template = new TypeGroup(tWidth, tHeight);
            for (int t = 0; t < tWidth * tHeight; t++)
            {
                if (template.EndOfStream)
                    break;
                var l = template.ReadLine().Split(spl);
                TypeGroups.Template.types[t].graphic = new System.Drawing.Point(int.Parse(l[0]), int.Parse(l[1]));
                TypeGroups.Template.types[t].collision = (Collisions)int.Parse(l[2]);
            }
            template.Close();


            activeTypeGroup = TypeGroups.Level;
            Refresh();

            Mouse.OverrideCursor = Cursors.Arrow;
        }





        public static void SaveLevel(string suffix = "")
        {
            Mouse.OverrideCursor = Cursors.Wait;

            // LEVEL
            if (activeTypeGroup == TypeGroups.Level)
            {
                string folderName = new string(new char[] { currLevel.ToCharArray()[0], currLevel.ToCharArray()[1], currLevel.ToCharArray()[2] });
                string fullName = string.Format("RAY\\{0}\\{1}.XXX", folderName, currLevel);
                FileStream mapFile = new FileStream(fullName + suffix, FileMode.Open);
                mapFile.Position = off_types + 4;

                foreach (Type t in TypeGroups.Level.types)
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
            else if (activeTypeGroup == TypeGroups.Template)
            {
                var template = new StreamWriter(world + "_template.txt");
                template.WriteLine("{0} {1}", TypeGroups.Template.width, TypeGroups.Template.height);
                foreach (Type t in TypeGroups.Template.types)
                    template.WriteLine("{0} {1} {2}", t.graphic.X, t.graphic.Y, (int)t.collision);
                template.Close();
            }


            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
