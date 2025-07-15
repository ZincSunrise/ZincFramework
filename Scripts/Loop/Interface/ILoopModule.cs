using System;

namespace ZincFramework.Loop
{
    /// <summary>
    /// 循环类型，目前只使用了简洁版本
    /// </summary>
    public enum E_LoopType
    {
        Initialization = 0,
        FixedUpdate = 1,
        Update = 2,
        PreLateUpdate = 3,
        EndOfFrame = 4,

        //EarlyUpdate = 1,
        //PreUpdate = 3,
        //PostLateUpdate = 6,
    }

    public interface ILoopModule
    {
        E_LoopType LoopType { get; }

        Type FlagType { get; }

        void Register(ILoopItem loopItem);

        void Unregister(ILoopItem loopItem);

        void Tick();

        void Clear();
    }


    public interface ILoopModule<TLoopItem> : ILoopModule where TLoopItem : ILoopItem 
    {
        void ILoopModule.Register(ILoopItem loopItem) => Register((TLoopItem)loopItem);

        void ILoopModule.Unregister(ILoopItem loopItem) => Unregister((TLoopItem)loopItem);

        void Register(TLoopItem loopItem);

        void Unregister(TLoopItem loopItem);
    }
}
