using System;

namespace ZincFramework
{
    namespace Serialization
    {
        [ZincSerializable()]
        public interface ISerializable
        {
            byte[] Serialize();

            void Deserialize(byte[] bytes);
        }


        [ZincSerializable()]
        public interface ISerializable<out T> : ISerializable
        {

        }
    }
}

