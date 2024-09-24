using UnityEngine;
using ZincFramework.DataPool;



namespace ZincFramework
{
    namespace GameObjectPool
    {
        /// <summary>
        /// 对象池对象消失逻辑必须写在OnEnable里
        /// </summary>
        public abstract class ResumableObject : MonoBehaviour, IResumable
        {
            public abstract void OnRent();

            public abstract void OnReturn();
        }
    }
}

