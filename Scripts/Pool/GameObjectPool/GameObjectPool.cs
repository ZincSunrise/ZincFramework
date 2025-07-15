using System.Collections;
using UnityEngine;


namespace ZincFramework.Pools.GameObjects
{
    /// <summary>
    /// 游戏对象池基类
    /// </summary>
    public abstract class GameObjectPool : ObjectPool<ReuseableObject>
    {
        public string Name { get; }

        public abstract IEnumerable UsingObjects { get; }

        public GameObject RootObject => _rootObject;


        private GameObject _rootObject;

        public GameObjectPool(GameObject prefab, int maxCount = -1, GameObject rootObject = null) : base(() => CreateReuseable(prefab), maxCount)
        {
            Name = prefab.name;
            _rootObject = rootObject == null ? new GameObject(prefab.name) : rootObject;
        }

        public abstract override ReuseableObject RentValue();

        public override void ReturnValue(ReuseableObject reusableGameObject)
        {
            reusableGameObject.OnReturn();
            reusableGameObject.gameObject.SetActive(false);
            reusableGameObject.transform.SetParent(_rootObject.transform);
        }

        public abstract void ReturnAll();

        public override void Dispose()
        {
            _rootObject = null;
        }

        private static ReuseableObject CreateReuseable(GameObject prefab)
        {
            ReuseableObject gameObject = GameObject.Instantiate(prefab).GetComponent<ReuseableObject>();
            gameObject.gameObject.name = prefab.name;
            return gameObject;
        }
    }
}
