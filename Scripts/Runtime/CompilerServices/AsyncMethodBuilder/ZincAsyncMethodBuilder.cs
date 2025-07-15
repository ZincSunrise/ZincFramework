using System;
using System.Runtime.CompilerServices;
using ZincFramework.Threading.Tasks;

namespace ZincFramework.Runtime.CompilerServices
{
    public struct ZincAsyncMethodBuilder<T> 
    {
        public ZincTask<T> Task => _completionSource.ZincTask;

        private ZincCompletionSource<T> _completionSource;

        public static ZincAsyncMethodBuilder<T> Create()
        {
            var builder = new ZincAsyncMethodBuilder<T>();
            builder._completionSource = ZincCompletionSource<T>.Create();

            return builder;
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        public void SetException(Exception exception)
        {
            _completionSource.SetException(exception);
        }

        public void SetResult(T result)
        {
            _completionSource.SetResult(result);
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
        }

        public void SetStateMachine(IAsyncStateMachine stateMechine)
        {

        }
    }
}
