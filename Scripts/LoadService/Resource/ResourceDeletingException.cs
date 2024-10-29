using System;

namespace ZincFramework.LoadServices.Resource
{
    public class ResourceDeletingException : Exception
    {
        public override string Message => _message ?? "你不能使用正在删除的资源";

        private readonly string _message;

        public ResourceDeletingException(string message) => _message = message;

        public ResourceDeletingException() { }
    }
}
