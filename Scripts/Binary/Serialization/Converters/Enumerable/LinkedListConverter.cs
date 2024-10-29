using System.Collections.Generic;



namespace ZincFramework.Binary.Serialization.Converters
{
    public class LinkedListConverter<T> : IEnumerableConverter<LinkedList<T>, T>
    {
        public override LinkedList<T> Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int count = SimpleConverters.Int32Converter.Read(ref byteReader, serializerOption);
            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);
            LinkedList<T> values = new LinkedList<T>();

            for (int i = 0; i < count; i++)
            {
                values.AddLast(_elementTypeInfo.WrapperConverter.Read(ref byteReader, serializerOption));
            }

            return values;
        }

        public override void Write(LinkedList<T> data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            SimpleConverters.Int32Converter.Write(data.Count, byteWriter, serializerOption);
            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);
            foreach (var item in data)
            {
                _elementTypeInfo.WrapperConverter.Write(item, byteWriter, serializerOption);
            }
        }
    }
}