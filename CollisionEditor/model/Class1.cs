using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CollisionEditor.model
{
    public struct Vector2<T>
    {
        public T X { get; set; }
        public T Y { get; set; }

        public Vector2(T x, T y)
        {
            X = x;
            Y = y;
        }
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
            Vector2<int> separate = new Vector2<int>(), Vector2<int> offset = new Vector2<int>())
        {
            TileWidth  = tileWidth;
            TileHeight = tileHeight;

            bitmaps = new List<Bitmap>();
            Bitmap bitmap = new Bitmap(path);

            int rowCount    = (bitmap.Width  - offset.X) / tileWidth;
            int columnCount = (bitmap.Height - offset.Y) / tileHeight;
            for (int y = 0; y < columnCount; y++)
            {
                for (int x = 0; x < rowCount; x++)
                {
                    Rectangle tile = new Rectangle(x * (tileWidth + separate.X) + offset.X, 
                        y * (tileHeight + separate.Y) + offset.Y, TileWidth, TileHeight);
                    bitmaps.Add(bitmap.Clone(tile, bitmap.PixelFormat));
                }
            }
        }

        public void Save(string path, int rowCount, Vector2<int> separation = new Vector2<int>(), Vector2<int> offset = new Vector2<int>())
        {
            if (File.Exists(path))
                File.Delete(path);

            int columnCount = bitmaps.Count / rowCount + (bitmaps.Count % rowCount == 0 ? 0 : 1);
            int width  = offset.X + rowCount    * (TileWidth  + separation.X) - separation.X;
            int height = offset.Y + columnCount * (TileHeight + separation.Y) - separation.Y;
            Bitmap image = new Bitmap(width, height);

            Vector2<int> Position = new Vector2<int>();
            using (Graphics graphics = Graphics.FromImage(image))
            {
                foreach (Bitmap bitmap in bitmaps)
                {
                    graphics.DrawImage(
                    image,
                    new Rectangle(Position.X * TileWidth, Position.Y * TileHeight, TileWidth, TileHeight),
                    new Rectangle(0, 0, TileWidth, TileHeight),
                    GraphicsUnit.Pixel);

                    if (++Position.X >= rowCount)
                    {
                        Position.X = 0;
                        Position.Y++;
                    }
                }
            }

            image.Save(path, ImageFormat.Png);
        }
    }
}
