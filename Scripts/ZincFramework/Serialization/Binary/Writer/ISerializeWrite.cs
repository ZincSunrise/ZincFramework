using System;
using System.IO;
using System.Threading.Tasks;

namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public interface ISerializeWrite : IBuilder
            {
                void Write(object obj, Stream stream, Type type);

                Task WriteAsync(object obj, Stream stream, Type type);
            }

            public interface ISerializeWrite<in T> : IBuilder
            {
                void Write(T obj, Stream stream);

                Task WriteAsync(T obj, Stream stream);
            }
            
            public interface ICollectionWrite<in T> : ISerializeWrite
            {
                void Write(T obj, Stream stream, Type genericType);

                Task WriteAsync(T obj, Stream stream, Type genericType);
            }
        }
    }
}