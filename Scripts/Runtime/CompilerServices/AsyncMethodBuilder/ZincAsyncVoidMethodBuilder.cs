using System;
using System.Runtime.CompilerServices;
using ZincFramework.Threading.Tasks;

namespace ZincFramework.Runtime.CompilerServices
{
    public struct ZincAsyncVoidMethodBuilder
    {
        public ZincTask Task => _completionSource.ZincTask;

        public ZincCompletionSource _completionSource;

        public static ZincAsyncVoidMethodBuilder Create()
        {
            var builder = new ZincAsyncVoidMethodBuilder();
            builder._completionSource = ZincCompletionSource.Create();

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

        public void SetResult()
        {
            _completionSource.SetResult();
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
