using System;
using System.Runtime.InteropServices;

namespace ZincFramework.Binary.Serialization.Converters
{
    public class SubstantialArrayConverter<T> : ArrayConverter<T> where T : struct
    {
        public override T[] Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int length = SimpleConverters.Int32Converter.Read(ref byteReader, serializerOption);
            ReadOnlySpan<byte> bytes = byteReader.ReadBytes(length);
            ReadOnlySpan<T> values = MemoryMarshal.Cast<byte, T>(bytes);
            return values.ToArray();
        }

        public override void Write(T[] data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            Span<byte> bytes = MemoryMarshal.AsBytes(data.AsSpan());
            SimpleConverters.Int32Converter.Write(bytes.Length, byteWriter, serializerOption);
            byteWriter.WriteBytes(bytes);
        }
    }
}