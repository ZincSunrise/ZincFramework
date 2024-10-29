using ZincFramework.Binary;

namespace ZincFramework
{
    namespace Network
    {
        namespace Protocol
        {
            public class QuitMessage : EmptyMessage
            {
                public override int SerializableCode => 100001;
            }
        }
    }
}
