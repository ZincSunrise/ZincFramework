using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.AddressableAssets.Addressables;



namespace ZincFramework.LoadServices.Addressable
{
    public class AddressablesManager : BaseSafeSingleton<AddressablesManager>, IDisposable
    {
        private readonly ConcurrentDictionary<string, AddressablesInfo> _handleMap = new ConcurrentDictionary<string, AddressablesInfo>();

        private bool _isUpdated = false;

        private AddressablesManager() { }

        #region 预加载相关
        private IEnumerator C_UpdateCataLogsAsync()
        {
            AsyncOperationHandle<List<string>> handle = UnityEngine.AddressableAssets.Addressables.CheckForCatalogUpdates();

            yield return handle;

            if (handle.Result.Count > 0)
            {
                UnityEngine.AddressableAssets.Addressables.UpdateCatalogs(handle.Result);
            }

            _isUpdated = true;
            yield return 0;
        }

        private async Task T_UpdateCataLogsAsync()
        {
            AsyncOperationHandle<List<string>> handle = UnityEngine.AddressableAssets.Addressables.CheckForCatalogUpdates();

            await handle.Task;

            if (handle.Result.Count > 0)
            {
                AsyncOperationHandle resourcesHandle = UnityEngine.AddressableAssets.Addressables.UpdateCatalogs(handle.Result);
                await resourcesHandle.Task;
            }

            _isUpdated = true;
        }

        /// <summary>
        /// Task版,依据模式来选择加载方式
        /// </summary>
        /// <param name="endCallback">结束时调用的回调函数</param>
        /// <param name="progressCallback">进度时调用的进度回调函数</param>
        /// <param name="keys"></param>
        /// <returns></returns>
        internal async Task PreLoadAssetAsync(Action endCallback, Func<float, Task> progressCallback, MergeMode mergeMode, params string[] keys)
        {
            if (!_isUpdated)
            {
                await T_UpdateCataLogsAsync();
            }

            List<string> names = new List<string>(keys);
            AsyncOperationHandle<long> dataHandle = UnityEngine.AddressableAssets.Addressables.GetDownloadSizeAsync(names);
            await dataHandle.Task;

            if (dataHandle.Result > 0)
            {
                AsyncOperationHandle operationHandle = UnityEngine.AddressableAssets.Addressables.DownloadDependenciesAsync(names, mergeMode);
                DownloadStatus downloadStatus;
                while (!operationHandle.IsDone)
                {
                    downloadStatus = operationHandle.GetDownloadStatus();

                    await progressCallback?.Invoke(downloadStatus.Percent);
                }

                endCallback?.Invoke();
                UnityEngine.AddressableAssets.Addressables.Release(dataHandle);
                UnityEngine.AddressableAssets.Addressables.Release(operationHandle);
            }
            else
            {
                LogUtility.Log("已经加载过了, 请不要重复加载");
            }
        }

        /// <summary>
        /// 协程版,依据模式来选择加载方式
        /// </summary>
        /// <param name="endCallback">结束时调用的回调函数</param>
        /// <param name="progressCallback">进度时调用的进度回调函数</param>
        /// <param name="keys"></param>
        internal void PreLoadAssetAsync(Action endCallback, Action<float> progressCallback, MergeMode mergeMode, params string[] keys)
        {
            MonoManager.Instance.StartCoroutine(RT_PreLoadAssetAsync(endCallback, progressCallback, keys));
        }

        private IEnumerator RT_PreLoadAssetAsync(Action endCallback, Action<float> callback, params string[] keys)
        {
            if (!_isUpdated)
            {
                yield return MonoManager.Instance.StartCoroutine(C_UpdateCataLogsAsync());
            }

            List<string> names = new List<string>(keys);
            AsyncOperationHandle<long> dataHandle = UnityEngine.AddressableAssets.Addressables.GetDownloadSizeAsync(names);
            yield return dataHandle;

            if (dataHandle.Result > 0)
            {
                AsyncOperationHandle operationHandle = UnityEngine.AddressableAssets.Addressables.DownloadDependenciesAsync(names, MergeMode.Union);
                DownloadStatus downloadStatus;
                while (!operationHandle.IsDone)
                {
                    downloadStatus = operationHandle.GetDownloadStatus();
                    callback?.Invoke(downloadStatus.Percent);
                    yield return 0;
                }

                endCallback?.Invoke();
                UnityEngine.AddressableAssets.Addressables.Release(dataHandle);
            }
            else
            {
                LogUtility.Log("已经加载过了, 请不要重复加载");
            }
        }
        #endregion


