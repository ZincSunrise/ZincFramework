namespace ZincFramework
{
    namespace Serialization
    {
        [BinarySerializable()]
        public interface ISerializable
        {
            byte[] Serialize();

            void Deserialize(byte[] bytes);
        }


        [BinarySerializable()]
        public interface ISerializable<out T> : ISerializable
        {

        }
    }
}

