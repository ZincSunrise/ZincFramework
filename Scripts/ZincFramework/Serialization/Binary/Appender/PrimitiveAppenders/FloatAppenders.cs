using System;
using ZincFramework.Binary;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Binary
        {
            public readonly struct FloatAppender : ISerializeAppend<float>
            {
                public void Append(float value, byte[] buffer, ref int nowIndex)
                {
                    ByteAppender.AppendFloat(value, buffer, ref nowIndex);
                }

                public void Append(float value, Span<byte> buffer, ref int nowIndex)
                {
                    ByteAppender.AppendFloat(value, buffer, ref nowIndex);
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

            public readonly struct DoubleAppender : ISerializeAppend<double>
            {
                public void Append(double value, byte[] buffer, ref int nowIndex)
                {
                    ByteAppender.AppendDouble(value, buffer, ref nowIndex);
                }

                public void Append(double value, Span<byte> buffer, ref int nowIndex)
                {
                    ByteAppender.AppendDouble(value, buffer, ref nowIndex);
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