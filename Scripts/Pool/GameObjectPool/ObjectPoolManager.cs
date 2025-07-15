using System;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Pools.GameObjects;


namespace ZincFramework.Pools
{
    public class ObjectPoolManager : BaseSafeSingleton<ObjectPoolManager>
    {
        private readonly Dictionary<string, GameObjectPool> _objectDic = new Dictionary<string, GameObjectPool>();

        public int DefaultMaxCount => 30;

        private ObjectPoolManager() { }


        /// <summary>
        /// 不循环对象池，如果达到最大容量不再会取出对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sourceObject"></param>
        /// <param name="maxCount">如果填入-1则代表没有最大容量限制</param>
        public void RegistNonCyclicPool(string name, GameObject sourceObject, int maxCount = 0, GameObject rootObject = null)
        {
#if UNITY_EDITOR
            if (_objectDic.ContainsKey(name))
            {
                Debug.LogWarning($"你替换了对象池中名字为{name}的对象");
            }
#endif
            if (!sourceObject.TryGetComponent<IReuseable>(out _))
            {
                throw new NotSupportedException("不支持不继承" + nameof(IReuseable) + "的类");
            }

            sourceObject.name = name;
            maxCount = maxCount == 0 ? DefaultMaxCount : maxCount;
            _objectDic[name] = new NonCyclicPool(sourceObject, maxCount, rootObject);
        }


        /// <summary>
        /// 循环对象池，如果达到最大容量，会将最早取出的对象重置并取出
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sourceObject"></param>
        /// <param name="maxCount"></param>
        public void RegistCyclicPool(string name, GameObject sourceObject, int maxCount = 0, GameObject rootObject = null)
        {
#if UNITY_EDITOR
            if (_objectDic.ContainsKey(name))
            {
                Debug.LogWarning($"你替换了对象池中名字为{name}的对象");
            }
#endif

            if (!sourceObject.TryGetComponent<IReuseable>(out _))
            {
                throw new NotSupportedException("不支持不继承" + nameof(IReuseable) + "的类");
            }

            sourceObject.name = name;
            maxCount = maxCount == 0 ? DefaultMaxCount : maxCount;
            _objectDic[name] = new CyclicPool(sourceObject, maxCount, rootObject);
        }

        /// <summary>
        /// 会删除所有在对象池之中的物体
        /// </summary>
        /// <param name="name"></param>
        public void UnRegistPool(string name)
        {
            if (_objectDic.TryGetValue(name, out GameObjectPool gameObjectPool))
            {
                gameObjectPool.Dispose();
                _objectDic.Remove(name);
            }
        }

        public void SetPoolMaxCount(string name, int maxCount)
        {
            if (_objectDic.TryGetValue(name, out GameObjectPool gameObjectPool))
            {
                gameObjectPool.MaxCount = maxCount;
            }
        }

        /// <summary>
        /// 检查是否有这个池子
        /// </summary>
        /// <param name="name"></param>
        public bool ContainsPool(string name) => _objectDic.ContainsKey(name);

        /// <summary>
        /// 清除某一个对象池
        /// </summary>
        /// <param name="name"></param>
        public void ClearPool(string name)
        {
            if (_objectDic.TryGetValue(name, out GameObjectPool gameObjectPool))
            {
                gameObjectPool.Clear();
            }
        }


        /// <summary>
        /// 借用之前记得先注册
        /// </summary>
        /// <param name="name"></param>
        /// <returns>不注册就会报错哦</returns>
        /// <exception cref="ArgumentException"></exception>
        public T RentReuseableObject<T>(string name) where T : ReuseableObject
        {
            if (!_objectDic.TryGetValue(name, out GameObjectPool gameObjectPool))
            {
                throw new ArgumentException($"你没有注册名字为{name}的对象");
            }

            return gameObjectPool.RentValue() as T;
        }

        /// <summary>
        /// 借用可回收对象
        /// </summary>
        /// <param name="reuseableObject"></param>
        /// <param name="resetCallback"></param>
        public void ReturnReusableGameObject(ReuseableObject reuseableObject, Action<GameObject> resetCallback = null)
        {
            resetCallback?.Invoke(reuseableObject.gameObject);
            _objectDic[reuseableObject.name].ReturnValue(reuseableObject);
        }


        /// <summary>
        /// 借用之前记得先注册
        /// </summary>
        /// <param name="name"></param>
        /// <returns>不注册就会报错哦</returns>
        /// <exception cref="ArgumentException"></exception>
        public GameObject RentGameObject(string name)
        {
            if (!_objectDic.TryGetValue(name, out GameObjectPool gameObjectPool))
            {
                throw new ArgumentException($"你没有注册名字为{name}的对象");
            }

            return gameObjectPool.RentValue().gameObject;
        }


        /// <summary>
        /// 不建议直接使用这个,更应该使用参数为ReuseableObject的
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="resetCallback"></param>
        public void ReturnGameObject(GameObject gameObject, Action<GameObject> resetCallback = null)
        {
            resetCallback?.Invoke(gameObject);
            _objectDic[gameObject.name].ReturnValue(gameObject.GetComponent<ReuseableObject>());
        }


        /// <summary>
        /// 回收不清除某一个对象池中的对象
        /// </summary>
        /// <param name="poolName"></param>
        public void PoolReturnAll(string poolName)
        {
            if (!_objectDic.TryGetValue(poolName, out GameObjectPool gameObjectPool))
            {
                throw new ArgumentException($"你没有注册名字为{poolName}的对象");
            }

            gameObjectPool.ReturnAll();
        }

        /// <summary>
        /// 回收但是不清除对象池中对象
        /// </summary>
        public void ReturnAll()
        {
            foreach (var pool in _objectDic.Values)
            {
                pool.ReturnAll();
            }
        }


        /// <summary>
        /// 清除对象池中的引用，但是不清除对象
        /// </summary>
        public void ClearAllPool()
        {
            foreach (var item in _objectDic.Values)
            {
                item.Clear();
            }
        }


        /// <summary>
        /// 清除对象池和对象池中的所有对象
        /// </summary>
        public void DisposeAllPool()
        {
            foreach (var item in _objectDic.Values)
            {
                item.Dispose();
            }

            _objectDic.Clear();
        }
    }
}

