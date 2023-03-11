using System;
using System.Globalization;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Collections.Generic;

namespace CollisionEditor.model
{
    internal static class ViewModelAssistant
    {
        public static (int byteAngle, string hexAngle, double fullAngle) GetAngles(AngleMap angleMap, int chosenTile)
        {
            byte angle = angleMap.Values[chosenTile];
            return (angle, GetHexAngle(angle), GetFullAngle(angle));
        }

        public static string GetCollisionValues(byte[] collisionArray)
        {
            return string.Join("  ", collisionArray);
        }

        public static Vector2<int> GetCorrectDotPosition(Vector2<double> position, int cellSize)
        {
            return new Vector2<int>(
                (int)Math.Floor(position.X) & -cellSize,
                (int)Math.Floor(position.Y) & -cellSize);
        }

        public static BitmapSource BitmapConvert(Bitmap bitmap, double dpi = 0.1)
        {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(bitmapData.Width, bitmapData.Height, dpi, dpi,
                PixelFormats.Bgra32, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }

        public static string GetHexAngle(byte angle)
        {
            return string.Format("0x{0:X}", angle);
        }

        public static double GetFullAngle(byte angle)
        {
            return Math.Round((256 - angle) * 1.40625, 1);
        }

        public static int GetByteAngle(string hexAngle)
        {
            return int.Parse(hexAngle.Substring(2), NumberStyles.HexNumber);
        }

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
