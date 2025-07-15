using System.Runtime.CompilerServices;
using System;
using ZincFramework.Loop;
using ZincFramework.Threading.Tasks;

namespace ZincFramework.Runtime.CompilerServices
{
    public readonly struct YieldAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        public bool IsCompleted => false;

        public E_LoopType LoopType { get; }

        public YieldAwaiter(E_LoopType loopType) => LoopType = loopType;

        public void GetResult()
        {

        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            ZincTaskLoopHelper.AddContinuation(LoopType, continuation);
        }
    }
}