        #region 加载单个资源相关
        /// <summary>
        /// 加载单个资源的函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">资源的名字或者标签</param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public async void LoadAssetAsync<T>(string name, Action<T> callback) where T : class
        {
            await RT_LoadAssetAsync(name, callback);

            async Task RT_LoadAssetAsync(string name, Action<T> callback)
            {
                string keyName = name + '_' + typeof(T).Name;
                AsyncOperationHandle<T> tempHandle;
                if (!_handleMap.TryGetValue(keyName, out AddressablesInfo info))
                {
                    tempHandle = Addressables.LoadAssetAsync<T>(name);
                    info = new AddressablesInfo(tempHandle, keyName);

                    _handleMap.TryAdd(keyName, info);
                    await tempHandle.Task;
                }
                else
                {
                    tempHandle = info.AssetHandle.Convert<T>();

                    if (!tempHandle.IsDone)
                    {
                        await tempHandle.Task;
                    }
                }


                if (tempHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    LogUtility.LogError($"不存在名字或标签为{name}的资源");
                    if (_handleMap.ContainsKey(keyName))
                    {
                        _handleMap.TryRemove(info.AssetName, out _); ;
                        Addressables.Release(info.AssetHandle);
                    }
                }

                callback.Invoke(tempHandle.Result);
            }
        }

        public async Task<T> LoadAssetAsync<T>(string name) where T : class
        {
            string keyName = name + '_' + typeof(T).Name;
            AsyncOperationHandle<T> tempHandle;

            if (!_handleMap.TryGetValue(keyName, out AddressablesInfo info))
            {
                tempHandle = Addressables.LoadAssetAsync<T>(name);
                info = new AddressablesInfo(tempHandle, keyName);
                _handleMap.TryAdd(keyName, info);

                await tempHandle.Task;
            }
            else
            {
                tempHandle = info.AssetHandle.Convert<T>();

                if (!tempHandle.IsDone)
                {
                    await tempHandle.Task;
                }
            }


            if (tempHandle.Status != AsyncOperationStatus.Succeeded)
            {
                if (_handleMap.ContainsKey(keyName))
                {
                    _handleMap.TryRemove(info.AssetName, out _); ;
                    Addressables.Release(info.AssetHandle);
                }
                throw new ArgumentException($"不存在名字或标签为{name}的资源");
            }

            return tempHandle.Result;
        }

        public T LoadAsset<T>(string name) where T : class
        {
            string keyName = name + '_' + typeof(T).Name;
            AsyncOperationHandle<T> tempHandle;

            if (!_handleMap.TryGetValue(keyName, out AddressablesInfo info))
            {
                tempHandle = Addressables.LoadAssetAsync<T>(name);
                info = new AddressablesInfo(tempHandle, keyName);
                _handleMap.TryAdd(keyName, info);

                tempHandle.Task.Wait();
            }
            else
            {
                tempHandle = info.AssetHandle.Convert<T>();

                if (!tempHandle.IsDone)
                {
                    tempHandle.Task.Wait();
                }
            }

            if (tempHandle.Status != AsyncOperationStatus.Succeeded)
            {
                if (_handleMap.ContainsKey(keyName))
                {
                    _handleMap.TryRemove(info.AssetName, out _);
                    Addressables.Release(tempHandle);
                }
                throw new ArgumentException($"不存在名字或标签为{name}的资源");
            }

            return tempHandle.Task.Result;
        }

        /// <summary>
        /// 加载单个资源的函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">资源的名字或者标签</param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>         
        public void LoadAssetCoroutineAsync<T>(string name, Action<T> callback) where T : class
        {
            MonoManager.Instance.StartCoroutine(RC_LoadAssetAsync(name, callback));

            IEnumerator RC_LoadAssetAsync(string name, Action<T> callback)
            {
                string keyName = name + "_" + typeof(T).Name;
                AsyncOperationHandle<T> tempHandle;

                if (!_handleMap.TryGetValue(keyName, out AddressablesInfo info))
                {
                    tempHandle = Addressables.LoadAssetAsync<T>(name);
                    info = new AddressablesInfo(tempHandle, keyName);

                    _handleMap.TryAdd(keyName, info);
                    yield return tempHandle;
                }
                else
                {
                    tempHandle = info.AssetHandle.Convert<T>();

                    if (!tempHandle.IsDone)
                    {
                        yield return tempHandle;
                    }
                }


                if (tempHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    LogUtility.LogError($"不存在名字或标签为{name}的资源");
                    if (_handleMap.ContainsKey(keyName))
                    {
                        _handleMap.TryRemove(info.AssetName, out _); ;
                        Addressables.Release(tempHandle);
                    }
                }

                callback.Invoke(tempHandle.Result);
            }
        }

