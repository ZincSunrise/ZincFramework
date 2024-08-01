using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using ZincFramework.Load.Editor;


namespace ZincFramework
{
    namespace Load
    {
        public class AssetBundleManager : BaseSafeSingleton<AssetBundleManager>
        {
            private readonly string _URLPlatform;
            private bool _isClearing = false;

            private readonly Dictionary<string, AssetBundle> _bundlesDic = new Dictionary<string, AssetBundle>();

            private AssetBundle _mainAssetBundle = null;

            private AssetBundleManifest _manifest = null;

            private AssetBundleManager()
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        _URLPlatform = "Android";
                        break;
                    case RuntimePlatform.WindowsEditor:
                    case RuntimePlatform.WindowsPlayer:
                        _URLPlatform = "PC";
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        _URLPlatform = "IOS";
                        break;
                }
            }

            public void LoadDependencies(string assetBundleName)
            {
                if (_mainAssetBundle == null)
                {
                    _mainAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath,  _URLPlatform));
                    _manifest = _mainAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                }

                string[] dependcies = _manifest.GetDirectDependencies(assetBundleName);

                for (int i = 0; i < dependcies.Length; i++)
                {
                    if (!_bundlesDic.TryGetValue(dependcies[i], out AssetBundle assetBundle))
                    {
                        assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, dependcies[i]));
                        _bundlesDic.Add(dependcies[i], assetBundle);
                    }
                }
            }

            public void LoadAssetBundleAsync(string assetBundleName)
            {
                MonoManager.Instance.StartCoroutine(R_LoadAssetBundleAsync(assetBundleName));
            }

            private IEnumerator R_LoadAssetBundleAsync(string assetBundleName)
            {
                if (!_bundlesDic.TryGetValue(assetBundleName, out AssetBundle assetBundle))
                {
                    AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, assetBundleName));
                    _bundlesDic.Add(assetBundleName, assetBundle);

                    yield return assetBundleCreateRequest;

                    _bundlesDic[assetBundleName] = assetBundleCreateRequest.assetBundle;
                }
            }

            public void LoadAssetAsync(string assetBundleName, string resourceName, System.Type type, UnityAction<Object> callback, bool isAnsyc = true)
            {
                MonoManager.Instance.StartCoroutine(R_LoadAssetAsync(assetBundleName, resourceName, type, callback, isAnsyc));
            }

            private IEnumerator R_LoadAssetAsync(string assetBundleName, string resourceName, System.Type type, UnityAction<Object> callback, bool isAnsyc)
            {
                if (_mainAssetBundle == null)
                {
                    _mainAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, _URLPlatform));
                    _manifest = _mainAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                }

                string[] dependcies = _manifest.GetDirectDependencies(assetBundleName);

                for (int i = 0; i < dependcies.Length; i++)
                {
                    if (!_bundlesDic.ContainsKey(dependcies[i]))
                    {
                        if (isAnsyc)
                        {
                            yield return R_LoadAssetBundleAsync(dependcies[i]);
                        }
                        else
                        {
                            _bundlesDic.Add(assetBundleName, AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, dependcies[i])));
                        }

                    }
                    else
                    {
                        while (_bundlesDic[dependcies[i]] == null)
                        {
                            yield return 1;
                        }
                    }
                }

                if (!_bundlesDic.ContainsKey(assetBundleName))
                {
                    if (isAnsyc)
                    {
                        yield return R_LoadAssetBundleAsync(assetBundleName);
                    }
                    else
                    {
                        _bundlesDic.Add(assetBundleName, AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleName)));
                    }
                }
                else
                {
                    while (_bundlesDic[assetBundleName] == null)
                    {
                        yield return 0;
                    }
                }

                if (isAnsyc)
                {
                    AssetBundleRequest assetBundleRequest = _bundlesDic[assetBundleName].LoadAssetAsync(resourceName);
                    yield return assetBundleRequest;
                    callback.Invoke(assetBundleRequest.asset);
                }
                else
                {
                    callback.Invoke(_bundlesDic[assetBundleName].LoadAsset(resourceName));
                }
            }

            public void LoadAssetAsync<T>(string assetBundleName, string resourceName, UnityAction<T> callback, bool isAnsyc = true) where T : Object
            {
#if UNITY_EDITOR
                if (FrameworkData.Shared.isDebug)
                {
                    callback?.Invoke(AssetDataManager.LoadAssetAtPath<T>(Path.Combine(assetBundleName, resourceName)));
                    return;
                }
#endif
                MonoManager.Instance.StartCoroutine(R_LoadAssetAsync<T>(assetBundleName, resourceName, callback, isAnsyc));
            }

            private IEnumerator R_LoadAssetAsync<T>(string assetBundleName, string resourceName, UnityAction<T> callback, bool isAnsyc) where T : Object
            {
                if (_mainAssetBundle == null)
                {
                    _mainAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, _URLPlatform));
                    _manifest = _mainAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                }

                string[] dependcies = _manifest.GetDirectDependencies(assetBundleName);

                for (int i = 0; i < dependcies.Length; i++)
                {
                    if (!_bundlesDic.ContainsKey(dependcies[i]))
                    {
                        if (isAnsyc)
                        {
                            yield return R_LoadAssetBundleAsync(dependcies[i]);
                        }
                        else
                        {
                            _bundlesDic.Add(assetBundleName, AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, dependcies[i])));
                        }

                    }
                    else
                    {
                        while (_bundlesDic[dependcies[i]] == null)
                        {
                            yield return 1;
                        }
                    }
                }

                if (!_bundlesDic.ContainsKey(assetBundleName))
                {
                    if (isAnsyc)
                    {
                        yield return R_LoadAssetBundleAsync(assetBundleName);
                    }
                    else
                    {
                        _bundlesDic.Add(assetBundleName, AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleName)));
                    }
                }
                else
                {
                    while (_bundlesDic[assetBundleName] == null)
                    {
                        yield return 0;
                    }
                }

                if (isAnsyc)
                {
                    AssetBundleRequest assetBundleRequest = _bundlesDic[assetBundleName].LoadAssetAsync<T>(resourceName);
                    yield return assetBundleRequest;
                    callback.Invoke(assetBundleRequest.asset as T);
                }
                else
                {
                    callback.Invoke(_bundlesDic[assetBundleName].LoadAsset<T>(resourceName));
                }
            }

            /// <summary>
            /// 使用该方法时，AB包也会被同步加载
            /// </summary>
            /// <param name="assetBundleName"></param>
            /// <param name="resourceName"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            [System.Obsolete("此方法不可在使用协同程序之后调用")]
            public Object LoadAsset(string assetBundleName, string resourceName, System.Type type)
            {
                LoadDependencies(assetBundleName);

                if (!_bundlesDic.TryGetValue(assetBundleName, out AssetBundle assetBundle))
                {
                    assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleName));
                    _bundlesDic.Add(assetBundleName, assetBundle);
                    return assetBundle.LoadAsset(resourceName, type);
                }
                else
                {
                    Debug.LogError("不可在使用协同程序之后调用该函数");
                    return null;
                }
            }


            /// <summary>
            /// 使用该方法时，AB包也会被同步加载
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="assetBundleName"></param>
            /// <param name="resourceName"></param>
            /// <returns></returns>
            [System.Obsolete("此方法不可在使用协同程序之后调用")]
            public T LoadAsset<T>(string assetBundleName, string resourceName) where T : Object
            {
                LoadDependencies(assetBundleName);

                if (!_bundlesDic.TryGetValue(assetBundleName, out AssetBundle assetBundle))
                {
                    assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleName));
                    _bundlesDic.Add(assetBundleName, assetBundle);
                    return assetBundle.LoadAsset<T>(resourceName);
                }
                else
                {
                    Debug.LogError("不可在使用协同程序之后调用该函数");
                    return null;
                }
            }

            public void RemoveAssetBundle(string assetBundleName)
            {
                if (_bundlesDic.ContainsKey(assetBundleName))
                {
                    MonoManager.Instance.StartCoroutine(R_RemoveAssetBundle(assetBundleName));
                }
            }

            private IEnumerator R_RemoveAssetBundle(string assetBundleName)
            {
                while (_bundlesDic[assetBundleName] == null)
                {
                    yield return 1;
                }
                _bundlesDic[assetBundleName].Unload(false);
                _bundlesDic.Remove(assetBundleName);
            }

            public void Clear()
            {
                if (!_isClearing)
                {
                    MonoManager.Instance.StopAllCoroutines();
                    MonoManager.Instance.StartCoroutine(R_Clear());
                }
            }

            private IEnumerator R_Clear()
            {
                _isClearing = true;
                yield return new WaitForSeconds(10f);

                AssetBundle.UnloadAllAssetBundles(false);
                _bundlesDic.Clear();
                _mainAssetBundle = null;
                _manifest = null;
                _isClearing = false;
            }
        }
    }
}

