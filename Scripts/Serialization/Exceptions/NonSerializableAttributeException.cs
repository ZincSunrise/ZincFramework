using System;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Exceptions
        {
            public class NonSerializableAttributeException : Exception
            {
                public override string Message => _message ?? "你没有给该类添加ZincSerializable特性，请去添加！";

                private readonly string _message;
                public NonSerializableAttributeException() { }

                public NonSerializableAttributeException(string message)
                {
                    _message = message;
                }
            }

        }
    }
}