using UnityEngine;



namespace ZincFramework.Pools.GameObjects
{
    /// <summary>
    /// 对象池对象消失逻辑必须写在OnEnable里
    /// </summary>
    public abstract class ReuseableObject : MonoBehaviour, IReuseable
    {
        public abstract void OnRent();

        public abstract void OnReturn();
    }
}

