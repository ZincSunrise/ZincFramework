using UnityEngine;

namespace ZincFramework
{
    namespace GameObjectPool
    {
        /// <summary>
        /// 对象池对象消失逻辑必须写在OnEnable里
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

