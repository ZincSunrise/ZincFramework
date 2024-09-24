using System.Collections.Generic;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class QueueConverter<T> : IEnumerableConverter<Queue<T>, T>
    {
        public override Queue<T> Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int count = SimpleConverters.Int32Converter.Convert(ref byteReader, serializerOption);
            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);
            Queue<T> queue = new Queue<T>();

            for (int i = 0; i < count; i++) 
            {
                queue.Enqueue(_elementTypeInfo.WrapperConverter.Convert(ref byteReader, serializerOption));
            }

            return queue;
        }

        public override void Write(Queue<T> data, ByteWriter byteWriter, SerializerOption serializerOption)
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