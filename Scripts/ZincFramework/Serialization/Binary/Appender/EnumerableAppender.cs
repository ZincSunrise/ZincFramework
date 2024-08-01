using System;
using System.Collections;
using ZincFramework.Binary;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct EnumerableAppender : ISerializeAppend
            {
                public void Append(object obj, Type type, byte[] buffer, ref int nowIndex)
                {
                    Append(obj, type, buffer.AsSpan(), ref nowIndex);
                }

                public void Append(object obj, Type type, Span<byte> buffer, ref int nowIndex)
                {
                    switch (obj)
                    {
                        case string str:
                            ByteAppender.AppendString(str, buffer, ref nowIndex);
                            return;
                        case IList list:
                            new ArrayListAppender().Append(list, type.IsArray ? type.GetElementType() : type.GenericTypeArguments[0], buffer, ref nowIndex);
                            break;
                        case IDictionary:
                            new DictionaryAppender().Append(obj, type, buffer, ref nowIndex);
                            break;
                        default:
                            new CollectionAppender().Append(obj as IEnumerable, type.GenericTypeArguments[0], buffer, ref nowIndex);
                            break;
                    }
                }
            }
        }
    }
}