        #endregion


        #region 加载多个资源相关

        /// <summary>
        /// 加载多个资源的异步函数，注意，不可以在第一次加载时在同一帧反复修改自动删除的bool值变量
        /// </summary>
        /// <typeparam name="T">泛型种类</typeparam>
        /// <param name="labelsOrNames">传入的字符串数组</param>
        /// <param name="callback">回调函数</param>
        /// <param name="mergeMode">合并模式</param>
        /// <param name="isAutoClear">是否加载完后自动清除</param>
        /// <returns></returns>
        public async void LoadAssetsAsync<T>(IList<string> labelsOrNames, Action<T> callback, MergeMode mergeMode, bool isAutoClear = false) where T : class
        {
            await RT_LoadAssetsAsync(labelsOrNames, callback, mergeMode, isAutoClear);
        }


        public async Task<IList<T>> LoadAssetsAsync<T>(IList<string> labelsOrNames, MergeMode mergeMode)
        {
            string keyName = string.Join('_', labelsOrNames) + typeof(T).Name;

            AsyncOperationHandle<IList<T>> handle;
            if (!_handleMap.TryGetValue(keyName, out AddressablesInfo info))
            {
                handle = UnityEngine.AddressableAssets.Addressables.LoadAssetsAsync<T>(labelsOrNames, null, mergeMode);
                info = new AddressablesInfo(handle, keyName);
                _handleMap.TryAdd(keyName, info);

                await handle.Task;

                if (handle.Status != AsyncOperationStatus.Succeeded)
                {
                    throw new ArgumentException($"传入的{keyName}有问题，请检查");
                }

                return handle.Result;
            }


            handle = info.AssetHandle.Convert<IList<T>>();
            if (!handle.IsDone)
            {
                await handle.Task;
            }

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new ArgumentException($"传入的{keyName}有问题，请检查");
            }

            info.AddCount();
            return handle.Result;
        }


        public async Task<IList<T>> LoadAssetsAsync<T>(string labelsOrName)
        {
            string keyName = labelsOrName + typeof(T).Name;

            AsyncOperationHandle<IList<T>> handle;
            if (!_handleMap.TryGetValue(keyName, out AddressablesInfo info))
            {
                handle = UnityEngine.AddressableAssets.Addressables.LoadAssetsAsync<T>(new string[] { labelsOrName } as IEnumerable, null, MergeMode.Union);
                info = new AddressablesInfo(handle, keyName);
                _handleMap.TryAdd(keyName, info);

                await handle.Task;

                if (handle.Status != AsyncOperationStatus.Succeeded)
                {
                    throw new ArgumentException($"传入的{keyName}有问题，请检查");
                }

                return handle.Result;
            }


            handle = info.AssetHandle.Convert<IList<T>>();
            if (!handle.IsDone)
            {
                await handle.Task;
            }

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new ArgumentException($"传入的{keyName}有问题，请检查");
            }

