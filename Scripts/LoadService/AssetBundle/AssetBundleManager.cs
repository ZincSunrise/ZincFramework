using System.IO;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.LoadServices.AssetBundles;
using UnityEngine.Events;


namespace ZincFramework
{
    namespace LoadServices
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

            public async Task<AssetBundle> LoadAssetBundleAsync(string assetBundleName)
            {
                if (!_bundlesDic.TryGetValue(assetBundleName, out AssetBundle assetBundle))
                {
                    AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, assetBundleName));
                    _bundlesDic.Add(assetBundleName, assetBundle);

                    assetBundle = await assetBundleCreateRequest;
                    _bundlesDic[assetBundleName] = assetBundle;
                }

                return assetBundle;
            }

            public async Task<T> LoadAssetAsync<T>(string assetBundleName, string resourceName) where T : Object
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
                        await LoadAssetBundleAsync(dependcies[i]);
                    }
                    else
                    {
                        while (_bundlesDic[dependcies[i]] == null)
                        {
                            await Task.Yield();
                        }
                    }
                }

                if (!_bundlesDic.ContainsKey(assetBundleName))
                {
                    await LoadAssetBundleAsync(assetBundleName);
                }
                else
                {
                    while (_bundlesDic[assetBundleName] == null)
                    {
                        await Task.Yield();
                    }
                }

                AssetBundleRequest assetBundleRequest = _bundlesDic[assetBundleName].LoadAssetAsync<T>(resourceName);
                return assetBundleRequest.isDone ? assetBundleRequest.asset as T : (await assetBundleRequest) as T;
            }



            public async void LoadAssetAsync<T>(string assetBundleName, string resourceName, UnityAction<T> callback) where T : Object
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
                        await LoadAssetBundleAsync(dependcies[i]);
                    }
                    else
                    {
                        while (_bundlesDic[dependcies[i]] == null)
                        {
                            await Task.Yield();
                        }
                    }
                }

                if (!_bundlesDic.ContainsKey(assetBundleName))
                {
                    await LoadAssetBundleAsync(assetBundleName);
                }
                else
                {
                    while (_bundlesDic[assetBundleName] == null)
                    {
                        await Task.Yield();
                    }
                }

                AssetBundleRequest assetBundleRequest = _bundlesDic[assetBundleName].LoadAssetAsync<T>(resourceName);

                if (assetBundleRequest.isDone)
                {
                    callback.Invoke(assetBundleRequest.asset as T);
                }
                else
                {
                    callback.Invoke(await assetBundleRequest as T);
                }
            }

            /// <summary>
            /// ʹ�ø÷���ʱ��AB��Ҳ�ᱻͬ������
            /// </summary>
            /// <param name="assetBundleName"></param>
            /// <param name="resourceName"></param>
            /// <param name="type"></param>
            /// <returns></returns>
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
                    Debug.LogError("������ʹ��Эͬ����֮����øú���");
                    return null;
                }
            }


            /// <summary>
            /// ʹ�ø÷���ʱ��AB��Ҳ�ᱻͬ������
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="assetBundleName"></param>
            /// <param name="resourceName"></param>
            /// <returns></returns>
            public T LoadAsset<T>(string assetBundleName, string resourceName) where T : Object
            {
                LoadDependencies(assetBundleName);

                if (!_bundlesDic.TryGetValue(assetBundleName, out AssetBundle assetBundle))
                {
                    assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleName));
                    _bundlesDic.Add(assetBundleName, assetBundle);
                }

                return assetBundle.LoadAsset<T>(resourceName);
            }

            public async void RemoveAssetBundle(string assetBundleName)
            {
                while (_bundlesDic[assetBundleName] == null)
                {
                    await Task.Yield();
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

