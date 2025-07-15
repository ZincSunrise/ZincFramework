using UnityEngine;
using ZincFramework.Runtime.CompilerServices;

namespace ZincFramework.Threading.Tasks
{
    public static class AsyncOperationExtension 
    {
        public static AsyncOperationAwaiter GetAwaiter(this AsyncOperation asyncOperation)
        {
            return new AsyncOperationAwaiter(asyncOperation);
        }
    }
}