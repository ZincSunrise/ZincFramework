using UnityEngine;

namespace ZincFramework.Localization
{
    public abstract class LocalizableBehaviour<T> : MonoBehaviour, ILocalizable<T>
    {
        public abstract void Localize(in T value);
    }
}