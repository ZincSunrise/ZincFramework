using ZincFramework.Loop;
using ZincFramework.Runtime.CompilerServices;

namespace ZincFramework.Threading.Tasks
{
    public readonly struct YieldAwaitable
    {
        public E_LoopType LoopType { get; }

        public YieldAwaitable(E_LoopType loopType)
        {
            LoopType = loopType;
        }

        public YieldAwaiter GetAwaiter()
        {
            return new YieldAwaiter(LoopType);
        }
    }
}