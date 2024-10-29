using System;



namespace ZincFramework
{
    namespace DataPools
    {
        public class DataPool : ObjectPool<IReuseable>
        {
            public DataPool(Func<IReuseable> factory) : base(factory) { }

            public override IReuseable RentValue()
            {
                IReuseable reuseable = base.RentValue();
                reuseable.OnRent();
                return reuseable;
            }

            public override void ReturnValue(IReuseable value)
            {
                value.OnReturn();
                base.ReturnValue(value);        
            }
        }

        public class DataPool<T> : ObjectPool<T> where T : IReuseable
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
