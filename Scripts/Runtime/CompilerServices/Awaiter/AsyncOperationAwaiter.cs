using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using ZincFramework.Pools;

namespace ZincFramework.Runtime.CompilerServices
{
    public readonly struct AsyncOperationAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        public bool IsCompleted => _asyncOperation.isDone;

        private readonly AsyncOperation _asyncOperation;

        public AsyncOperationAwaiter(AsyncOperation asyncOperation)
        {
            _asyncOperation = asyncOperation;
        }

        public void GetResult() 
        {
            if (!_asyncOperation.isDone) 
            {
                throw new InvalidOperationException("不能在加载完成之前就获取结果");
            }
        }


        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _asyncOperation.completed += PooledDelegate<AsyncOperation>.Create(continuation);
        }
    }

}