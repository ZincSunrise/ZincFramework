namespace ZincFramework.Pool.GameObjects
{
    public abstract class AutoReuseableObject : ReuseableObject
    {
        public abstract float ReturnOffset { get; }

        private AutoReturnObserver _autoReturnObserver;

        public override void OnRent()
        {
            _autoReturnObserver ??= new AutoReturnObserver(this, ReturnOffset);
            MonoManager.Instance.AddFixedUpdateObserver(_autoReturnObserver);
        }

        public override void OnReturn()
        {
            MonoManager.Instance.RemoveFixedUpdateObserver(_autoReturnObserver);
        }

        protected virtual void OnDestroy()
        {
            MonoManager.Instance.RemoveFixedUpdateObserver(_autoReturnObserver);
        }
    }
}