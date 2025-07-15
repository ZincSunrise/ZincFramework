using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Events;
using ZincFramework.LoadServices.Resource;
using ZincFramework.Threading.Tasks;


namespace ZincFramework.LoadServices
{
    public static class ResourcesManager
    {
        private static readonly Dictionary<string, IResourceCache> _resourcesDic = new Dictionary<string, IResourceCache>();

        /// <summary>
        /// 同步加载方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadAsset<T>(string path) where T : Object
        {
            if (!_resourcesDic.TryGetValue(path, out var info))
            {
                info = new ResourceSyncCache(Resources.Load<T>(path));
                _resourcesDic.Add(path, info);
            }
            else if(!info.IsCompleted)
            {
                throw new System.InvalidOperationException($"不可以在{path}的资源异步加载的时候进行同步加载");
            }

            return info.Asset as T;
        }

        public static async ZincTask<T> LoadAssetAsync<T>(string path) where T : Object
        {
            if (!_resourcesDic.TryGetValue(path, out var resourceCache))
            {
                resourceCache = new ResourceAsyncCache<T>(Resources.LoadAsync<T>(path));
                _resourcesDic.Add(path, resourceCache);
            }

            if(resourceCache.IsDeleting)
            {
                throw new ResourceDeletingException($"{path}的资源正在删除，不可使用");
            }

            return resourceCache.IsCompleted ? resourceCache.Asset as T : await (resourceCache as ResourceAsyncCache<T>).ResourceRequest as T;
        }

        public static void LoadAssetAsync<T>(string path, ZincAction<T> callback) where T : Object
        {
            if (!_resourcesDic.TryGetValue(path, out var resourceCache))
            {
                resourceCache = new ResourceAsyncCache<T>(Resources.LoadAsync<T>(path), (obj) => callback.Invoke(obj as T));
                _resourcesDic.Add(path, resourceCache);
            }
            else
            {
                if (resourceCache.IsDeleting)
                {
                    throw new ResourceDeletingException($"{path}的资源正在删除，不可使用");
                }

                if (resourceCache.IsCompleted)
                {
                    callback?.Invoke(resourceCache.Asset as T);
                }
                else
                {
                    (resourceCache as ResourceAsyncCache<T>).Completed += callback;
                }
            }
        }

        public static void LoadAssetAsync(string path, ZincAction<Object> callback, System.Type type)
        {
            if (!_resourcesDic.TryGetValue(path, out var resourceCache))
            {
                resourceCache = new ResourceAsyncCache<Object>(Resources.LoadAsync(path, type), callback);
                _resourcesDic.Add(path, resourceCache);
            }
            else
            {
                if (resourceCache.IsCompleted)
                {
                    callback?.Invoke(resourceCache.Asset);
                }
                else
                {
                    (resourceCache as ResourceAsyncCache<Object>).Completed += callback;
                }
            }
        }

        public static void Release<T>(string path) where T : Object
        {
            if (_resourcesDic.TryGetValue(path, out var resourceCache))
            {
                if (!resourceCache.IsCompleted)
                {
                    (resourceCache as ResourceAsyncCache<T>).StopLoad();
                }
                else
                {
                    Resources.UnloadAsset(resourceCache.Asset);
                    _resourcesDic.Remove(path);
                }
            }
        }


        public static void Release(string path)
        {
            if (_resourcesDic.TryGetValue(path, out var resourceCache))
            {
                if (!resourceCache.IsCompleted)
                {
                    (resourceCache as ResourceAsyncCache<Object>).StopLoad();
                }
                else
                {
                    Resources.UnloadAsset(resourceCache.Asset);
                    _resourcesDic.Remove(path);
                }
            }
        }

        public static async ZincTask Clear(ZincAction callback)
        {
            await Resources.UnloadUnusedAssets();
            callback.Invoke();
        }
    }
}

