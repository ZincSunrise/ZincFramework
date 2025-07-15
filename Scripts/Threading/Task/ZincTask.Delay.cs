using System.Threading;
using ZincFramework.Runtime.CompilerServices;

namespace ZincFramework.Threading.Tasks
{
    public readonly partial struct ZincTask
    {
        public static ZincTask Delay(int millisecondsDelay, CancellationToken cancellationToken)
        {
            ITaskSource delaySource = DelayPromise.Create(millisecondsDelay, cancellationToken);
            return new ZincTask(delaySource);
        }

        public static ZincTask Delay(int millisecondsDelay)
        {
            ITaskSource delaySource = DelayPromise.Create(millisecondsDelay, default);
            return new ZincTask(delaySource);
        }
    }
}
