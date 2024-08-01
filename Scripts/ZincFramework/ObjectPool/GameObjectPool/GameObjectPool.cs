using System.Collections.Generic;
using UnityEngine;
using System;
using ZincFramework.DataPool;


namespace ZincFramework
{
    namespace GameObjectPool
    {
        /// <summary>
        /// 存储对象池中游戏对象信息的类
        /// </summary>
        internal sealed class GameObjectPool : ObjectPool<GameObject>
        {
            private GameObject _poolRootObject;

            private Queue<GameObject> _usingObjects = new Queue<GameObject>();

            public GameObjectPool(Func<GameObject> addFunc, string name, GameObject rootObject = null) : base(addFunc)
            {
                if (rootObject != null)
                {
                    _poolRootObject = new GameObject(name);
                    _poolRootObject.transform.SetParent(rootObject.transform);
                }
            }

            public override void ReturnValue(GameObject gameObject)
            {
                if (_poolRootObject != null)
                {
                    gameObject.transform.SetParent(_poolRootObject.transform);
                }

                gameObject.SetActive(false);
                _cacheValues.Enqueue(gameObject);
                _usingObjects.Dequeue();
            }


            public override GameObject RentValue()
            {
                GameObject gameObject;

                if (CacheCount > 0)
                {
                    gameObject = _cacheValues.Dequeue();
                }
                else if (_usingObjects.Count >= MaxCount)
                {
                    gameObject = _usingObjects.Dequeue();
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject = _createFunction.Invoke();
                }

                _usingObjects.Enqueue(gameObject);
                gameObject.SetActive(true);

                if (_poolRootObject != null)
                {
                    gameObject.transform.SetParent(null);
                }
                return gameObject;
            }

            public override void Dispose()
            {
                base.Dispose();
                _usingObjects.Clear();
                _usingObjects = null;
                _poolRootObject = null;
            }
        }
    }
}