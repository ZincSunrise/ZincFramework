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
                    _message = "被标记为过时的成员不可以更改其原来的序号" + originNumber.ToString();
                }
            }
        }
    }
}
