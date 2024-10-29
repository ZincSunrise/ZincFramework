using System.Collections.Generic;
using ZincFramework.Binary.Serialization;
using ZincFramework.Serialization;

namespace ZincFramework.Localization
{
    public class LocalizationInfo : ISerializable
    {
        public Dictionary<string, string> LocalizationStr { get; private set; } = new Dictionary<string, string>();

        public LocalizationInfo() { }

        public void Add(string key, string value) 
        {
            LocalizationStr.Add(key, value);
        }

        public void Remove(string key) 
        {
            LocalizationStr.Remove(key);
        }

        public void Write(ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteInt32(LocalizationStr.Count);
            foreach (var data in LocalizationStr)
            {
                byteWriter.WriteString(data.Key);
                byteWriter.WriteString(data.Value);
            }
        }

        public void Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int count = byteReader.ReadInt32();
            for (int i = 0; i < count; i++) 
            {
                LocalizationStr.Add(byteReader.ReadString(), byteReader.ReadString());
            }
        }
    }
}