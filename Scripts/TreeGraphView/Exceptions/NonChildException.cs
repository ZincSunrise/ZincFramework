using System;


namespace ZincFramework.TreeService.Exceptions
{
    public class NonChildException : Exception
    {
        public override string Message => _message;

        private readonly string _message;

        public NonChildException(string message) => _message = message;
    }
}
