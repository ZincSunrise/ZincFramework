using System;


namespace ZincFramework
{
    namespace Exceptions
    {
        public class NonIGetSerializeIDException : Exception
        {
            public override string Message => "����û�м̳�IGetSerializeID�ӿڣ���ȥ�̳У�";
            public NonIGetSerializeIDException() { }
        }


        public class InvalidValueException : Exception
        {
            public override string Message => _message ?? "����ֵ���Ϸ���";

            private readonly string _message;
            public InvalidValueException() { }

            public InvalidValueException(string message) 
            {
                _message = message;
            }
        }
    }
}

