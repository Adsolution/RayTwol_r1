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
        public static int off_sprites;
        public static int off_end;

        public static int count_events;

        public static int event_off_pos = 0x1C;
        public static int event_off_type = 0x63;



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
            off_sprites = BitConverter.ToInt32(XXX, 0x10);
            off_end = BitConverter.ToInt32(XXX, 0x14);

            // types
            Scenes.Level.width = BitConverter.ToUInt16(XXX, off_types);
            Scenes.Level.height = BitConverter.ToUInt16(XXX, off_types + 2);

            if (Scenes.Level.width == 0 || Scenes.Level.height == 0)
            {
                var warn = new Warning("Level open failed", string.Format("Could not read the map dimensions. Hit OK to load the original {0}.", currLevel));
                warn.ShowDialog();
                if (warn.DialogResult == true)
                    OpenLevel(currLevel, "_ORIG");
                else
                    Environment.Exit(0);
                return;
            }

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
            Scenes.Level.events.Clear();
            int ID = 0;
            count_events = 0;
            for (int e = off_events + 16; XXX[e] > 0; e += 0x70, ID++)
            {
                // position
                int x = BitConverter.ToUInt16(XXX, e + event_off_pos);
                int y = BitConverter.ToUInt16(XXX, e + event_off_pos + 2);
                var ev = new Event(x, y);
                ev.ID = ID;
                ev.type = (Behaviours)XXX[e + event_off_type];
                Scenes.Level.events.Add(ev);

                // bytes
                for (int b = 0; b < 0x70; b++)
                    ev.bytes[b] = XXX[b + e];

                off_types -= 113;
                off_sprites -= 113;
                off_end -= 113;

                count_events++;
            }
            RefreshObjectsList();
            


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
            mainWindow.Title = string.Format("RayTwol  -  {0}  -  {1}×{2}", levelName, Scenes.Level.width, Scenes.Level.height);

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        public static void CorruptedLevelWarning(string exception)
        {
            var warn = new Warning("Level open failed", string.Format("{0} appears to be corrupted ({1}). Hit OK to load the original {0}.", levelName, exception));
            warn.ShowDialog();
            if (warn.DialogResult == true)
                OpenLevel(currLevel, "_ORIG");
            else
                return;
        }


        public static void RefreshObjectsList()
        {
            mainWindow.objectsList.Items.Clear();
            foreach (Event e in Scenes.Level.events)
                mainWindow.objectsList.Items.Add(string.Format("{1}    {0}", e.type.ToString(), e.ID.ToString("000"), ((byte)e.type).ToString("X")));
        }

        public static void ClearObjectInfo()
        {
            mainWindow.textbox_ObjName.Text = "";
            mainWindow.textbox_ObjPosX.IsEnabled = false;
            mainWindow.textbox_ObjPosY.IsEnabled = false;
            mainWindow.textbox_ObjPosX.Text = "";
            mainWindow.textbox_ObjPosY.Text = "";
        }

        public static void RefreshObjectInfo()
        {
            mainWindow.objectsList.SelectedIndex = -1;
            selectedEventsCount = 0;
            foreach (Event e in Scenes.Level.events)
                if (e.selected)
                    selectedEventsCount++;

            if (selectedEventsCount == 1)
                foreach (Event e in Scenes.Level.events)
                    if (e.selected)
                    {
                        mainWindow.textbox_ObjName.Text = string.Format("{0}  {1}", e.ID.ToString("000"), e.type.ToString(), ((byte)e.type).ToString("X2"));
                        if (e.inLevel)
                        {
                            mainWindow.textbox_ObjPosX.IsEnabled = true;
                            mainWindow.textbox_ObjPosY.IsEnabled = true;
                            mainWindow.textbox_ObjPosX.Text = e.position.X.ToString();
                            mainWindow.textbox_ObjPosY.Text = e.position.Y.ToString();
                            selectedEvent = e;
                        }
                        else
                        {
                            mainWindow.textbox_ObjPosX.IsEnabled = false;
                            mainWindow.textbox_ObjPosY.IsEnabled = false;
                            mainWindow.textbox_ObjPosX.Text = "";
                            mainWindow.textbox_ObjPosY.Text = "";
                        }
                        break;
                    }
            else
            {
                mainWindow.textbox_ObjName.Text = "";
                mainWindow.textbox_ObjPosX.IsEnabled = false;
                mainWindow.textbox_ObjPosY.IsEnabled = false;
                mainWindow.textbox_ObjPosX.Text = "";
                mainWindow.textbox_ObjPosY.Text = "";
            }
        }


        public static void SaveLevelNew(string suffix = "")
        {
            int evecount = count_events;
            //evecount = 71;

            uint o_off = 0x04;
            uint o_bgo = 0x18;
            uint o_eve = 0x198;
            uint o_map = (uint)(o_eve + 16 + (evecount * 0x70) + evecount + 1);
            uint o_tex = (uint)(o_map + 4 + (2 * (Scenes.Level.width * Scenes.Level.height)));
            uint o_end = o_tex + 1;
            
            string folderName = new string(new char[] { currLevel.ToCharArray()[0], currLevel.ToCharArray()[1], currLevel.ToCharArray()[2] });
            string fullName = string.Format("RAY\\{0}\\{1}.XXX_new", folderName, currLevel);

            var file = new FileStream(fullName, FileMode.OpenOrCreate);
            file.SetLength(o_end);


            // Offsets
            file.Write(BitConverter.GetBytes(o_off), 0, 4);
            file.Write(BitConverter.GetBytes(o_bgo), 0, 4);
            file.Write(BitConverter.GetBytes(o_eve), 0, 4);
            file.Write(BitConverter.GetBytes(o_map), 0, 4);
            file.Write(BitConverter.GetBytes(o_tex), 0, 4);
            file.Write(BitConverter.GetBytes(o_end), 0, 4);


            // Events header
            file.Position = o_eve;
            {
                // read memory offset
                var wfile = new FileStream(string.Format("RAY\\{0}\\{0}.XXX", world), FileMode.Open);
                byte[] memoff = new byte[4];
                wfile.Position = 8;
                wfile.Read(memoff, 0, 4);
                wfile.Close();

                // write event memory start
                file.Write(BitConverter.GetBytes(0x800A5014 + BitConverter.ToUInt32(memoff, 0)), 0, 4);
                // write event count
                file.Write(BitConverter.GetBytes((uint)evecount), 0, 4);
                // write event memory end
                file.Write(BitConverter.GetBytes(0x800A5014 + BitConverter.ToUInt32(memoff, 0) + (evecount * 0x70)), 0, 4);
                // write event count (2)
                file.Write(BitConverter.GetBytes((uint)evecount), 0, 4);
            }

            // Event definitions
            for (int e = 0; e < evecount; e++)
            {
                //file.Write(Scenes.Level.events[e].bytes, 0, 0x70);
                for (int i = 0; i < 112; i++)
                    file.WriteByte(0);
            }
                

            // Event links
            for (byte e = 0; e < evecount; e++)
                file.WriteByte(e);
            file.WriteByte(0);


            // UNKNOWN event info


            // Map data
            file.Write(BitConverter.GetBytes((ushort)Scenes.Level.width), 0, 2);
            file.Write(BitConverter.GetBytes((ushort)Scenes.Level.height), 0, 2);
            foreach (Type t in Scenes.Level.types)
            {
                int graphic = t.graphic.X + (t.graphic.Y << 4);
                file.WriteByte((byte)graphic);
                file.WriteByte((byte)(((byte)t.collision << 2) + (graphic >> 8)));
            }


            file.Close();
        }


        public static void SaveLevel(string suffix = "")
        {
            Mouse.OverrideCursor = Cursors.Wait;

            // LEVEL
            if (activeTypeGroup == Scenes.Level)
            {
                
                string folderName = new string(new char[] { currLevel.ToCharArray()[0], currLevel.ToCharArray()[1], currLevel.ToCharArray()[2] });
                string fullName = string.Format("RAY\\{0}\\{1}.XXX", folderName, currLevel);
                FileStream mapFileTemp = new FileStream(fullName, FileMode.Open);
                byte[] XXXa = new byte[mapFileTemp.Length];
                mapFileTemp.Read(XXXa, 0, (int)mapFileTemp.Length);
                mapFileTemp.Close();

                var XXX = XXXa.ToList();

                XXX.RemoveRange(off_events + 16, count_events * 112);
                XXX.RemoveRange(off_events + 16, count_events);

                count_events = 0;

                // Events list
                foreach (Event e in Scenes.Level.events)
                {
                    if (e.inLevel)
                    {
                        byte[] save_posX = BitConverter.GetBytes((short)e.position.X);
                        byte[] save_posY = BitConverter.GetBytes((short)e.position.Y);
                        e.bytes[event_off_pos + 0] = save_posX[0];
                        e.bytes[event_off_pos + 1] = save_posX[1];
                        e.bytes[event_off_pos + 2] = save_posY[0];
                        e.bytes[event_off_pos + 3] = save_posY[1];
                    }

                    XXX.InsertRange(off_events + 16 + (count_events * 112), e.bytes.ToList());
                    off_types += 113;
                    off_sprites += 113;
                    off_end += 113;
                    count_events++;
                }

                // Events counter
                for (int e = 0; e < count_events; e++)
                    XXX.Insert(off_events + 16 + (count_events * 112) + e, (byte)e);

                // Events header
                {
                    // counter determiners
                    byte[] b = BitConverter.GetBytes((uint)count_events);

                    XXX[off_events + 04] = b[0];
                    XXX[off_events + 05] = b[1];
                    XXX[off_events + 06] = b[2];
                    XXX[off_events + 07] = b[3];

                    XXX[off_events + 12] = b[0];
                    XXX[off_events + 13] = b[1];
                    XXX[off_events + 14] = b[2];
                    XXX[off_events + 15] = b[3];

                    // weird list offsets
                    var wfile = new FileStream(string.Format("RAY\\{0}\\{0}.XXX", world), FileMode.Open);
                    byte[] data = new byte[4];
                    wfile.Position = 8;
                    wfile.Read(data, 0, 4);
                    wfile.Close();
                    uint a = 0x800A5014 + BitConverter.ToUInt32(data, 0);
                    b = BitConverter.GetBytes(a);

                    XXX[off_events + 00] = b[0];
                    XXX[off_events + 01] = b[1];
                    XXX[off_events + 02] = b[2];
                    XXX[off_events + 03] = b[3];

                    a += (uint)count_events * 112;
                    b = BitConverter.GetBytes(a);

                    XXX[off_events + 08] = b[0];
                    XXX[off_events + 09] = b[1];
                    XXX[off_events + 10] = b[2];
                    XXX[off_events + 11] = b[3];
                }
                

                // Update offsets
                var write_off_types = (BitConverter.GetBytes((uint)off_types));
                for (int i = 0; i < 4; i++)
                    XXX[0xC + i] = write_off_types[i];

                var write_off_sprites = (BitConverter.GetBytes((uint)off_sprites));
                for (int i = 0; i < 4; i++)
                    XXX[0x10 + i] = write_off_sprites[i];

                var write_off_end = (BitConverter.GetBytes((uint)off_end));
                for (int i = 0; i < 4; i++)
                    XXX[0x14 + i] = write_off_end[i];

                int typeCount = 0;
                foreach (Type t in Scenes.Level.types)
                {
                    int graphic = t.graphic.X + (t.graphic.Y << 4);
                    byte byte1 = (byte)graphic;
                    byte byte2 = (byte)(((byte)t.collision << 2) + (graphic >> 8));
                    XXX[off_types + 4 + (typeCount * 2) + 0] = byte1;
                    XXX[off_types + 4 + (typeCount * 2) + 1] = byte2;
                    typeCount++;
                }
                File.Delete(fullName + suffix);
                FileStream mapFile = new FileStream(fullName + suffix, FileMode.Create);
                mapFile.Write(XXX.ToArray(), 0, XXX.Count);
                mapFile.Close();

                OpenLevel(currLevel);

                /*
                var file = new FileStream("level.t", FileMode.OpenOrCreate);
                var w = BitConverter.GetBytes((ushort)Scenes.Level.width);
                var h = BitConverter.GetBytes((ushort)Scenes.Level.height);
                file.WriteByte(w[0]);
                file.WriteByte(w[1]);
                file.WriteByte(h[0]);
                file.WriteByte(h[1]);
                foreach (Type t in Scenes.Level.types)
                {
                    file.WriteByte((byte)t.collision);
                    file.WriteByte((byte)t.graphic.X);
                    file.WriteByte((byte)t.graphic.Y);
                }
                file.Close();*/
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
