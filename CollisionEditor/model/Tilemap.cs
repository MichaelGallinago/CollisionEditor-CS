using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CollisionEditor.model
{
    internal class Tilemap
    {
        public readonly Size TileSize;

        public List<Bitmap> Tiles { get; set; }

        public Tilemap(string path, int tileWidth = 16, int tileHeight = 16,
            Size separate = new Size(), Size offset = new Size())
        {
            TileSize = new Size(tileWidth, tileHeight);
            Tiles = new List<Bitmap>();
            Bitmap bitmap = new Bitmap(path);

            Vector2<int> cellCount = new Vector2<int>(
                (bitmap.Width - offset.Width) / TileSize.Width,
                (bitmap.Height - offset.Height) / TileSize.Height);
            for (int y = 0; y < cellCount.Y; y++)
            {
                for (int x = 0; x < cellCount.X; x++)
                {
                    Rectangle tile = new Rectangle(
                        x * (TileSize.Width + separate.Width) + offset.Width,
                        y * (TileSize.Height + separate.Height) + offset.Height,
                        TileSize.Width, TileSize.Height);
                    Tiles.Add(bitmap.Clone(tile, bitmap.PixelFormat));
                }
            }
        }

        public void Save(string path, int rowCount, Size separation = new Size(), Size offset = new Size())
        {
            if (File.Exists(path))
                File.Delete(path);

            Size cell = new Size(TileSize.Width + separation.Width, TileSize.Height + separation.Height);
            int columnCount = Tiles.Count / rowCount + (Tiles.Count % rowCount == 0 ? 0 : 1);

            Bitmap tilemap = new Bitmap(
                offset.Width  + rowCount    * cell.Width  - separation.Width, 
                offset.Height + columnCount * cell.Height - separation.Height);
            using (Graphics graphics = Graphics.FromImage(tilemap))
            {
                Vector2<int> position = new Vector2<int>();
                foreach (Bitmap tile in Tiles)
                {
                    graphics.DrawImage(
                    tile,
                    new Rectangle(
                        offset.Width  + position.X * (TileSize.Width  + separation.Width),
                        offset.Height + position.Y * (TileSize.Height + separation.Height),
                        TileSize.Width, TileSize.Height),
                    new Rectangle(0, 0, TileSize.Width, TileSize.Height),
                    GraphicsUnit.Pixel);

                    if (++position.X >= rowCount)
                    {
                        position.X = 0;
                        position.Y++;
                    }
                }
            }

            tilemap.Save(path, ImageFormat.Png);
        }
    }
}
