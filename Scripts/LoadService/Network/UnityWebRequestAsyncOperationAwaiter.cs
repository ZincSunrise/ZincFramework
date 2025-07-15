using System;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;


namespace ZincFramework.LoadServices.Network
{
    public readonly struct UnityWebRequestAsyncOperationAwaiter : INotifyCompletion
    {
        public readonly UnityWebRequestAsyncOperation _unityWebRequestAsyncOperation;

        public bool IsCompleted => _unityWebRequestAsyncOperation.isDone;

        public UnityWebRequestAsyncOperationAwaiter(UnityWebRequestAsyncOperation unityWebRequestAsyncOperation)
        {
            _unityWebRequestAsyncOperation = unityWebRequestAsyncOperation;
        }

        public void OnCompleted(Action continuation)
        {
            if (IsCompleted)
            {
                continuation.Invoke();
            }
            else
            {
                _unityWebRequestAsyncOperation.completed += x => continuation.Invoke();
            }
        }

        public void GetResult()
        {

        }
    }
}