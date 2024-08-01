using ZincFramework.Binary;

namespace ZincFramework
{
    namespace Network
    {
        namespace Protocol
        {
            public record QuitMessage : BaseMessage
            {
                public override int SerializableCode => 100001;

                public override byte[] Serialize()
                {
                    int length = GetBytesLength();
                    byte[] bytes = new byte[length];
                    int nowIndex = 0;

                    ByteAppender.AppendInt32(SerializableCode, bytes, ref nowIndex);
                    ByteAppender.AppendInt32(GetBytesLength(), bytes, ref nowIndex);
                    return bytes;
                }

                public override void Deserialize(byte[] bytes)
                {
                    return;
                }

                public override int GetBytesLength()
                {
                    return 8;
                }
            }
        }
    }
}
