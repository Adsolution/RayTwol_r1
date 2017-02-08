using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RayTwol
{
    public static partial class Editor
    {
        public static void ExtractBackground(string background)
        {
            var fileStream = new FileStream(string.Format("RAY\\IMA\\FND\\{0}.XXX", background), FileMode.Open);
            byte[] file = new byte[fileStream.Length];
            fileStream.Read(file, 0, (int)fileStream.Length);
            fileStream.Close();

            int end = BitConverter.ToInt32(file, 0x8);
            int length = (end - 0x18) / 2;
            int height = BitConverter.ToInt16(file, 0x14);
            int blockSize = height * 64;
            int blockCount = length / blockSize;
            int width = blockCount * 64;

            var bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            var bitmapData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadWrite, bmp.PixelFormat);
            byte[] buffer = new byte[(bmp.Width * bmp.Height) * 3];

            for (int block = 0; block < blockCount; block++)
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < 64; x++)
                    {
                        int x16 = (x * 2) + 0x18;
                        int y16 = (y * 2);
                        int x24 = (x * 3);
                        int y24 = (y * 3);
                        uint colour16 = BitConverter.ToUInt16(file, x16 + (y16 * 64) + ((block * blockSize) * 2)); // BGR 1555
                        buffer[0 + (x24 + (block * 64 * 3)) + (y24 * width)] = (byte)(((colour16 & 0x7C00) >> 10) << 3);
                        buffer[1 + (x24 + (block * 64 * 3)) + (y24 * width)] = (byte)(((colour16 & 0x3E0) >> 5) << 3);
                        buffer[2 + (x24 + (block * 64 * 3)) + (y24 * width)] = (byte)((colour16 & 0x1F) << 3);
                    }
            
            Marshal.Copy(buffer, 0, bitmapData.Scan0, buffer.Length);
            bmp.UnlockBits(bitmapData);

            if (!Directory.Exists("bg"))
                Directory.CreateDirectory("bg");

            bmp.Save(string.Format("bg\\{0}.png", background), ImageFormat.Png);
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
            var tiles = new List<Bitmap>();
            int buffPos = off_tiles;
            int tile = 0;
            for (int fy = 0; fy < (off_palette - off_tiles) / 4096; fy++, buffPos += 256 * 15)
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
                    try
                    {
                        for (int c = 0; c < 256; c++)
                        {
                            byte r = palettes[file[off_assign + tile]][c].R;
                            byte g = palettes[file[off_assign + tile]][c].G;
                            byte b = palettes[file[off_assign + tile]][c].B;
                            entries[c] = System.Drawing.Color.FromArgb(r, g, b);
                            bmp.Palette = palette;
                        }
                    }
                    catch { }

                    bmp.MakeTransparent(System.Drawing.Color.FromArgb(0, 0, 0));
                    tiles.Add(bmp);
                    //bmp.Save(string.Format("{0}\\{1}\\{3}_{2}.png", ExtractDir, world, fx, fy), ImageFormat.Png);
                }

            var ts = new Bitmap(256, (off_palette - off_tiles) / 256, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int posX = 0;
            int posY = 0;
            for (int bmp = 0; bmp < tiles.Count; bmp++)
            {
                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < 16; x++)
                        ts.SetPixel((posX * 16) + x, (posY * 16) + y, tiles[bmp].GetPixel(x, y));
                }

                if (posX < 15)
                    posX++;
                else
                {
                    posX = 0;
                    posY++;
                }
            }

            tileset = ts;
            //Directory.CreateDirectory(ExtractDir);
            //Level.tileset.Save(string.Format("{0}\\{1}.png", ExtractDir, world), ImageFormat.Png);
        }
    }
}
