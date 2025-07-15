using System;
using ZincFramework.Loop;
using ZincFramework.Loop.Internal;

namespace ZincFramework.Threading.Tasks.Internal
{
    internal class TimeRunnerModule : ILoopModule
    {
        public E_LoopType LoopType { get; }

        public Type FlagType { get; }


        public ConcurrentAutoLoopArray _loopArray = new ConcurrentAutoLoopArray();

        public TimeRunnerModule(E_LoopType loopType, Type flagType)
        {
            LoopType = loopType;
            FlagType = flagType;
        }

        public void Register(ILoopItem loopItem)
        {
            _loopArray.Register(loopItem);
        }

        void ILoopModule.Unregister(ILoopItem loopItem) 
        {
            throw new NotImplementedException("不允许移除");
        }

        public void Tick()
        {
            _loopArray.Tick();  
        }

        public void Clear()
        {
            _loopArray.Clear();
        }
    }
}