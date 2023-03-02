using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace CollisionEditor.model
{
    internal class Tilemap
    {
        public readonly Size TileSize;

        public List<Bitmap> Tiles { get; private set; }
        public List<byte[]> Widthmap { get; private set; }
        public List<byte[]> Heightmap { get; private set; }


        public Tilemap(string path, int tileWidth = 16, int tileHeight = 16,
            Size separate = new Size(), Size offset = new Size())
        {
            TileSize = new Size(tileWidth, tileHeight);

            Tiles = new List<Bitmap>();
            Widthmap  = new List<byte[]>();
            Heightmap = new List<byte[]>();

            Bitmap bitmap = new Bitmap(path);

            Vector2<int> cellCount = new Vector2<int>(
                (bitmap.Width  - offset.Width)  / TileSize.Width,
                (bitmap.Height - offset.Height) / TileSize.Height);

            for (int y = 0; y < cellCount.Y; y++)
            {
                for (int x = 0; x < cellCount.X; x++)
                {
                    Rectangle tileBounds = new Rectangle(
                        x * (TileSize.Width  + separate.Width)  + offset.Width,
                        y * (TileSize.Height + separate.Height) + offset.Height,
                        TileSize.Width, TileSize.Height);

                    Tiles.Add(bitmap.Clone(tileBounds, bitmap.PixelFormat));
                }
            }

            CreateCollisionmaps();
        }

        private void CreateCollisionmaps()
        {
            for (int i = 0; i < Tiles.Count; i++)
            {
                Widthmap.Add(new byte[TileSize.Width]);
                Heightmap.Add(new byte[TileSize.Height]);

                for (int x = 0; x < TileSize.Width; x++)
                {
                    for (int y = 0; y < TileSize.Height; y++)
                    {
                        if (Tiles[i].GetPixel(x, y).A > 0)
                        {
                            Widthmap[i][x]++;
                            Heightmap[i][y]++;
                        }
                    }
                }
            }
        }

        public Tilemap(int angleCount, int tileWidth = 16, int tileHeight = 16)
        {
            Tiles = new List<Bitmap>(angleCount);
            Widthmap  = new List<byte[]>(angleCount);
            Heightmap = new List<byte[]>(angleCount);

            for (int i = 0; i < angleCount; i++)
            {
                Tiles[i] = new Bitmap(tileWidth, tileHeight);
                Widthmap[i]  = new byte[tileWidth];
                Heightmap[i] = new byte[tileHeight];
            }
        }

        public void Save(string path, int columnCount, Size separation = new Size(), Size offset = new Size())
        {
            if (File.Exists(path))
                File.Delete(path);

            Size cell = new Size(TileSize.Width + separation.Width, TileSize.Height + separation.Height);
            int rowCount = (Tiles.Count & -columnCount) / columnCount;

            Size tilemapSize = new Size(
                offset.Width  + columnCount * cell.Width  - separation.Width, 
                offset.Height + rowCount    * cell.Height - separation.Height);

            Bitmap tilemap = DrawTilemap(columnCount, tilemapSize, separation, offset);

            tilemap.Save(path, ImageFormat.Png);
        }

        public Bitmap GetTilePanel(int panelWidth, Size separation)
        {
            int columnCount = (panelWidth - separation.Width) / (TileSize.Width + separation.Width);
            int panelHeight = (Tiles.Count & -columnCount) / columnCount * (TileSize.Height + separation.Height);

            return DrawTilemap(columnCount, new Size(panelWidth, panelHeight), separation, separation);
        }

        private Bitmap DrawTilemap(int columnCount, Size tilemapSize, Size separation, Size offset)
        {
            Bitmap tilemap = new Bitmap(tilemapSize.Width, tilemapSize.Height);
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

                    if (++position.X >= columnCount)
                    {
                        position.X = 0;
                        position.Y++;
                    }
                }
            }
            return tilemap;
        }
    }
}
