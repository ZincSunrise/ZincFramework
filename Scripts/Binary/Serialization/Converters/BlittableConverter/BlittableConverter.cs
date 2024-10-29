using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZincFramework.Binary.Serialization.Converters
{
    public class BlittableConverter<T> : BinaryConverter<T> where T : struct
    {
        public override ConvertStrategy GetConvertStrategy() => ConvertStrategy.Blittable;

        public override T Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {
            int length = SimpleConverters.Int32Converter.Read(ref byteReader, serializerOption);
            ReadOnlySpan<byte> bytes = byteReader.ReadBytes(length);

            return MemoryMarshal.Read<T>(bytes);
        }

        public override void Write(T data, ByteWriter byteWriter, SerializerOption serializerOption)
        {
            int size = Unsafe.SizeOf<T>();
            Span<byte> bytes = stackalloc byte[size];
            MemoryMarshal.Write(bytes, ref data);

            SimpleConverters.Int32Converter.Write(size, byteWriter, serializerOption);
            byteWriter.WriteBytes(bytes);
        }
    }
}
