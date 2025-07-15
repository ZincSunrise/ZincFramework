using System;


namespace ZincFramework.Pools
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

#if UNITY_EDITOR
        private static bool IsAllowDebug => FrameworkConsole.Instance.SharedData.isDebug;
#endif

        public override T RentValue()
        {
            T value = base.RentValue();
            value.OnRent();

#if UNITY_EDITOR
            if (IsAllowDebug)
            {
                UnityEngine.Debug.Log($"还剩下{_unuseValues.Count}未借出，类型为{typeof(T).Name}，借出的哈希码为{value.GetHashCode()}");
            }
#endif
            return value;
        }

        public override void ReturnValue(T value)
        {
#if UNITY_EDITOR
            if (IsAllowDebug)
            {
                foreach (var item in _unuseValues)
                {
                    if (item.Equals(value))
                    {
                        UnityEngine.Debug.LogWarning("你借出了相同的物体!");
                        break;
                    }
                }
            }
#endif
            try
            {
                value.OnReturn();
            }
            finally
            {
                base.ReturnValue(value);

#if UNITY_EDITOR
                if (IsAllowDebug)
                {
                    UnityEngine.Debug.Log($"归还后还剩下{_unuseValues.Count}，类型为{typeof(T).Name}，归还的哈希码为{value.GetHashCode()}");
                }
#endif
            }
        }
    }
}
