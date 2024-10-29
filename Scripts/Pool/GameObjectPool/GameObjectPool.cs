using System.Collections;
using UnityEngine;
using ZincFramework.DataPools;


namespace ZincFramework.Pool.GameObjects
{
    /// <summary>
    /// 游戏对象池基类
    /// </summary>
    public abstract class GameObjectPool : ObjectPool<ReuseableObject>
    {
        public string Name { get; }

        public abstract IEnumerable UsingObjects { get; }

        public GameObject RootObject 
        {
            get => _rootObject;
            set
            {
                if (value != null)
                {
                    _rootObject = new GameObject(Name);
                    _rootObject.transform.SetParent(value.transform);
                }
            } 
        }


        private GameObject _rootObject;

        public GameObjectPool(GameObject prefab, int maxCount = -1, GameObject rootObject = null) : base(() => CreateReuseable(prefab), maxCount)
        {
            Name = prefab.name;

            if (rootObject != null)
            {
                _rootObject = new GameObject(prefab.name);
                _rootObject.transform.SetParent(rootObject.transform);
            }
        }

        public abstract override ReuseableObject RentValue();

        public override void ReturnValue(ReuseableObject reusableGameObject)
        {
            reusableGameObject.OnReturn();
            reusableGameObject.gameObject.SetActive(false); 
        }

        public abstract void ReturnAll();

        public override void Dispose()
        {
            RootObject = null;
        }

        private static ReuseableObject CreateReuseable(GameObject prefab)
        {
            ReuseableObject gameObject = GameObject.Instantiate(prefab).GetComponent<ReuseableObject>();
            gameObject.gameObject.name = prefab.name;
            return gameObject;
        }
    }
}
