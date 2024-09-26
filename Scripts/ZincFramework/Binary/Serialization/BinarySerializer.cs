using System;
using System.IO;


namespace ZincFramework
{
    namespace Binary
    {
        namespace Serialization
        {
            public static class BinarySerializer
            {
                public static byte[] Serialize(object obj, SerializerOption serializerOption = null)
                {
                    return SerializeInternal(obj, obj.GetType(), serializerOption);
                }

                public static byte[] Serialize(object obj, Type type, SerializerOption serializerOption = null)
                {
                    return SerializeInternal(obj, type, serializerOption);
                }

                public static byte[] Serialize<T>(T t, SerializerOption serializerOption = null)
                {
                    return SerializeInternal(t, serializerOption);
                }

                public static byte[] Serialize<T>(T t, Func<T> factory, SerializerOption serializerOption = null)
                {
                    return SerializeInternal(t, serializerOption, factory);
                }

                public static void Serialize(object obj, Stream stream, SerializerOption serializerOption = null)
                {
                    SerializeInternal(obj, stream, obj.GetType(), serializerOption);
                }

                public static void Serialize(object obj, Stream stream, Type type, SerializerOption serializerOption = null)
                {
                    SerializeInternal(obj, stream, type, serializerOption);
                }

                public static void Serialize<T>(T t, Stream stream, SerializerOption serializerOption = null)
                {
                    SerializeInternal(t, stream, serializerOption);
                }

                public static void Serialize<T>(T t, Stream stream, Func<T> factory, SerializerOption serializerOption = null)
                {
                    SerializeInternal(t, stream, serializerOption, factory);
                }

                public static object Deserialize(byte[] buffer, Type type, SerializerOption serializerOption = null)
                {
                    return DeserializeInternal(buffer, type, serializerOption);
                }

                public static T Deserialize<T>(ReadOnlySpan<byte> bytes, SerializerOption serializerOption = null)
                {
                    return DeserializeInternal<T>(bytes, serializerOption);
                }

                public static T Deserialize<T>(byte[] buffer, SerializerOption serializerOption = null)
                {
                    return DeserializeInternal<T>(buffer, serializerOption);
                }

                public static T Deserialize<T>(ReadOnlySpan<byte> bytes, Func<T> factory, SerializerOption serializerOption = null)
                {
                    return DeserializeInternal(bytes, serializerOption, factory);
                }

                public static T Deserialize<T>(byte[] bytes, Func<T> factory, SerializerOption serializerOption = null)
                {
                    return DeserializeInternal(bytes, serializerOption, factory);
                }

                public static object Deserialize(Stream stream, Type type, SerializerOption serializerOption = null)
                {
                    return DeserializeInternal(stream, type, serializerOption);
                }

                public static T Deserialize<T>(Stream stream, SerializerOption serializerOption = null)
                {
                    return DeserializeInternal<T>(stream, serializerOption);
                }

                public static T Deserialize<T>(Stream stream, Func<T> factory, SerializerOption serializerOption = null)
                {
                    return DeserializeInternal(stream, serializerOption, factory);
                }


                private static byte[] SerializeInternal(object obj, Type type, SerializerOption serializerOption)
                {
                    serializerOption ??= SerializerOption.Default;
                    var byteWriter = ByteWriterPool.GetCachedWriter(serializerOption, out var pooledBufferWriter);

                    SerializeInternal(obj, byteWriter, type, serializerOption);
                    byte[] bytes = pooledBufferWriter.GetSpan().ToArray();
                    ByteWriterPool.ReturnWriterAndBuffer(byteWriter, pooledBufferWriter);

                    return bytes;
                }

                private static byte[] SerializeInternal<T>(T t, SerializerOption serializerOption, Func<T> factory = null)
                {
                    serializerOption ??= SerializerOption.Default;
                    var byteWriter = ByteWriterPool.GetCachedWriter(serializerOption, out var pooledBufferWriter);

                    serializerOption.GetTypeInfo(factory).Serialize(t, byteWriter);
                    byte[] bytes = pooledBufferWriter.WrittenMemory.ToArray();

                    ByteWriterPool.ReturnWriterAndBuffer(byteWriter, pooledBufferWriter);
                    return bytes;
                }


                private static void SerializeInternal(object obj, Stream stream, Type type, SerializerOption serializerOption)
                {
                    serializerOption ??= SerializerOption.Default;
                    serializerOption.GetTypeInfo(type).SerializeAsObject(obj, stream);
                }

                private static void SerializeInternal<T>(T t, Stream stream, SerializerOption serializerOption, Func<T> factory = null)
                {
                    serializerOption ??= SerializerOption.Default;
                    serializerOption.GetTypeInfo(factory).Serialize(t, stream);
                }


                private static void SerializeInternal(object obj, ByteWriter byteWriter, Type type, SerializerOption serializerOption)
                {
                    serializerOption ??= SerializerOption.Default;
                    serializerOption.GetTypeInfo(type).SerializeAsObject(obj, byteWriter);
                }

                private static void SerializeInternal<T>(T t, ByteWriter byteWriter, SerializerOption serializerOption, Func<T> factory = null)
                {
                    serializerOption ??= SerializerOption.Default;
                    serializerOption.GetTypeInfo(factory).Serialize(t, byteWriter);
                }


                private static object DeserializeInternal(ReadOnlySpan<byte> buffer, Type type, SerializerOption serializerOption)
                {
                    serializerOption ??= SerializerOption.Default;
                    ByteReader byteReader = new ByteReader(buffer, serializerOption.GetReaderOption());

                    return serializerOption.GetTypeInfo(type).DeserializeAsObject(ref byteReader);
                }

                private static T DeserializeInternal<T>(ReadOnlySpan<byte> buffer, SerializerOption serializerOption, Func<T> factory = null)
                {
                    serializerOption ??= SerializerOption.Default;
                    ByteReader byteReader = new ByteReader(buffer, serializerOption.GetReaderOption());

                    return serializerOption.GetTypeInfo(factory).Deserialize(ref byteReader);
                }


                private static object DeserializeInternal(Stream stream, Type type, SerializerOption serializerOption)
                {
                    serializerOption ??= SerializerOption.Default;
                    return serializerOption.GetTypeInfo(type).DeserializeAsObject(stream);
                }

                private static T DeserializeInternal<T>(Stream stream, SerializerOption serializerOption, Func<T> factory = null)
                {
                    serializerOption ??= SerializerOption.Default;
                    return serializerOption.GetTypeInfo(factory).Deserialize(stream);
                }

                private static object DeserializeInternal(ref ByteReader byteReader, Type type, SerializerOption serializerOption)
                {
                    serializerOption ??= SerializerOption.Default;
                    return serializerOption.GetTypeInfo(type).DeserializeAsObject(ref byteReader);
                }

                private static T DeserializeInternal<T>(ref ByteReader byteReader, SerializerOption serializerOption, Func<T> factory = null)
                {
                    serializerOption ??= SerializerOption.Default;
                    return serializerOption.GetTypeInfo(factory).Deserialize(ref byteReader);
                }
            }
        }    
    }
}
