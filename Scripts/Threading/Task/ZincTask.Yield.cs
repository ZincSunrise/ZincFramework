using System.Threading;
using ZincFramework.Loop;
using ZincFramework.Runtime.CompilerServices;

namespace ZincFramework.Threading.Tasks
{
    public readonly partial struct ZincTask
    {
        public static YieldAwaitable Yield()
        {
            return new YieldAwaitable(E_LoopType.Update); 
        }

        public static YieldAwaitable Yield(E_LoopType loopType)
        {
            return new YieldAwaitable(loopType);
        }

        public static ZincTask Yield(CancellationToken cancellationToken)
        {
            ITaskSource taskSource = YieldPromise.Create(E_LoopType.Update, cancellationToken);
            return new ZincTask(taskSource);
        }

        public static ZincTask Yield(E_LoopType loopType, CancellationToken cancellationToken)
        {
            ITaskSource taskSource = YieldPromise.Create(loopType, cancellationToken);
            return new ZincTask(taskSource);
        }

        public static YieldAwaitable WaitForFixedUpdate()
        {
            return new YieldAwaitable(E_LoopType.FixedUpdate);
        }

        public static YieldAwaitable WaitForEndOfFrame()
        {
            return new YieldAwaitable(E_LoopType.EndOfFrame);
        }
    }
}
