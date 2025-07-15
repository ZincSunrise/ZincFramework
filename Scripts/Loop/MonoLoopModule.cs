using System;
using ZincFramework.Loop.Internal;

namespace ZincFramework.Loop
{
    public class MonoLoopModule : ILoopModule<IMonoObserver>
    {
        private readonly LoopArray _loopItemArray = new LoopArray();

        public E_LoopType LoopType { get; }

        public Type FlagType { get; }

        public MonoLoopModule(E_LoopType loopType) 
        {
            LoopType = loopType;
            FlagType = ZincLoopSystem.SelectUpdateType(loopType);
        }

        public void Register(IMonoObserver monoObserver)
        {
            monoObserver.OnRegist();
            _loopItemArray.Register(monoObserver);
        }

        public void Unregister(IMonoObserver monoObserver)
        {
            monoObserver.OnRemove();
            _loopItemArray.Unregister(monoObserver);
        }

        public void Tick()
        {
            _loopItemArray.Tick();
        }

        public void Clear()
        {
            _loopItemArray.Clear();
        }
    }
}
