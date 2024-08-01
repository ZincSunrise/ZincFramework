using System;
using System.IO;
using ZincFramework.Serialization.Factory;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public static class StreamSerializer
            {
                public static void Serialize(Stream stream, object serializableObject, Type type, SerializeConfig serializeConfig)
                {
                    SerializeInternal(stream, serializableObject, type, serializeConfig);
                }

                private static void SerializeInternal(Stream stream, object serializableObject, Type type, SerializeConfig serializeConfig)
                {
                    ISerializeWrite writer = WriterFactory.Shared.CreateBuilder(E_Builder_Type.OtherValue);
                    writer.Write(serializableObject, stream, type);
                }


                public static T Deserialize<T>(Stream stream, SerializeConfig serializeConfig)
                {
                    byte[] buffer = new byte[stream.Length];

                    if (stream is MemoryStream memoryStream)
                    {
                        buffer = memoryStream.ToArray();
                    }
                    else
                    {
                        stream.Read(buffer, 0, buffer.Length);
                        stream.Close();
                        stream.Dispose();
                    }


                    return BinarySerializer.Deserialize<T>(buffer, serializeConfig);
                }
            }
        }
    }
}

