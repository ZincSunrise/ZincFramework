using UnityEngine;
using ZincFramework.DataPools;



namespace ZincFramework.Pool.GameObjects
{
    /// <summary>
    /// ����ض�����ʧ�߼�����д��OnEnable��
    /// </summary>
    public abstract class ReuseableObject : MonoBehaviour, IReuseable
    {
        public abstract void OnRent();

        public abstract void OnReturn();
    }
}

