using System;
using System.Collections;
using ZincFramework.Binary;
using ZincFramework.Serialization.Factory;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct DictionaryAppender : ISerializeAppend
            {
                public void Append(object obj, Type type, byte[] buffer, ref int nowIndex)
                {
                    Append(obj, type, buffer.AsSpan(), ref nowIndex);
                }

                public void Append(object obj, Type type, Span<byte> buffer, ref int nowIndex)
                {
                    IDictionary dictionary = obj as IDictionary;
                    ByteAppender.AppendInt16((short)dictionary.Count, buffer, ref nowIndex);
                    
                    Type[] types = type.GenericTypeArguments;
                    Type keyType = types[0];
                    Type valueType = types[1];

                    ISerializeAppend keyAppender = AppenderFactory.Shared.CreateBuilder(keyType);
                    ISerializeAppend valueAppender = AppenderFactory.Shared.CreateBuilder(valueType);

                    foreach (var key in dictionary.Keys)
                    {
                        keyAppender.Append(key, keyType, buffer, ref nowIndex);
                        valueAppender.Append(dictionary[key] ?? throw new ArgumentException("字典中不允许传入空键"), valueType, buffer, ref nowIndex);
                    }
                }
            }   
        }
    }
}

