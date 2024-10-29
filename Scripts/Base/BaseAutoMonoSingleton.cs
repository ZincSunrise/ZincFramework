using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ZincFramework
{
    public class BaseAutoMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject monoObject = new GameObject();
                    monoObject.name = typeof(T).Name;
                    instance = monoObject.AddComponent<T>();
                    DontDestroyOnLoad(monoObject);
                }
                return instance;
            }
        }
    }
}

