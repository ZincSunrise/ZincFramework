using UnityEngine;

namespace ZincFramework
{
    public class BaseMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance => _instance;


        protected virtual void Awake()
        {
            _instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}

