using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ZincFramework.Pool.GameObjects
{
    /// <summary>
    /// 不循环对象池，当对象池达到最大数量，不再创建新的对象
    /// 用哈希表追踪元素
    /// </summary>
    public sealed class NonCyclicPool : GameObjectPool
    {
        public override IEnumerable UsingObjects => _usingObjects;

        private HashSet<ReuseableObject> _usingObjects = new HashSet<ReuseableObject>();

        /// <summary>
        /// 如果maxCount为-1则代表没有最大容量
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="maxCount"></param>
        /// <param name="rootObject"></param>
        public NonCyclicPool(GameObject prefab, int maxCount = -1, GameObject rootObject = null) : base(prefab, maxCount, rootObject)
        {

        }

        public override ReuseableObject RentValue()
        {
            ReuseableObject reuseableObject;

            if (MaxCount != -1 && _usingObjects.Count >= MaxCount)
            {
                return null;
            }

            if (CacheCount > 0)
            {
                reuseableObject = _unuseValues.Dequeue();
            }
            else
            {
                reuseableObject = _factory.Invoke();
            }

            _usingObjects.Add(reuseableObject);

            reuseableObject.OnRent();
            reuseableObject.gameObject.SetActive(true);

            if(RootObject != null)
            {
                reuseableObject.transform.SetParent(RootObject.transform);
            }

            return reuseableObject;
        }

        public override void ReturnAll()
        {
            foreach(ReuseableObject reuseableObject in _usingObjects)
            {
                reuseableObject.gameObject.SetActive(false);
                reuseableObject.OnReturn();
                _unuseValues.Enqueue(reuseableObject);
            }

            _usingObjects.Clear();
        }

        public override void ReturnValue(ReuseableObject reusableGameObject)
        {
            base.ReturnValue(reusableGameObject);

            if (_usingObjects.Remove(reusableGameObject))
            {
                _unuseValues.Enqueue(reusableGameObject);
            }
        }

        public override void Clear()
        {
            base.Clear();
            _usingObjects.Clear();
        }

        public override void Dispose()
        {
            foreach(var reuseableObject in _usingObjects)
            {
                if(reuseableObject != null)
                {
                    GameObject.Destroy(reuseableObject.gameObject);
                }
            }

            _usingObjects.Clear();

            while (_unuseValues.Count > 0)
            {
                var reuseableObject = _unuseValues.Dequeue();
                if(reuseableObject != null)
                {
                    GameObject.Destroy(reuseableObject.gameObject);
                }
            }

            _usingObjects = null;
            _unuseValues = null;
            base.Dispose();
        }
    }
}