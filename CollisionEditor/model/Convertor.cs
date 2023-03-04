﻿using System;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;

namespace CollisionEditor.model
{
    internal static class Convertor
    {
        private const double dpi = 0.1;
        public static BitmapSource BitmapConvert(Bitmap bitmap)
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

        public static double Get360Angle(byte angle)
        {
            return Math.Round((256 - angle) * 1.40625, 2);
        }

        public static int Get256Angle(string hexAngle)
        {
            return int.Parse(hexAngle.Substring(2), NumberStyles.HexNumber);
        }
    }
}
