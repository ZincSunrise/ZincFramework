using System;
using ZincFramework.Binary;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct BooleanAppender : ISerializeAppend<bool>
            {
                public void Append(bool t, byte[] buffer, ref int nowIndex)
                {
                    ByteAppender.AppendBoolean(t, buffer, ref nowIndex);
                }

                public void Append(bool t, Span<byte> buffer, ref int nowIndex)
                {
                    ByteAppender.AppendBoolean(t, buffer, ref nowIndex);
                }

                void ISerializeAppend.Append(object obj, Type type, byte[] buffer, ref int nowIndex)
                {
                    throw new NotImplementedException();
                }

                void ISerializeAppend.Append(object obj, Type type, Span<byte> buffer, ref int nowIndex)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}