using System.Collections.Generic;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class StackConverter<T> : IEnumerableConverter<Stack<T>, T>
    {
        public override Stack<T> Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int count = SimpleConverters.Int32Converter.Convert(ref byteReader, serializerOption);
            _elementTypeInfo ??= GetElementTypeInfo(serializerOption);
            Stack<T> stack = new Stack<T>();

            for (int i = 0; i < count; i++) 
            {
                T element = _elementTypeInfo.WrapperConverter.Convert(ref byteReader, serializerOption);
                if(stack.Count > 0)
                {
                    T pop = stack.Pop();
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

        public override void Write(Stack<T> data, ByteWriter byteWriter, SerializerOption serializerOption)
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