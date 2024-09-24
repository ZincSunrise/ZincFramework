using System;


namespace ZincFramework
{
    namespace MVC
    {
        namespace Exceptions
        {
            public class CommandException : Exception
            {
                public override string Message => _message ?? "�������������";

                private readonly string _message;

                public CommandException(string message)
                {
                    _message = message;
                }

                public CommandException()
                {

                }
            }
        }
    }
}
