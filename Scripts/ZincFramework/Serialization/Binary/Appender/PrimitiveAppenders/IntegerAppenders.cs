using System;
using ZincFramework.Binary;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct Int16Appender : ISerializeAppend<short>
            {
                public void Append(short value, byte[] buffer, ref int nowIndex)
                {
                    ByteAppender.AppendInt16(value, buffer, ref nowIndex);
                }

                public void Append(short value, Span<byte> buffer, ref int nowIndex)
                {
                    ByteAppender.AppendInt16(value, buffer, ref nowIndex);
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

            public readonly struct Int32Appender : ISerializeAppend<int>
            {
                public void Append(int value, byte[] buffer, ref int nowIndex)
                {
                    ByteAppender.AppendInt32(value, buffer, ref nowIndex);
                }

                public void Append(int value, Span<byte> buffer, ref int nowIndex)
                {
                    ByteAppender.AppendInt32(value, buffer, ref nowIndex);
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

            public readonly struct Int64Appender : ISerializeAppend<long>
            {
                public void Append(long value, byte[] buffer, ref int nowIndex)
                {
                    ByteAppender.AppendInt64(value, buffer, ref nowIndex);
                }

                public void Append(long value, Span<byte> buffer, ref int nowIndex)
                {
                    ByteAppender.AppendInt64(value, buffer, ref nowIndex);
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

            public readonly struct UInt16Appender : ISerializeAppend<ushort>
            {
                public void Append(ushort value, byte[] buffer, ref int nowIndex)
                {
                    ByteAppender.AppendUInt16(value, buffer, ref nowIndex);
                }

                public void Append(ushort value, Span<byte> buffer, ref int nowIndex)
                {
                    ByteAppender.AppendUInt16(value, buffer, ref nowIndex);
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

            public readonly struct UInt32Appender : ISerializeAppend<uint>
            {
                public void Append(uint value, byte[] buffer, ref int nowIndex)
                {
                    ByteAppender.AppendUInt32(value, buffer, ref nowIndex);
                }

                public void Append(uint value, Span<byte> buffer, ref int nowIndex)
                {
                    ByteAppender.AppendUInt32(value, buffer, ref nowIndex);
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

            public readonly struct UInt64Appender : ISerializeAppend<ulong>
            {
                public void Append(ulong value, byte[] buffer, ref int nowIndex)
                {
                    ByteAppender.AppendUInt64(value, buffer, ref nowIndex);
                }

                public void Append(ulong value, Span<byte> buffer, ref int nowIndex)
                {
                    ByteAppender.AppendUInt64(value, buffer, ref nowIndex);
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
