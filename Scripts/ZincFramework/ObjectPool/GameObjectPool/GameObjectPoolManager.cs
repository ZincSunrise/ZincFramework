using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Events;


namespace ZincFramework
{
    namespace GameObjectPool
    {
        public class GameObjectPoolManager : BaseSafeSingleton<GameObjectPoolManager>
        {
            private readonly Dictionary<string, GameObjectPool> _objectDic = new Dictionary<string, GameObjectPool>();

            public bool IsOpenLayout => FrameworkData.Shared.isOpenLayout;
            public int DefaultMaxCount => FrameworkData.Shared.maxPoolCount;


            private GameObject _poolObjectRoot;

            private GameObjectPoolManager()
            {
                if (IsOpenLayout)
                {
                    _poolObjectRoot = new GameObject("Pool");
                }
            }

            public void ReturnGameObject(GameObject gameObject, ZincAction<GameObject> resetCallback = null)
            {
                resetCallback?.Invoke(gameObject);
                _objectDic[gameObject.name].ReturnValue(gameObject);
            }

            public GameObject RentGameObject(string name, int maxCount = -1)
            {
                if (!_objectDic.TryGetValue(name, out GameObjectPool gameObjectPool))
                {
                    GameObject gameObject = Resources.Load<GameObject>(name);
                    gameObjectPool = new GameObjectPool(() =>
                    {
                        GameObject returnObj = GameObject.Instantiate(gameObject);
                        returnObj.name = name;
                        return returnObj;
                    }, name, _poolObjectRoot);

                    _objectDic.Add(name, gameObjectPool);
                }

                gameObjectPool.MaxCount = maxCount == -1 ? DefaultMaxCount : maxCount;
                return gameObjectPool.RentValue();
            }


            public void Clear()
            {
                foreach (var item in _objectDic.Values)
                {
                    item.Dispose();
                }

                _objectDic.Clear();
                _poolObjectRoot = null;
            }
        }
    }
}

