using System;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Events;

namespace ZincFramework
{
    namespace DataPool
    {
        /// <summary>
        /// �����, ������������ĳһ�����
        /// </summary>
        public class ComponentPoolManager : BaseSafeSingleton<ComponentPoolManager>
        {
            private readonly Dictionary<Type, ComponentPool> _componentPoolInfos = new Dictionary<Type, ComponentPool>();

            private int _defaultMaxCount => FrameworkConsole.Instance.SharedData.maxPoolCount;
            private ComponentPoolManager() { }

            public T RentComponent<T>(int maxCount = -1) where T : Component
            {               
                Type type = typeof(T);
                if(!_componentPoolInfos.TryGetValue(type, out var compnentPool))
                {
                    compnentPool = new ComponentPool(() =>
                    {
                        return compnentPool.CompnentContainer.AddComponent<T>();
                    }, maxCount == -1 ? _defaultMaxCount : maxCount, type.Name);
                    _componentPoolInfos.Add(type, compnentPool);
                }

                compnentPool.MaxCount = maxCount == -1 ? _defaultMaxCount : maxCount;
                return compnentPool.RentValue() as T;
            }

            public void ReturnComponent<T>(T component, ZincAction<T> resetCallback = null) where T : Component
            {
                if (_componentPoolInfos.TryGetValue(typeof(T), out var compnentPool))
                {
                    resetCallback?.Invoke(component);
                    compnentPool.ReturnValue(component);
                }
            }

            public void Clear()
            {
                foreach (var compnent in _componentPoolInfos.Values)
                {
                    compnent.Dispose();
                }
                _componentPoolInfos.Clear();
            }
        }
    }
}

