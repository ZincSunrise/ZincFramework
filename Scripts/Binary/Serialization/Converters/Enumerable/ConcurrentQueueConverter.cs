using System.Collections.Concurrent;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class ConcurrentQueueConverter<T> : IEnumerableConverter<ConcurrentQueue<T>, T>
    {
        public override ConcurrentQueue<T> Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int count = SimpleConverters.Int32Converter.Read(ref byteReader, serializerOption);
            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);
            ConcurrentQueue<T> queue = new ConcurrentQueue<T>();

            for (int i = 0; i < count; i++) 
            {
                queue.Enqueue(_elementTypeInfo.WrapperConverter.Read(ref byteReader, serializerOption));
            }

            return queue;
        }

        public override void Write(ConcurrentQueue<T> data, ByteWriter byteWriter, SerializerOption serializerOption)
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