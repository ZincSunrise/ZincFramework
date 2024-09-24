using UnityEngine;
using ZincFramework.DataPool;



namespace ZincFramework
{
    namespace GameObjectPool
    {
        /// <summary>
        /// ����ض�����ʧ�߼�����д��OnEnable��
        /// </summary>
        public abstract class ResumableObject : MonoBehaviour, IResumable
        {
            public abstract void OnRent();

            public abstract void OnReturn();
        }
    }
}

