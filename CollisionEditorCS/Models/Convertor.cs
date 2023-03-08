using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;

namespace CollisionEditor.model
{
    internal static class Convertor
    {
        public static Avalonia.Media.Imaging.Bitmap BitmapConvert(Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), 
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            var avaloniaBitmap = new Avalonia.Media.Imaging.Bitmap(
                Avalonia.Platform.PixelFormat.Bgra8888, Avalonia.Platform.AlphaFormat.Premul,
                bitmapData.Scan0,
                new Avalonia.PixelSize(bitmapData.Width, bitmapData.Height),
                new Avalonia.Vector(96, 96),
                bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            bitmap.Dispose();

            return avaloniaBitmap;
        }

        public static string GetHexAngle(byte angle)
        {
            return string.Format("0x{0:X}", angle);
        }

        public static double GetFullAngle(byte angle)
        {
            return Math.Round((256 - angle) * 1.40625, 2);
        }

        public static int GetByteAngle(string hexAngle)
        {
            return int.Parse(hexAngle.Substring(2), NumberStyles.HexNumber);
        }
    }
}
