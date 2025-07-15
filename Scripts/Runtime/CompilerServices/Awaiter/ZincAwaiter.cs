using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ZincFramework.Threading.Tasks;


namespace ZincFramework.Runtime.CompilerServices
{
    public readonly struct ZincAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        public bool IsCompleted
        {
            get 
            {
                Debug.Assert(_task.TaskSource != null);
                return _task.TaskSource.GetStatus() != ZincTaskStatus.Executing;
            }
        }

        private readonly ZincTask _task;

        public ZincAwaiter(in ZincTask task)
        {
            _task = task;
        }

        public void GetResult()
        {
            _task.TaskSource.GetResult();
        }

        public void OnCompleted(Action continuation)
        {
            _task.TaskSource.OnComplete(ContinuationAction.InvokeActionDelegate, continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _task.TaskSource.OnComplete(ContinuationAction.InvokeActionDelegate, continuation);
        }

        public void OnForget()
        {
            UnsafeOnCompleted(GetResult);
        }

        public void SourceOnCompleted()
        {
            UnsafeOnCompleted(GetResult);
        }
    }
}
