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
    }
}
