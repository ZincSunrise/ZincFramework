using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using ZincFramework.Binary;
using ZincFramework.Serialization.Factory;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct DictionaryWriter : ISerializeWrite
            {
                public void Write(object obj, Stream stream, Type type)
                {
                    IDictionary dictionary = obj as IDictionary;
                    ByteWriter.WriteInt16((short)dictionary.Count, stream);

                    Type[] types = type.GenericTypeArguments;
                    Type keyType = types[0];
                    Type valueType = types[1];

                    ISerializeWrite keyWriter = WriterFactory.Shared.CreateBuilder(keyType);
                    ISerializeWrite valueWriter = WriterFactory.Shared.CreateBuilder(valueType);

                    foreach (object key in dictionary.Keys)
                    {
                        keyWriter.Write(key, stream, type);
                        valueWriter.Write(dictionary[key], stream, type);
                    }
                }

                public async Task WriteAsync(object obj, Stream stream, Type type)
                {
                    IDictionary dictionary = obj as IDictionary;
                    ByteWriter.WriteInt16((short)dictionary.Count, stream);

                    Type[] types = type.GenericTypeArguments;
                    Type keyType = types[0];
                    Type valueType = types[1];

                    ISerializeWrite keyWriter = WriterFactory.Shared.CreateBuilder(keyType);
                    ISerializeWrite valueWriter = WriterFactory.Shared.CreateBuilder(valueType);

                    foreach (object key in dictionary.Keys)
                    {
                        await keyWriter.WriteAsync(key, stream, type);
                        await valueWriter.WriteAsync(dictionary[key], stream, type);
                    }
                }
            }   
        }
    }
}

