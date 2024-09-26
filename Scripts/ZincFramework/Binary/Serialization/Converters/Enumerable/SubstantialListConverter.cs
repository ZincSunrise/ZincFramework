using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ZincFramework.Binary.Serialization.MetaModule;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class SubstantialListConverter<T> : ListConverter<T> where T : struct
    {
        private readonly Func<List<T>, T[]> _getItemField;

        private readonly Action<List<T>, T[]> _setItemField;

        private readonly Action<List<T>, int> _setSizeField;

        public SubstantialListConverter()
        {
            FieldInfo itemField = typeof(List<T>).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo sizeField = typeof(List<T>).GetField("_size", BindingFlags.NonPublic | BindingFlags.Instance);

            (_getItemField, _setItemField) = DefaultMetaModule.AccessorsProvider.GetField<List<T>, T[]>(itemField);
            _setSizeField = DefaultMetaModule.AccessorsProvider.SetSizeField<List<T>>(sizeField);
        }

        public override List<T> Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int length = SimpleConverters.Int32Converter.Read(ref byteReader, serializerOption);
            ReadOnlySpan<byte> bytes = byteReader.ReadBytes(length);
            T[] array = MemoryMarshal.Cast<byte, T>(bytes).ToArray();
            List<T> list = new List<T>();

            _setItemField.Invoke(list, array);
            _setSizeField.Invoke(list, array.Length);
            return list;
        }

        public override void Write(List<T> data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            T[] array = _getItemField.Invoke(data);

            Span<byte> bytes = MemoryMarshal.AsBytes(array.AsSpan()[..data.Count]);
            SimpleConverters.Int32Converter.Write(bytes.Length, byteWriter, serializerOption);
            byteWriter.WriteBytes(bytes);
        }
    }
}