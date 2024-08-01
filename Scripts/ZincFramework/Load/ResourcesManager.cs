using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace ZincFramework
{
    namespace Load
    {
        public class ResourceInfo<T> where T : Object
        {
            public T asset;
            public UnityAction<T> completed;
            public Coroutine coroutine = null;
            public bool IsDeleting { get; set; }

            public ResourceInfo(UnityAction<T> callback)
            {
                completed = callback;
            }

            public ResourceInfo()
            {

            }
        }


        public class ResourcesManager : BaseSafeSingleton<ResourcesManager>
        {
            private readonly Dictionary<string, ResourceInfo<Object>> _resourcesDic = new Dictionary<string, ResourceInfo<Object>>();

            private ResourcesManager()
            {

            }

            public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
            {
                string resName = path + '_' + typeof(T).Name;

                if (!_resourcesDic.TryGetValue(resName, out var info))
                {
                    info = new ResourceInfo<Object>((obj) =>
                    {
                        callback?.Invoke(obj as T);
                        info.completed = null;
                        info.coroutine = null;
                    });

                    info.coroutine = MonoManager.Instance.StartCoroutine(R_LoadAsync<T>(path, resName));
                    _resourcesDic.Add(resName, info);
                }
                else
                {
                    if (info.asset == null)
                    {
                        info.completed += (obj) =>
                        {
                            callback?.Invoke(obj as T);
                        };
                    }
                    else
                    {
                        callback.Invoke(info.asset as T);
                    }
                }
            }

            private IEnumerator R_LoadAsync<T>(string path, string resName) where T : Object
            {
                ResourceRequest request = Resources.LoadAsync<T>(path);
                yield return request;

                if (_resourcesDic.TryGetValue(resName, out var info))
                {
                    if (info.IsDeleting)
                    {
                        Resources.UnloadAsset(info.asset);
                        _resourcesDic.Remove(resName);
                    }
                    else
                    {
                        info.asset = request.asset;
                        info.completed?.Invoke(info.asset);
                        info.completed = null;
                        info.coroutine = null;
                    }
                }
            }

            public void LoadAsync(string path, UnityAction<Object> callback, System.Type type)
            {
                string resName = path + "_" + type.Name;
                if (!_resourcesDic.TryGetValue(resName, out var info))
                {
                    info = new ResourceInfo<Object>((obj) =>
                    {
                        callback?.Invoke(obj);
                        info.completed = null;
                        info.coroutine = null;
                    });

                    info.coroutine = MonoManager.Instance.StartCoroutine(R_LoadAsync(path, resName, type));
                    _resourcesDic.Add(resName, info);
                }
                else
                {
                    if (info.asset == null)
                    {
                        info.completed += (obj) =>
                        {
                            callback?.Invoke(obj);
                        };
                    }
                    else
                    {
                        callback.Invoke(info.asset);
                    }
                }
            }

            private IEnumerator R_LoadAsync(string path, string resName, System.Type type)
            {
                ResourceRequest request = Resources.LoadAsync(path, type);
                yield return request;

                if (_resourcesDic.TryGetValue(resName, out var info))
                {
                    if (info.IsDeleting)
                    {
                        Resources.UnloadAsset(info.asset);
                        _resourcesDic.Remove(resName);
                    }
                    else
                    {
                        info.asset = request.asset;
                        info.completed?.Invoke(info.asset);
                        info.completed = null;
                        info.coroutine = null;
                    }
                }
            }



            public T Load<T>(string path) where T : Object
            {
                //更改同步加载逻辑
                string resName = path + "_" + typeof(T).Name;
                if (_resourcesDic.TryGetValue(resName, out var info))
                {
                    if (info.asset == null)
                    {
                        MonoManager.Instance.StopCoroutine(info.coroutine);
                        info.asset = Resources.Load<T>(path);
                        info.completed.Invoke(info.asset);

                        info.completed = null;
                        info.coroutine = null;
                    }
                }
                else
                {
                    info = new ResourceInfo<Object>();
                    info.asset = Resources.Load<T>(path);
                    _resourcesDic.Add(resName, info);
                }
                return info.asset as T;
            }


            public void Unload<T>(string name) where T : Object
            {
                string resName = name + "_" + typeof(T);
                if (_resourcesDic.TryGetValue(name, out var info))
                {
                    if (info.asset == null)
                    {
                        info.IsDeleting = true;
                    }
                    else
                    {
                        Resources.UnloadAsset(info.asset);
                        _resourcesDic.Remove(resName);
                    }
                }
            }

            public void Unload(string name, System.Type type)
            {
                string resName = name + "_" + type.Name;
                if (_resourcesDic.TryGetValue(name, out var info))
                {
                    if (info.asset == null)
                    {
                        info.IsDeleting = true;
                    }
                    else
                    {
                        Resources.UnloadAsset(info.asset);
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
}

