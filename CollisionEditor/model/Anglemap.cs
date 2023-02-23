using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CollisionEditor.model
{
    internal class Anglemap
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

        public void UpdateWithLine(int index, Vector2<int> positionGreen, Vector2<int> positionBlue)
        {
            Values[index] = (byte)(256 - Math.Atan2(positionGreen.Y - positionBlue.Y, positionGreen.X - positionBlue.X) * 128 / Math.PI);
        }
    }
}
