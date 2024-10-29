using ZincFramework.Binary.Serialization;

namespace ZincFramework
{
    namespace Serialization
    {
        [ZincSerializable()]
        public interface ISerializable
        {
            void Write(ByteWriter byteWriter, SerializerOption serializerOption);

            void Read(ref ByteReader byteReader, SerializerOption serializerOption);
        }


        [ZincSerializable()]
        public interface ISerializable<out T> : ISerializable
        {

        }
    }
}

