using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ZincFramework.LoadServices.Resource
{
    public readonly struct ResourceRequestAwaiter : INotifyCompletion, ICriticalNotifyCompletion
    {
        private readonly ResourceRequest _resourceRequest;

        public bool IsCompleted => _resourceRequest.isDone;

        public ResourceRequestAwaiter(ResourceRequest resourceRequest) => _resourceRequest = resourceRequest;   

        public void OnCompleted(Action continuation)
        {
            if (_resourceRequest.isDone) 
            {
                continuation.Invoke();
            }
            else
            {
                _resourceRequest.completed += x => continuation.Invoke();
            }
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            if (_resourceRequest.isDone)
            {
                continuation.Invoke();
            }
            else
            {
                _resourceRequest.completed += x => continuation.Invoke();
            }
        }

        public UnityEngine.Object GetResult()
        {
            return _resourceRequest.asset;
        }
    }
}