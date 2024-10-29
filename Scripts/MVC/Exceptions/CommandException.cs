using System;


namespace ZincFramework
{
    namespace MVC
    {
        namespace Exceptions
        {
            public class CommandException : Exception
            {
                public override string Message => _message ?? "传入的命令有误";

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
