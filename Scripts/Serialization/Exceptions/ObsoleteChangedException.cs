using System;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Exceptions
        {

            public class ObsoleteChangedException : Exception
            {
                public override string Message => _message;

                private readonly string _message;

                public ObsoleteChangedException(int originNumber) 
                {
                    _message = "�����Ϊ��ʱ�ĳ�Ա�����Ը�����ԭ�������" + originNumber.ToString();
                }
            }
        }
    }
}
