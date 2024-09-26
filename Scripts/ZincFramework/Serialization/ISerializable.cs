using ZincFramework.Binary.Serialization;

namespace ZincFramework
{
    namespace Serialization
    {
        [ZincSerializable()]
        public interface ISerializable
        {
            void Write(ByteWriter byteWriter);

            void Read(ref ByteReader byteReader);
        }


        [ZincSerializable()]
        public interface ISerializable<out T> : ISerializable
        {

        }
    }
}

