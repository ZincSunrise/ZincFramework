using System;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Exceptions
        {
            public class NonSerializableAttributeException : Exception
            {
                public override string Message => _message ?? "��û�и��������ZincSerializable���ԣ���ȥ��ӣ�";

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