using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using ZincFramework.Events;

namespace ZincFramework
{
    public class SceneLoadManager : BaseSafeSingleton<SceneLoadManager>
    {
        public float Progress => _progress;
        public bool IsLoading => _isLoading;

        private float _progress;

        private bool _isLoading = false;

        private SceneLoadManager() { }


        public void LoadScene(string name, UnityAction callback = null)
        {
            if (!_isLoading)
            {
                SceneManager.LoadScene(name);
                callback?.Invoke();
            }
        }

        public void LoadSceneAsync(string name, UnityAction callback = null)
        {
            MonoManager.Instance.StartCoroutine(R_LoadSceneAsync(name, callback));
        }

        private IEnumerator R_LoadSceneAsync(string name, UnityAction callback)
        {
            _isLoading = true;
            AsyncOperation operation = SceneManager.LoadSceneAsync(name);
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                EventCenter.Boardcast<float>(E_Event_Type.E_SceneLoading, operation.progress);
                _progress = operation.progress;
                if (operation.progress >= 0.9f)
                {
                    operation.allowSceneActivation = true;
                }
                yield return 1;
            }

            EventCenter.Boardcast<float>(E_Event_Type.E_SceneLoading, 1);
            callback?.Invoke();
            _isLoading = false;
        }
    }
}

