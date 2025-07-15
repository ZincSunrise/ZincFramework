using System;
using System.Runtime.CompilerServices;
using YooAsset;
using ZincFramework.Pools;


namespace ZincFramework.LoadServices.YooAsset
{
    public static class AsyncOperationBaseOperation
    {
        public static AsyncOperationBaseAwaiter GetAwaiter(this AsyncOperationBase asyncOperationBase)
        {
            return new AsyncOperationBaseAwaiter(asyncOperationBase);
        }
    }

    public readonly struct AsyncOperationBaseAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        public bool IsCompleted => _asyncOperationBase.IsDone;

        private readonly AsyncOperationBase _asyncOperationBase;

        public AsyncOperationBaseAwaiter(AsyncOperationBase asyncOperationBase)
        {
            _asyncOperationBase = asyncOperationBase;
        }

        public void GetResult()
        {

        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _asyncOperationBase.Completed += PooledDelegate<AsyncOperationBase>.Create(continuation);
        }
    }
}

