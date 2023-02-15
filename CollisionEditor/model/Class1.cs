using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollisionEditor.model
{
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

        public void Save(string path, int rowCount, int tileWidth = 16, int tileHeight = 16,
            int separateX = 0, int separateY = 0, int offsetX = 0, int offsetY = 0)
        {
            if (File.Exists(path))
                File.Delete(path);

            int width  = offsetX + rowCount * (TileWidth + separateX) - separateX;
            int height = offsetX + rowCount * (TileWidth + separateX) - separateX;
            Bitmap image = new Bitmap(width, height);

            foreach (Bitmap bitmap in bitmaps)
            {

            }

            image.Save(path, ImageFormat.Png);
        }
    }
}
