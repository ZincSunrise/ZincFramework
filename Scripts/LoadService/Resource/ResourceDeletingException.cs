using System;

namespace ZincFramework.LoadServices.Resource
{
    public class ResourceDeletingException : Exception
    {
        public override string Message => _message ?? "�㲻��ʹ������ɾ������Դ";

        private readonly string _message;

        public ResourceDeletingException(string message) => _message = message;

        public ResourceDeletingException() { }
    }
}
