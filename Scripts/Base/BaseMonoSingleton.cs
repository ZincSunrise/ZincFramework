using UnityEngine;

namespace ZincFramework
{
    public class BaseMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance => instance;
        protected virtual void Awake()
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}

