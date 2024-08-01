using System;
using ZincFramework.Binary;




namespace ZincFramework
{
    namespace Network
    {
        namespace Protocol
        {
            public readonly struct HeadInfo
            {
                public int SerializeCode { get; }

                //不包含头文件的字节长度
                public int Length { get; }

                public static int HeadLength => 8;

                public HeadInfo(int serializeCode, int length) 
                {
                    SerializeCode = serializeCode;
                    Length = length;
                }

                public readonly void AppendHeadInfo(byte[] bytes, ref int nowIndex)
                {
                    ByteAppender.AppendInt32(SerializeCode, bytes, ref nowIndex);
                    ByteAppender.AppendInt32(Length, bytes, ref nowIndex);
                }

                public readonly void AppendHeadInfo(Span<byte> bytes, ref int nowIndex)
                {
                    ByteAppender.AppendInt32(SerializeCode, bytes, ref nowIndex);
                    ByteAppender.AppendInt32(Length, bytes, ref nowIndex);
                }

                public static HeadInfo ConvertHeadInfo(byte[] buffer, ref int nowIndex)
                {
                    int serializeCode = ByteConverter.ToInt32(buffer, ref nowIndex);
                    int length = ByteConverter.ToInt32(buffer, ref nowIndex);
                    return new HeadInfo(serializeCode, length);
                }
            }
        }
    }
}