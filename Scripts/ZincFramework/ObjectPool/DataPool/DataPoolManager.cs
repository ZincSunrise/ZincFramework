using System;
using System.Collections.Generic;

namespace ZincFramework
{
    namespace DataPool
    {
        public static class DataPoolManager
        {
            private static readonly Dictionary<string, DataPool> _poolDic = new Dictionary<string, DataPool>();

            public static void ReturnInfo<T>(T data) where T : class, IResumable
            {
                string name = typeof(T).FullName;

                if (_poolDic.TryGetValue(name, out var cachePool))
                {
                    cachePool.ReturnValue(data);
                }
            }

            public static T RentInfo<T>() where T : class, IResumable, new()
            {
                return RentInfo(() => new T());
            }

            public static T RentInfo<T>(Func<T> createFunc) where T : class, IResumable
            {
                string name = typeof(T).FullName;

                if (!_poolDic.TryGetValue(name, out var cachePool))
                {
                    cachePool = new DataPool(createFunc);
                    _poolDic.Add(name, cachePool);
                }

                return cachePool.RentValue() as T;
            }


            public static void ClearAllInfo()
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

