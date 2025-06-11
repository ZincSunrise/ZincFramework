using System;
using UnityEngine;
using System.Collections.Generic;
using ZincFramework.Pool.GameObjects;
using ZincFramework.DataPools;


namespace ZincFramework.Pool
{
    public class ObjectPoolManager : BaseSafeSingleton<ObjectPoolManager>
    {
        private readonly Dictionary<string, GameObjectPool> _objectDic = new Dictionary<string, GameObjectPool>();

        public int DefaultMaxCount => 30;

        private ObjectPoolManager() { }


        /// <summary>
        /// ��ѭ������أ�����ﵽ����������ٻ�ȡ������
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sourceObject"></param>
        /// <param name="maxCount">�������-1�����û�������������</param>
        public void RegistNonCyclicPool(string name, GameObject sourceObject, int maxCount = 0, GameObject rootObject = null)
        {
#if UNITY_EDITOR
            if (_objectDic.ContainsKey(name))
            {
                Debug.LogWarning($"���滻�˶����������Ϊ{name}�Ķ���");
            }
#endif
            if (!sourceObject.TryGetComponent<IReuseable>(out _))
            {
                throw new NotSupportedException("��֧�ֲ��̳�" + nameof(IReuseable) + "����");
            }

            sourceObject.name = name;
            maxCount = maxCount == 0 ? DefaultMaxCount : maxCount;
            _objectDic[name] = new NonCyclicPool(sourceObject, maxCount, rootObject);
        }


        /// <summary>
        /// ѭ������أ�����ﵽ����������Ὣ����ȡ���Ķ������ò�ȡ��
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sourceObject"></param>
        /// <param name="maxCount"></param>
        public void RegistCyclicPool(string name, GameObject sourceObject, int maxCount = 0, GameObject rootObject = null)
        {
#if UNITY_EDITOR
            if (_objectDic.ContainsKey(name))
            {
                Debug.LogWarning($"���滻�˶����������Ϊ{name}�Ķ���");
            }
#endif

            if (!sourceObject.TryGetComponent<IReuseable>(out _))
            {
                throw new NotSupportedException("��֧�ֲ��̳�" + nameof(IReuseable) + "����");
            }

            sourceObject.name = name;
            maxCount = maxCount == 0 ? DefaultMaxCount : maxCount;
            _objectDic[name] = new CyclicPool(sourceObject, maxCount, rootObject);
        }

        /// <summary>
        /// ��ɾ�������ڶ����֮�е�����
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

        public void SetPoolParent(string name, GameObject parent)
        {
            if (_objectDic.TryGetValue(name, out GameObjectPool gameObjectPool))
            {
                gameObjectPool.RootObject = parent;
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
        /// ����Ƿ����������
        /// </summary>
        /// <param name="name"></param>
        public bool ContainsPool(string name) => _objectDic.ContainsKey(name);

        /// <summary>
        /// ���ĳһ�������
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
        /// ����֮ǰ�ǵ���ע��
        /// </summary>
        /// <param name="name"></param>
        /// <returns>��ע��ͻᱨ��Ŷ</returns>
        /// <exception cref="ArgumentException"></exception>
        public T RentReuseableObject<T>(string name) where T : ReuseableObject
        {
            if (!_objectDic.TryGetValue(name, out GameObjectPool gameObjectPool))
            {
                throw new ArgumentException($"��û��ע������Ϊ{name}�Ķ���");
            }

            return gameObjectPool.RentValue() as T;
        }

        /// <summary>
        /// ���ÿɻ��ն���
        /// </summary>
        /// <param name="reuseableObject"></param>
        /// <param name="resetCallback"></param>
        public void ReturnReusableGameObject(ReuseableObject reuseableObject, Action<GameObject> resetCallback = null)
        {
            resetCallback?.Invoke(reuseableObject.gameObject);
            _objectDic[reuseableObject.name].ReturnValue(reuseableObject);
        }


        /// <summary>
        /// ����֮ǰ�ǵ���ע��
        /// </summary>
        /// <param name="name"></param>
        /// <returns>��ע��ͻᱨ��Ŷ</returns>
        /// <exception cref="ArgumentException"></exception>
        public GameObject RentGameObject(string name)
        {
            if (!_objectDic.TryGetValue(name, out GameObjectPool gameObjectPool))
            {
                throw new ArgumentException($"��û��ע������Ϊ{name}�Ķ���");
            }

            return gameObjectPool.RentValue().gameObject;
        }


        /// <summary>
        /// ������ֱ��ʹ�����,��Ӧ��ʹ�ò���ΪReuseableObject��
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="resetCallback"></param>
        public void ReturnGameObject(GameObject gameObject, Action<GameObject> resetCallback = null)
        {
            resetCallback?.Invoke(gameObject);
            _objectDic[gameObject.name].ReturnValue(gameObject.GetComponent<ReuseableObject>());
        }


        /// <summary>
        /// ���ղ����ĳһ��������еĶ���
        /// </summary>
        /// <param name="poolName"></param>
        public void PoolReturnAll(string poolName)
        {
            if (!_objectDic.TryGetValue(poolName, out GameObjectPool gameObjectPool))
            {
                throw new ArgumentException($"��û��ע������Ϊ{poolName}�Ķ���");
            }

            gameObjectPool.ReturnAll();
        }

        /// <summary>
        /// ���յ��ǲ����������ж���
        /// </summary>
        public void ReturnAll()
        {
            foreach (var pool in _objectDic.Values)
            {
                pool.ReturnAll();
            }
        }


        /// <summary>
        /// ���������е����ã����ǲ��������
        /// </summary>
        public void ClearAllPool()
        {
            foreach (var item in _objectDic.Values)
            {
                item.Clear();
            }
        }


        /// <summary>
        /// �������غͶ�����е����ж���
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

