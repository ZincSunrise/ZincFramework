using System.Threading;
using ZincFramework.Loop;
using ZincFramework.Runtime.CompilerServices;

namespace ZincFramework.Threading.Tasks
{
    public readonly partial struct ZincTask
    {
        public static ZincTask WaitForSeconds(float waitTime)
        {
            return WaitForSeconds(waitTime, E_LoopType.Update, false, default);
        }

        public static ZincTask WaitForSeconds(float waitTime, E_LoopType loopType)
        {
            return WaitForSeconds(waitTime, loopType, false, default);
        }

        public static ZincTask WaitForSeconds(float waitTime, E_LoopType loopType, bool isRealTime, CancellationToken cancellationToken = default)
        {
            ITaskSource taskSource = WaitForSecondsPromise.Create(waitTime, loopType, isRealTime, cancellationToken);
            return new ZincTask(taskSource);
        }
    }
}