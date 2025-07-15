using System.Threading;
using ZincFramework.Loop;

namespace ZincFramework.Threading.Tasks
{
    public partial struct ZincTask
    {
        public static ZincTask NextFrame()
        {
            var promise = NextFramePromise.Create(E_LoopType.Update, default);
            return new ZincTask(promise);
        }

        public static ZincTask NextFrame(E_LoopType loopType)
        {
            var promise = NextFramePromise.Create(loopType, default);
            return new ZincTask(promise);
        }

        public static ZincTask NextFrame(CancellationToken cancellationToken)
        {
            var promise = NextFramePromise.Create(E_LoopType.Update, cancellationToken);
            return new ZincTask(promise);
        }

        public static ZincTask NextFrame(E_LoopType loopType, CancellationToken cancellationToken)
        {
            var promise = NextFramePromise.Create(loopType, cancellationToken);
            return new ZincTask(promise);
        }
    }
}