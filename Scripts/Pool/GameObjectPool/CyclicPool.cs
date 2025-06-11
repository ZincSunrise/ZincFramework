using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.DataPools;


namespace ZincFramework.Pool.GameObjects
{
    /// <summary>
    /// 循环对象池，当对象池达到最大数量，会拿出第一个正在使用队列中的第一个游戏对象来使用
    /// 适用于子弹, 特效等能够已经确定好出生顺序的物体
    /// </summary>
    internal sealed class CyclicPool : GameObjectPool
    {
        public override IEnumerable UsingObjects => _usingObjects;

        private LinkedList<ReuseableObject> _usingObjects;

        private LinkedListNodePool<ReuseableObject> _nodePool = new LinkedListNodePool<ReuseableObject>();

        public CyclicPool(GameObject prefab, int maxCount = -1, GameObject rootObject = null) : base(prefab, maxCount, rootObject)
        {
            _usingObjects = new LinkedList<ReuseableObject>();
        }

        public override ReuseableObject RentValue()
        {
            ReuseableObject reuseableObject;

            if (CacheCount > 0)
            {
                reuseableObject = _unuseValues.Dequeue();
            }
            else if (MaxCount != -1 && _usingObjects.Count >= MaxCount)
            {
                var firstNode = _usingObjects.First;
                reuseableObject = firstNode.Value;

                _usingObjects.RemoveFirst();
                reuseableObject.OnReturn();
                reuseableObject.gameObject.SetActive(false);

                //将该链表节点还回
                _nodePool.ReturnValue(firstNode);
            }
            else
            {
                reuseableObject = _factory.Invoke();
            }

            var usingNode = _nodePool.RentValue(reuseableObject);
            _usingObjects.AddLast(usingNode);

            reuseableObject.OnRent();
            reuseableObject.gameObject.SetActive(true);

            if (RootObject != null)
            {
                reuseableObject.transform.SetParent(RootObject.transform);
            }

            return reuseableObject;
        }

        public override void ReturnValue(ReuseableObject reusableGameObject)
        {
            reusableGameObject.gameObject.SetActive(false);
            reusableGameObject.OnReturn();
            _unuseValues.Enqueue(reusableGameObject);

            var node = _usingObjects.Find(reusableGameObject);
            _usingObjects.Remove(reusableGameObject);
            _nodePool.ReturnValue(node);
        }
        public override void ReturnAll()
        {
            while (_usingObjects.Count > 0) 
            {
                ReturnValue(_usingObjects.First.Value);  
            }
        }

        public override void Clear()
        {
            base.Clear();
            _usingObjects.Clear();
        }

        public override void Dispose()
        {
            foreach (ReuseableObject reuseableObject in _usingObjects) 
            {
                if(reuseableObject != null)
                {
                    GameObject.Destroy(reuseableObject.gameObject);
                }
            }

            _usingObjects.Clear();

            while (_unuseValues.Count > 0)
            {
                var unuseObject = _unuseValues.Dequeue();
                if (unuseObject != null)
                {
                    GameObject.Destroy(unuseObject.gameObject);
                }
            }

            base.Dispose();
            _usingObjects = null;
            _unuseValues = null;
        }
    }
}