using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ZincFramework.Threading.Tasks;


namespace ZincFramework.Runtime.CompilerServices
{
    public readonly struct ZincAwaiter<T> : ICriticalNotifyCompletion, INotifyCompletion
    {
        public bool IsCompleted
        {
            get 
            {
                Debug.Assert(_task.TaskSource != null);
                return _task.TaskSource.GetStatus() != ZincTaskStatus.Executing;
            }
        }

        public T Result => _task.TaskSource.GetResult();


        private readonly ZincTask<T> _task;

        public ZincAwaiter(in ZincTask<T> task)
        {
            _task = task;
        }

        public T GetResult() => Result;

        public void OnCompleted(Action continuation)
        {
            _task.TaskSource.OnComplete(ContinuationAction.InvokeActionDelegate, continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _task.TaskSource.OnComplete(ContinuationAction.InvokeActionDelegate, continuation);
        }
    }
}
