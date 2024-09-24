using System;



namespace ZincFramework
{
    namespace DataPool
    {
        public class DataPool : ObjectPool<IResumable>
        {
            public DataPool(Func<IResumable> factory) : base(factory) { }

            public override IResumable RentValue()
            {
                IResumable reuseable = base.RentValue();
                reuseable.OnRent();
                return reuseable;
            }

            public override void ReturnValue(IResumable value)
            {
                value.OnReturn();
                base.ReturnValue(value);        
            }
        }

        public class DataPool<T> : ObjectPool<T> where T : IResumable
        {
            public DataPool(Func<T> factory) : base(factory) { }

            public override T RentValue()
            {
                T value = base.RentValue();
                value.OnRent();
                return value;
            }

            public override void ReturnValue(T value)
            {
                value.OnReturn();
                base.ReturnValue(value);
            }
        }
    }
}
