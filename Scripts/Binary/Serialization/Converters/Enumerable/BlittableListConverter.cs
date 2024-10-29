using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ZincFramework.Binary.Serialization.MetaModule;
using System.Runtime.CompilerServices;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class BlittableListConverter<T> : ListConverter<T> where T : struct
    {
        private readonly Func<List<T>, T[]> _getItemField;

        private readonly Action<List<T>, int> _setSizeField;

        public BlittableListConverter()
        {
            FieldInfo itemField = typeof(List<T>).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo sizeField = typeof(List<T>).GetField("_size", BindingFlags.NonPublic | BindingFlags.Instance);

            (_getItemField, _) = DefaultMetaModule.AccessorsProvider.GetField<List<T>, T[]>(itemField);
            _setSizeField = (list, size) => sizeField.SetValue(list, size);
        }

        public override List<T> Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int length = SimpleConverters.Int32Converter.Read(ref byteReader, serializerOption);

            List<T> list = new List<T>(length);
            T[] internalArray = _getItemField.Invoke(list);

            //将span中的数据直接赋值
            ReadOnlySpan<T> span = MemoryMarshal.Cast<byte, T>(byteReader.ReadBytes(length * Unsafe.SizeOf<T>()));
            span.CopyTo(internalArray);
            _setSizeField.Invoke(list, length);

            return list;
        }

        public override void Write(List<T> data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            T[] array = _getItemField.Invoke(data);

            Span<byte> bytes = MemoryMarshal.AsBytes(array.AsSpan()[..data.Count]);

            //写入长度
            SimpleConverters.Int32Converter.Write(data.Count, byteWriter, serializerOption);
            byteWriter.WriteBytes(bytes);
        }
    }
}