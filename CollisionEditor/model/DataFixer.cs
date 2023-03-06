using System.Drawing;

namespace CollisionEditor.model
{
    internal static class DataFixer
    {
        public static void SupplementElements(AngleMap angleMap, TileSet tileSet)
        {
            if (tileSet.Tiles.Count < angleMap.Values.Count)
            {
                Size size = tileSet.TileSize;
                for (int i = tileSet.Tiles.Count; i < angleMap.Values.Count; i++)
                {
                    tileSet.Tiles.Add(new Bitmap(size.Width, size.Height));
                    tileSet.WidthMap.Add(new byte[size.Width]);
                    tileSet.HeightMap.Add(new byte[size.Height]);
                }
            }
            else
            {
                for (int i = angleMap.Values.Count; i < tileSet.Tiles.Count; i++)
                {
                    angleMap.Values.Add(0);
                }
            }
        }
    }
}
