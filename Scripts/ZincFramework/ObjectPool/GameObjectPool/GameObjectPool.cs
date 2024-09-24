using System.Collections.Generic;
using UnityEngine;
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
            public string Name => _prefab.name;


            private GameObject _poolRootObject;

            private readonly GameObject _prefab;


            private Queue<GameObject> _usingObjects = new Queue<GameObject>();


            public GameObjectPool(GameObject prefab, GameObject rootObject = null) : base()
            {
                _prefab = prefab;

                _createFunction = () =>
                {
                    GameObject returnObj = GameObject.Instantiate(_prefab);
                    returnObj.name = _prefab.name;
                    return returnObj;
                };

                if (rootObject != null)
                {
                    _poolRootObject = new GameObject(prefab.name);
                    _poolRootObject.transform.SetParent(rootObject.transform);
                }
            }


            public override void ReturnValue(GameObject gameObject)
            {
                if (_poolRootObject != null)
                {
                    gameObject.transform.SetParent(_poolRootObject.transform);
                }

                gameObject.SendMessage("OnReturn", SendMessageOptions.DontRequireReceiver);
                gameObject.SetActive(false);

                _unuseValues.Enqueue(gameObject);
                _usingObjects.Dequeue();
            }


            public override GameObject RentValue()
            {
                GameObject gameObject;

                if (CacheCount > 0)
                {
                    gameObject = _unuseValues.Dequeue();
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

                gameObject.SendMessage("OnRent", SendMessageOptions.DontRequireReceiver);
                gameObject.SetActive(true);

                if (_poolRootObject != null)
                {
                    gameObject.transform.SetParent(null);
                }

                return gameObject;
            }

            public void ReturnAll()
            {
                while (_usingObjects.Count > 0)
                {
                    ReturnValue(_usingObjects.Dequeue());
                }
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