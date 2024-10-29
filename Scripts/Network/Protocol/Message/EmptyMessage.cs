using ZincFramework.Binary.Serialization;


namespace ZincFramework.Network.Protocol
{
    public abstract class EmptyMessage : BaseMessage
    {
        public override int GetBytesLength()
        {
            return 8;
        }

        public override void Write(ByteWriter byteWriter, SerializerOption serializerOption)
        {
            byteWriter.WriteInt32(SerializableCode);
            byteWriter.WriteInt32(GetBytesLength());
        }

        public override void Read(ref ByteReader byteReader, SerializerOption serializerOption)
        {

        }
    }
}
