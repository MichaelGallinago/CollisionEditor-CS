using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CollisionEditor.model
{
    public class Anglemap
    {
        internal List<byte> Values { get; set; }

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

        public byte UpdateWithLine(int tileIndex, Vector2<int> positionGreen, Vector2<int> positionBlue)
        {
            return Values[tileIndex] = (byte)(Math.Atan2(positionBlue.Y - positionGreen.Y, positionBlue.X - positionGreen.X) * 128 / Math.PI);
        }
    }
}
