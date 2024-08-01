using UnityEngine;

namespace ZincFramework
{
    namespace GameObjectPool
    {
        /// <summary>
        /// ����ض�����ʧ�߼�����д��OnEnable��
        /// </summary>
        public class PooledObject : MonoBehaviour
        {
            protected virtual void HideMe()
            {
                GameObjectPoolManager.Instance.ReturnGameObject(gameObject);
            }
        }
    }
}

