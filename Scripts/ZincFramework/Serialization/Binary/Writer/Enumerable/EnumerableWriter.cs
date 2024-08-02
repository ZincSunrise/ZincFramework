using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using ZincFramework.Binary;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct EnumerableWriter : ISerializeWrite
            {
                public void Write(object obj, Stream stream, Type type)
                {
                    switch (obj)
                    {
                        case string value:
                            ByteWriter.WriteString(value, stream);
                            break;
                        case IList:
                            new ArrayListWriter().Write(obj as IList, stream, type.IsArray ? type.GetElementType() : type.GenericTypeArguments[0]);
                            break;
                        case IDictionary:
                            new DictionaryWriter().Write(obj, stream, type);
                            break;
                        default:
                            new CollectionWriter().Write(obj as IEnumerable, stream, type.GenericTypeArguments[0]);
                            break;
                    }
                }

                public async Task WriteAsync(object obj, Stream stream, Type type)
                {
                    switch (obj)
                    {
                        case string value:
                            await ByteWriter.WriteStringAsync(value, stream);
                            break;
                        case IList:
                            await new ArrayListWriter().WriteAsync(obj as IList, stream, type.IsArray ? type.GetElementType() : type.GenericTypeArguments[0]);
                            break;
                        case IDictionary:
                            await new DictionaryWriter().WriteAsync(obj, stream, type);
                            break;
                        default:
                            await new CollectionWriter().WriteAsync(obj as IEnumerable, stream, type.GenericTypeArguments[0]);
                            break;
                    }
                }
            }
        }
    }
}