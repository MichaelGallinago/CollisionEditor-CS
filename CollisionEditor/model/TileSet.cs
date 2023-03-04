﻿using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace CollisionEditor.model
{
    internal class TileSet
    {
        public readonly Size TileSize;

        public List<Bitmap> Tiles { get; private set; }
        public List<byte[]> WidthMap { get; private set; }
        public List<byte[]> HeightMap { get; private set; }

        public TileSet(string path, int tileWidth = 16, int tileHeight = 16,
            Size separate = new Size(), Size offset = new Size())
        {
            TileSize = new Size(tileWidth, tileHeight);

            Tiles = new List<Bitmap>();
            WidthMap  = new List<byte[]>();
            HeightMap = new List<byte[]>();

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
                WidthMap.Add(new byte[TileSize.Width]);
                HeightMap.Add(new byte[TileSize.Height]);

                for (int x = 0; x < TileSize.Width; x++)
                {
                    for (int y = 0; y < TileSize.Height; y++)
                    {
                        if (Tiles[i].GetPixel(x, y).A > 0)
                        {
                            WidthMap[i][x]++;
                            HeightMap[i][y]++;
                        }
                    }
                }
            }
        }

        public TileSet(int angleCount, int tileWidth = 16, int tileHeight = 16)
        {
            Tiles = new List<Bitmap>(angleCount);
            WidthMap  = new List<byte[]>(angleCount);
            HeightMap = new List<byte[]>(angleCount);

            for (int i = 0; i < angleCount; i++)
            {
                Tiles[i] = new Bitmap(tileWidth, tileHeight);
                WidthMap[i]  = new byte[tileWidth];
                HeightMap[i] = new byte[tileHeight];
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