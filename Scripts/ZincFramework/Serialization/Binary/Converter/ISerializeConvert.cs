using System;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public interface ISerializeConvert : IBuilder
            {
                object Convert(byte[] bytes, ref int nowIndex, Type type);

                object Convert(ref ReadOnlySpan<byte> bytes, Type type);
            }

            public interface ISerializeConvert<out T> : IBuilder
            {
                T Convert(byte[] bytes, ref int nowIndex);
                
                T Convert(ref ReadOnlySpan<byte> bytes);
            }
        }
    }
}