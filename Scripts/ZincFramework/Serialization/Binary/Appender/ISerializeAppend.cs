using System;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public interface ISerializeAppend : IBuilder
            {
                void Append(object obj, Type type, byte[] buffer, ref int nowIndex);

                void Append(object obj, Type type, Span<byte> buffer, ref int nowIndex);
            }

            public interface ISerializeAppend<in T> : ISerializeAppend
            {
                void Append(T t, byte[] buffer, ref int nowIndex);

                void Append(T t, Span<byte> buffer, ref int nowIndex);
            }
            
            public interface ICollectionAppend<in T> : ISerializeAppend
            {
                void Append(T list, Type genericType, byte[] buffer, ref int nowIndex);

                void Append(T list, Type genericType, Span<byte> buffer, ref int nowIndex);
            }
        }
    }
}