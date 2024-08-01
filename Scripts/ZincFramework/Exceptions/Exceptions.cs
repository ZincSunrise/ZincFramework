using System;


namespace ZincFramework
{
    namespace Exceptions
    {
        public class NonIGetSerializeIDException : Exception
        {
            public override string Message => "该类没有继承IGetSerializeID接口，请去继承！";
            public NonIGetSerializeIDException() { }
        }


        public class InvalidValueException : Exception
        {
            public override string Message => _message ?? "此类值不合法！";

            private readonly string _message;
            public InvalidValueException() { }

            public InvalidValueException(string message) 
            {
                _message = message;
            }
        }
    }
}

