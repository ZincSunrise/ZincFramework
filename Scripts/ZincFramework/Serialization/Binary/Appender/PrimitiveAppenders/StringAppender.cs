using System;
using ZincFramework.Binary;

namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct StringAppender : ISerializeAppend<string>
            {
                public void Append(string str, byte[] buffer, ref int nowIndex)
                {
                    ByteAppender.AppendString(str, buffer, ref nowIndex);
                }

                public void Append(string str, Span<byte> buffer, ref int nowIndex)
                {
                    ByteAppender.AppendString(str, buffer, ref nowIndex);
                }

                void ISerializeAppend.Append(object obj, Type type, byte[] buffer, ref int nowIndex)
                {
                    ByteAppender.AppendString((string)obj, buffer, ref nowIndex);
                }

                void ISerializeAppend.Append(object obj, Type type, Span<byte> buffer, ref int nowIndex)
                {
                    ByteAppender.AppendString((string)obj, buffer, ref nowIndex);
                }
            }
        }
    }
}
