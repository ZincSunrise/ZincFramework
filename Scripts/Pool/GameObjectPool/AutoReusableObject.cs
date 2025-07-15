using ZincFramework.Loop;

namespace ZincFramework.Pools.GameObjects
{
    public abstract class AutoReusableObject : ReuseableObject
    {
        public abstract float ReturnOffset { get; }

        private AutoReturnObserver _autoReturnObserver;

        public override void OnRent()
        {
            _autoReturnObserver ??= new AutoReturnObserver(this, ReturnOffset);
            ZincLoopSystem.AddFixedUpdateObserver(_autoReturnObserver);
        }

        public override void OnReturn()
        {
            ZincLoopSystem.RemoveFixedUpdateObserver(_autoReturnObserver);
        }

        protected virtual void OnDestroy()
        {
            ZincLoopSystem.RemoveFixedUpdateObserver(_autoReturnObserver);
        }
    }
}