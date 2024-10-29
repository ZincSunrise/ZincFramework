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


        public void LoadScene(string name, ZincAction callback = null)
        {
            if (!_isLoading)
            {
                SceneManager.LoadScene(name);
                callback?.Invoke();
            }
        }

        public void LoadScene(int sceneIndex, ZincAction callback = null)
        {
            if (!_isLoading)
            {
                SceneManager.LoadScene(sceneIndex);
                callback?.Invoke();
            }
        }

        public IEnumerator LoadSceneAsync(int sceneIndex, ZincAction callback)
        {
            _isLoading = true;
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                EventCenter.Boardcast<float>(Events.EventType.SceneLoading, operation.progress);
                _progress = operation.progress;
                if (operation.progress >= 0.9f)
                {
                    operation.allowSceneActivation = true;
                }
                yield return 1;
            }

            EventCenter.Boardcast<float>(Events.EventType.SceneLoading, 1);
            callback?.Invoke();
            _isLoading = false;
        }

        public IEnumerator LoadSceneAsync(string name, ZincAction callback)
        {
            _isLoading = true;
            AsyncOperation operation = SceneManager.LoadSceneAsync(name);
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                EventCenter.Boardcast<float>(Events.EventType.SceneLoading, operation.progress);
                _progress = operation.progress;
                if (operation.progress >= 0.9f)
                {
                    operation.allowSceneActivation = true;
                }
                yield return 1;
            }

            EventCenter.Boardcast<float>(Events.EventType.SceneLoading, 1);
            callback?.Invoke();
            _isLoading = false;
        }
    }
}