            info.AddCount();
            return handle.Result;
        }


        public void LoadAssetsCAsync<T>(IList<string> labelsOrNames, Action<T> callback, MergeMode mergeMode, bool isAutoClear = false)
        {
            MonoManager.Instance.StartCoroutine(RC_LoadAssetsAsync<T>(labelsOrNames, callback, mergeMode, isAutoClear));
        }

        private async Task RT_LoadAssetsAsync<T>(IList<string> labelsOrNames, Action<T> callback, MergeMode mergeMode, bool isAutoClear)
        {
            string keyName = string.Join('_', labelsOrNames) + typeof(T).Name;

            AsyncOperationHandle<IList<T>> handle;
            if (!_handleMap.TryGetValue(keyName, out AddressablesInfo info))
            {
                handle = UnityEngine.AddressableAssets.Addressables.LoadAssetsAsync<T>(labelsOrNames, callback, mergeMode);
                if (!isAutoClear)
                {
                    info = new AddressablesInfo(handle, keyName);
                    _handleMap.TryAdd(keyName, info);
                }

                await handle.Task;

                if (isAutoClear)
                {
                    UnityEngine.AddressableAssets.Addressables.Release(handle);
                }
            }
            else if (!isAutoClear)
            {
                handle = info.AssetHandle.Convert<IList<T>>();
                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    info.AddCount();
                    foreach (var item in handle.Result)
                    {
                        callback?.Invoke(item);
                    }
                }
                else
                {
                    LogUtility.LogError($"不存在名字或标签为{keyName}的资源");
                    if (_handleMap.ContainsKey(keyName))
                    {
                        _handleMap.TryRemove(info.AssetName, out _); ;
                        UnityEngine.AddressableAssets.Addressables.Release(info.AssetHandle);
                    }
                }
            }
            else
            {
                throw new ArgumentException("请不要反复变更自动删除的标识");
            }
        }

        private IEnumerator RC_LoadAssetsAsync<T>(IList<string> labelsOrNames, Action<T> callback, MergeMode mergeMode, bool isAutoClear)
        {
            string keyName = string.Join('_', labelsOrNames) + typeof(T).Name;

            AsyncOperationHandle<IList<T>> handle;
            if (!_handleMap.TryGetValue(keyName, out AddressablesInfo info))
            {
                handle = UnityEngine.AddressableAssets.Addressables.LoadAssetsAsync<T>(labelsOrNames, callback, mergeMode);
                if (!isAutoClear)
                {
                    info = new AddressablesInfo(handle, keyName);
                    _handleMap.TryAdd(keyName, info);
                }

                yield return handle;

                if (isAutoClear)
                {
                    UnityEngine.AddressableAssets.Addressables.Release(handle);
                }
            }
            else if (!isAutoClear)
            {
                handle = info.AssetHandle.Convert<IList<T>>();
                yield return handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    info.AddCount();
                    foreach (var item in handle.Result)
                    {
                        callback?.Invoke(item);
                    }
                }
                else
                {
                    LogUtility.LogError($"不存在名字或标签为{keyName}的资源");
                    if (_handleMap.ContainsKey(keyName))
                    {
                        _handleMap.TryRemove(info.AssetName, out _); ;
                        UnityEngine.AddressableAssets.Addressables.Release(info.AssetHandle);
                    }
                }
            }
            else
            {
                throw new ArgumentException("请不要反复变更自动删除的标识");
            }
        }



        /// <summary>
        /// 加载多个资源的异步函数，注意，不可以在第一次加载时在同一帧反复修改自动删除的bool值变量
        /// </summary>
        /// <typeparam name="T">泛型种类</typeparam>
        /// <param name="callback">回调函数</param>
        /// <param name="mergeMode">合并模式</param>
        /// <param name="isAutoClear">是否加载完后自动清除</param>
        /// <param name="nameOrlabelsOrNames">传入的字符串数组</param>
        /// <returns></returns>
        public async Task LoadAssetsTAsync<T>(Action<T> callback, MergeMode mergeMode, bool isAutoClear = false, params string[] nameOrlabelsOrNames)
        {
            await RT_LoadAssetsAsync(callback, mergeMode, isAutoClear, nameOrlabelsOrNames);


        }
        public void LoadAssetsCAsync<T>(Action<T> callback, MergeMode mergeMode, bool isAutoClear = false, params string[] nameOrlabelsOrNames)
        {
            MonoManager.Instance.StartCoroutine(RC_LoadAssetsAsync<T>(callback, mergeMode, isAutoClear, nameOrlabelsOrNames));
        }


        private IEnumerator RC_LoadAssetsAsync<T>(Action<T> callback, MergeMode mergeMode = MergeMode.Intersection, bool isAutoClear = false, params string[] labelsOrNames)
        {
            string keyName = string.Join('_', labelsOrNames) + typeof(T).Name;

            AsyncOperationHandle<IList<T>> handle;
            if (!_handleMap.TryGetValue(keyName, out AddressablesInfo info))
            {
                handle = UnityEngine.AddressableAssets.Addressables.LoadAssetsAsync<T>(labelsOrNames as IEnumerable, callback, mergeMode);
                if (!isAutoClear)
                {
                    info = new AddressablesInfo(handle, keyName);
                    _handleMap.TryAdd(keyName, info);
                }

                yield return handle;

                if (isAutoClear)
                {
                    UnityEngine.AddressableAssets.Addressables.Release(handle);
                }
            }
            else if (!isAutoClear)
            {
                handle = info.AssetHandle.Convert<IList<T>>();
                yield return handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    info.AddCount();
                    foreach (var item in handle.Result)
                    {
                        callback?.Invoke(item);
                    }
                }
                else
                {
                    LogUtility.LogError($"不存在名字或标签为{keyName}的资源");
                    if (_handleMap.ContainsKey(keyName))
                    {
                        _handleMap.TryRemove(info.AssetName, out _); ;
                        UnityEngine.AddressableAssets.Addressables.Release(info.AssetHandle);
                    }
                }
            }
            else
            {
                throw new ArgumentException("请不要反复变更自动删除的标识");
            }
        }

        private async Task RT_LoadAssetsAsync<T>(Action<T> callback, MergeMode mergeMode = MergeMode.Intersection, bool isAutoClear = false, params string[] labelsOrNames)
        {
            string keyName = string.Join('_', labelsOrNames) + typeof(T).Name;

            AsyncOperationHandle<IList<T>> handle;
            if (!_handleMap.TryGetValue(keyName, out AddressablesInfo info))
            {
                handle = UnityEngine.AddressableAssets.Addressables.LoadAssetsAsync<T>(labelsOrNames as IEnumerable, callback, mergeMode);
                if (!isAutoClear)
                {
                    info = new AddressablesInfo(handle, keyName);
                    _handleMap.TryAdd(keyName, info);
                }

                await handle.Task;

                if (isAutoClear)
                {
                    UnityEngine.AddressableAssets.Addressables.Release(handle);
                }
            }
            else if (!isAutoClear)
            {
                handle = info.AssetHandle.Convert<IList<T>>();
                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    info.AddCount();
                    foreach (var item in handle.Result)
                    {
                        callback?.Invoke(item);
                    }
                }
                else
                {
                    LogUtility.LogError($"不存在名字或标签为{keyName}的资源");
                    if (_handleMap.ContainsKey(keyName))
                    {
                        _handleMap.TryRemove(info.AssetName, out _); ;
                        UnityEngine.AddressableAssets.Addressables.Release(info.AssetHandle);
                    }
                }
            }
            else
            {
                throw new ArgumentException("请不要反复变更自动删除的标识");
            }
        }
        #endregion


        #region 释放资源相关
        public void Release<T>(string name)
        {
            string keyName = name + '_' + typeof(T).Name;
            if (_handleMap.TryGetValue(keyName, out AddressablesInfo info))
            {
                info.SubtractCount();
            }
        }

        public void Release<T>(params string[] labelsOrNames)
        {
            string keyName = string.Join('_', labelsOrNames) + typeof(T).Name;

            if (_handleMap.TryGetValue(keyName, out AddressablesInfo info))
            {
                info.SubtractCount();
            }
        }

        public void Release<T>(List<string> labelsOrNames)
        {
            string keyName = string.Join('_', labelsOrNames) + typeof(T).Name;

            if (_handleMap.TryGetValue(keyName, out AddressablesInfo info))
            {
                info.SubtractCount();
            }
        }

        internal void Release(AddressablesInfo info)
        {
            _handleMap.TryRemove(info.AssetName, out _);
            UnityEngine.AddressableAssets.Addressables.Release(info.AssetHandle);
        }

        internal void Clear(bool isDefinitelyClear = false)
        {
            MonoManager.Instance.StopAllCoroutines();
            foreach (AddressablesInfo info in _handleMap.Values)
            {
                UnityEngine.AddressableAssets.Addressables.Release(info.AssetHandle);
            }
            if (isDefinitelyClear)
            {
                AssetBundle.UnloadAllAssetBundles(true);
            }


            Resources.UnloadUnusedAssets();
            _handleMap.Clear();
            GC.Collect();
        }

        public void Dispose()
        {
            Clear();
            _instance = new Lazy<AddressablesManager>(() => new AddressablesManager());
        }
        #endregion
    }
}


