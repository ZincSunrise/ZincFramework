using System;
using System.Collections.Generic;

namespace ZincFramework
{
    namespace DataPool
    {
        public class DataPoolManager : BaseSafeSingleton<DataPoolManager>
        {
            private readonly Dictionary<Type, ObjectPool<IResetInfo>> _poolDic = new Dictionary<Type, ObjectPool<IResetInfo>>();
            //得到某一个脚本对象相关
            private DataPoolManager() 
            {

            }

            public void ReturnInfo<T>(T data, bool isReset = true) where T : class, IResetInfo, new()
            {
                if (_poolDic.TryGetValue(typeof(T), out var cachePool))
                {
                    if (isReset) 
                    {
                        data.ResetInfo();
                    }
                    cachePool.ReturnValue(data);
                }
            }

            public T RentInfo<T>() where T : class, IResetInfo, new()
            {
                Type type = typeof(T);
                if (!_poolDic.TryGetValue(type, out var cachePool))
                {
                    cachePool = new ObjectPool<IResetInfo>(() =>
                    {
                        return new T();
                    });
                    _poolDic.Add(type, cachePool);
                }

                return cachePool.RentValue() as T;
            }

            public T RentInfo<T>(Func<T> createFunc) where T : class, IResetInfo
            {
                Type type = typeof(T);
                if (!_poolDic.TryGetValue(type, out var cachePool))
                {
                    cachePool = new ObjectPool<IResetInfo>(createFunc);
                    _poolDic.Add(type, cachePool);
                }

                return cachePool.RentValue() as T;
            }

            public void ClearAllInfo()
            {
                foreach (var item in _poolDic.Values)
                {
                    item.Clear();
                }
                _poolDic.Clear();
            }
        }
    }
}

