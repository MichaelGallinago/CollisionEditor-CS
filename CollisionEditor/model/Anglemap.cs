using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CollisionEditor.model
{
    public class Anglemap
    {
        internal List<byte> Values { get; private set; }

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

        public byte SetAngleWithLine(int tileIndex, Vector2<int> positionGreen, Vector2<int> positionBlue)
        {
            return Values[tileIndex] = (byte)(Math.Atan2(positionBlue.Y - positionGreen.Y, positionBlue.X - positionGreen.X) * 128 / Math.PI);
        }

        public byte SetAngle(int tileIndex, byte value)
        {
            return Values[tileIndex] = value;
        }

        public byte ChangeAngle(int tileIndex, int value)
        {
            return Values[tileIndex] = (byte)(Values[tileIndex] + value);
        }
    }
}
