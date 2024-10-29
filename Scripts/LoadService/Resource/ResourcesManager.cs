using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZincFramework.LoadServices.Resource;


namespace ZincFramework.LoadServices
{
    public class ResourcesManager : BaseSafeSingleton<ResourcesManager>
    {
        private readonly Dictionary<string, ResourceCache> _resourcesDic = new Dictionary<string, ResourceCache>();

        private ResourcesManager()
        {

        }

        public async Task<T> LoadAsync<T>(string path) where T : Object
        {
            string resName = path + '_' + typeof(T).Name;

            if (!_resourcesDic.TryGetValue(resName, out var info))
            {
                info = new ResourceCache(Resources.LoadAsync<T>(path));
                _resourcesDic.Add(resName, info);
            }

            if(info.IsDeleting)
            {
                throw new ResourceDeletingException($"{resName}的资源正在删除，不可使用");
            }

            return info.IsCompleted ? info.Asset as T : await info.ResourceRequest as T;
        }

        public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
        {
            string resName = path + '_' + typeof(T).Name;

            if (!_resourcesDic.TryGetValue(resName, out var resourceCache))
            {
                resourceCache = new ResourceCache(Resources.LoadAsync<T>(path), (obj) => callback.Invoke(obj as T));
                _resourcesDic.Add(resName, resourceCache);
            }
            else
            {
                if (resourceCache.IsCompleted)
                {
                    callback?.Invoke(resourceCache.Asset as T);
                }
                else
                {
                    resourceCache.Completed += (x) => callback.Invoke(x as T);
                    MonoManager.Instance.StartCoroutine(CheckDelete(resName, resourceCache));
                }
            }
        }

        public void LoadAsync(string path, UnityAction<Object> callback, System.Type type)
        {
            string resName = path + '_' + type.Name;

            if (!_resourcesDic.TryGetValue(resName, out var resourceCache))
            {
                resourceCache = new ResourceCache(Resources.LoadAsync(path), (obj) => callback.Invoke(obj));
                _resourcesDic.Add(resName, resourceCache);
            }
            else
            {
                if (resourceCache.IsCompleted)
                {
                    callback?.Invoke(resourceCache.Asset);
                }
                else
                {
                    resourceCache.Completed += (obj) => callback.Invoke(obj);
                    MonoManager.Instance.StartCoroutine(CheckDelete(resName, resourceCache));
                }
            }
        }

        private IEnumerator CheckDelete(string resName, ResourceCache resourceCache)
        {
            yield return resourceCache.ResourceRequest;

            if (resourceCache.IsDeleting)
            {
                Resources.UnloadAsset(resourceCache.Asset);
                _resourcesDic.Remove(resName);
            }
        }

        public T Load<T>(string path) where T : Object
        {
            //更改同步加载逻辑
            string resName = path + '_' + typeof(T).Name;
            if (_resourcesDic.TryGetValue(resName, out var info) && !info.IsCompleted)
            {
                return info.Asset as T;
            }
            else if(info == null)
            {
                info = new ResourceCache(Resources.Load<T>(path));
                _resourcesDic.Add(resName, info);
            }

            return info.Asset as T;
        }

        public void Release<T>(string name) where T : Object
        {
            string resName = name + '_' + typeof(T);
            if (_resourcesDic.TryGetValue(name, out var info))
            {
                if (!info.IsCompleted)
                {
                    info.StopLoad();
                }
                else
                {
                    Resources.UnloadAsset(info.Asset);
                    _resourcesDic.Remove(resName);
                }
            }
        }

        public void Release(string name, System.Type type)
        {
            string resName = name + '_' + type.Name;
            if (_resourcesDic.TryGetValue(name, out var info))
            {
                if (!info.IsCompleted)
                {
                    info.StopLoad();
                }
                else
                {
                    Resources.UnloadAsset(info.Asset);
                    _resourcesDic.Remove(resName);
                }
            }
        }

        public void Clear(UnityAction callback)
        {
            MonoManager.Instance.StartCoroutine(R_Clear(callback));
        }

        private IEnumerator R_Clear(UnityAction callback)
        {
            yield return Resources.UnloadUnusedAssets();
            callback.Invoke();
        }
    }
}

