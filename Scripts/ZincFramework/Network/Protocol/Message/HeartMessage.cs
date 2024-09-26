using ZincFramework.Binary.Serialization;



namespace ZincFramework
{
    namespace Network
    {
        namespace Protocol
        {
            public class HeartMessage : EmptyMessage
            {
                public override int SerializableCode => 100002;
            }
        }
    }
}