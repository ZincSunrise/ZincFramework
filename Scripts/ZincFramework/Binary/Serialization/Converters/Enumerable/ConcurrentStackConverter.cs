using System.Collections.Concurrent;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class ConcurrentStackConverter<T> : IEnumerableConverter<ConcurrentStack<T>, T>
    {
        public override ConcurrentStack<T> Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int count = SimpleConverters.Int32Converter.Read(ref byteReader, serializerOption);
            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);
            ConcurrentStack<T> stack = new ConcurrentStack<T>();

            for (int i = 0; i < count; i++) 
            {
                T element = _elementTypeInfo.WrapperConverter.Read(ref byteReader, serializerOption);
                if(stack.Count > 0)
                {
                    stack.TryPop(out var pop);
                    stack.Push(element);
                    stack.Push(pop);
                }
                else
                {
                    stack.Push(element);
                }
            }

            return stack;
        }

        public override void Write(ConcurrentStack<T> data, ByteWriter byteWriter, SerializerOption serializerOption)
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