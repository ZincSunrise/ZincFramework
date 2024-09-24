using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace ZincFramework.Binary.Serialization.Converters
{
    public class SubstantialListConverter<T> : ListConverter<T> where T : struct
    {
        private readonly FieldInfo _getItemField;

        private readonly FieldInfo _getSizeField;

        public SubstantialListConverter()
        {
            _getItemField = typeof(List<T>).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
            _getSizeField = typeof(List<T>).GetField("_size", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public override List<T> Convert(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int length = SimpleConverters.Int32Converter.Convert(ref byteReader, serializerOption);
            ReadOnlySpan<byte> bytes = byteReader.ReadBytes(length);
            T[] array = MemoryMarshal.Cast<byte, T>(bytes).ToArray();
            List<T> list = new List<T>();

            _getItemField.SetValue(list, array);
            _getSizeField.SetValue(list, array.Length);
            return list;
        }

        public override void Write(List<T> data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            T[] array = _getItemField.GetValue(data) as T[];

            Span<byte> bytes = MemoryMarshal.AsBytes(array.AsSpan()[..data.Count]);
            SimpleConverters.Int32Converter.Write(bytes.Length, byteWriter, serializerOption);
            byteWriter.WriteBytes(bytes);
        }
    }
}