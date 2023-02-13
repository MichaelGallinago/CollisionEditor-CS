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
        public List<byte> values { get; set; }

        public Anglemap(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException();

            BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open));
            values = reader.ReadBytes(int.MaxValue).ToList();
        }

        public void Save(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.CreateNew));
            writer.Write(values.ToArray());
        }
    }

    class TileStrip
    {
        public List<Bitmap> bitmaps { get; set; }

        public TileStrip(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException();

            Bitmap bitmap = new Bitmap(fileName);
            int count = (bitmap.Width / 16) * (bitmap.Height / 16);
            for (int i = 0; i < count; i++)
            {

            }
            bitmap.GetPixel(0, 0);
            //bitmaps.Add();
        }
    }
}
