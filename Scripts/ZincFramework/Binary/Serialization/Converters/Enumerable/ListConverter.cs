using System.Collections.Generic;



namespace ZincFramework.Binary.Serialization.Converters
{
    public class ListConverter<T> : IListConverterOfT<List<T>, T>
    {
        public override List<T> Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int count = byteReader.ReadInt32();

            List<T> list = new List<T>(count);
            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);

            for (int i = 0; i < count; i++) 
            {
                list.Add(_elementTypeInfo.WrapperConverter.Convert(ref byteReader, serializerOption));
            }

            return list;
        }

        public override void Write(List<T> data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteInt32(data.Count);
            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);

            for (int i = 0; i < data.Count; i++) 
            {
                _elementTypeInfo.WrapperConverter.Write(data[i], byteWriter, serializerOption);
            }
        }
    }
}