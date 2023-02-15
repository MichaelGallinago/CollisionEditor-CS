using System;
using System.Collections.Generic;
using System.Drawing;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CollisionEditor.model
{
    struct Vector2
    {
        int X, Y;
    }

    class Anglemap
    {
        public List<byte> Values { get; set; }

        public Anglemap(string path)
        {
            BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));
            Values = reader.ReadBytes((int)reader.BaseStream.Length).ToList();
        }

        public void Save(string path)
        {
            if (File.Exists(path)) 
                File.Delete(path);

            BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.CreateNew));
            writer.Write(Values.ToArray());
        }
    }

    class TileStrip
    {
        public readonly int TileWidth;
        public readonly int TileHeight;

        public List<Bitmap> bitmaps { get; set; }

        public TileStrip(string path, int tileWidth = 16, int tileHeight = 16, 
            int separateX = 0, int separateY = 0, int offsetX = 0, int offsetY = 0)
        {
            TileWidth  = tileWidth;
            TileHeight = tileHeight;

            bitmaps = new List<Bitmap>();
            Bitmap bitmap = new Bitmap(path);

            int rowCount    = (bitmap.Width  - offsetX) / tileWidth;
            int columnCount = (bitmap.Height - offsetY) / tileHeight;
            for (int y = 0; y < columnCount; y++)
            {
                for (int x = 0; x < rowCount; x++)
                {
                    Rectangle tile = new Rectangle(x * (tileWidth + separateX) + offsetX, 
                        y * (tileHeight + separateY) + offsetY, TileWidth, TileHeight);
                    bitmaps.Add(bitmap.Clone(tile, bitmap.PixelFormat));
                }
            }
        }

        //public void Save(string path, int rowCount, Vector<int> separation = , Vector<int> offset = 0, int offsetY = 0)
        //{
        //    if (File.Exists(path))
        //        File.Delete(path);
        //    Vector<int> vec = new Vector<int>();
        //    vec[0]

        //    int columnCount = ;
        //    int width  = offsetX + rowCount    * (TileWidth + separateX) - separateX;
        //    int height = offsetY + columnCount * (TileHeight + separateY) - separateX;
        //    Bitmap image = new Bitmap(width, height);

        //    using (Graphics graphics = Graphics.FromImage(image))
        //    {
        //        foreach (Bitmap bitmap in bitmaps)
        //        {

        //        }
        //        graphics.DrawImage(
        //            image,
        //            new Rectangle(0, 0, TileWidth, TileHeight),
        //            new Rectangle(0, 0, TileWidth, TileHeight),
        //            GraphicsUnit.Pixel);
        //    }

        //    image.Save(path, ImageFormat.Png);
        //}
    }
}
