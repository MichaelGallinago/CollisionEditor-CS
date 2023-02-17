using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;

namespace CollisionEditor.model
{
    class Convertor
    {
        public static BitmapSource BitmapConvert(Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(bitmapData.Width, bitmapData.Height, 0.1, 0.1,
                PixelFormats.Bgra32, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }

        public static string GetHexAngle(byte angle)
        {
            return "";
        }

        public static float Get360Angle(byte angle)
        {
            return 0f;
        }

        public static int Get256Angle(string hexAngle)
        {
            return 0;
        }
    }
}
