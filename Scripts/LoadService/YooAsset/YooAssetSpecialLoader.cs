using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using ZincFramework.Events;
using ZincFramework.Threading.Tasks;

namespace ZincFramework.LoadServices.YooAsset
{
    public class YooAssetSpecialLoader : AssetLoader<string>
    {
        private readonly ResourcePackage _resourcePackage;

        private readonly Dictionary<string, AssetHandle> _assetHandles = new Dictionary<string, AssetHandle>();

        public YooAssetSpecialLoader(ResourcePackage resourcePackage)
        {
            _resourcePackage = resourcePackage;
        }

        public override ZincTask InitializeAsync()
        {
            if (!YooAssets.Initialized)
            {
                return YooAssetManager.Initialize();
            }

            return new ZincTask();
        }

        public override T LoadAsset<T>(string key)
        {
            if(!_assetHandles.TryGetValue(key, out var assetHandle))
            {
                assetHandle = _resourcePackage.LoadAssetSync<T>(key);
                _assetHandles.Add(key, assetHandle);
            }

            if (!assetHandle.IsDone)
            {
                throw new System.InvalidOperationException("不可以在加载的时候同步加载资源");
            }


            return assetHandle.AssetObject as T;
        }


        public override async ZincTask<T> LoadAssetAsync<T>(string name)
        {
            if (!_assetHandles.TryGetValue(name, out var assetHandle))
            {
                assetHandle = YooAssets.LoadAssetAsync<T>(name);
                _assetHandles.Add(name, assetHandle);
            }

            if (!assetHandle.IsDone)
            {
                await assetHandle;
            }

            return assetHandle.AssetObject as T;
        }


        public override void LoadAssetAsync<T>(string name, ZincAction<T> callback)
        {
            if (!_assetHandles.TryGetValue(name, out var assetHandle))
            {
                assetHandle = _resourcePackage.LoadAssetAsync<T>(name);
                _assetHandles.Add(name, assetHandle);
            }

            if (!assetHandle.IsDone)
            {
                assetHandle.Completed += x => callback?.Invoke(x.AssetObject as T);
            }
            else
            {
                callback?.Invoke(assetHandle.AssetObject as T);
            }
        }


        /// <summary>
        /// 通过LoadAssetsAsync加载的资源不会缓存，资源此时会立刻释放
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="label"></param>
        /// <returns></returns>
        public override async ZincTask<IEnumerable<T>> LoadAssetsAsync<T>(string label)
        {
            var assetInfos = _resourcePackage.GetAssetInfos(label);
            List<T> assets = new List<T>();

            for (int i = 0; i < assetInfos.Length; i++)
            {
                AssetHandle assetHandle = _resourcePackage.LoadAssetAsync(assetInfos[i]);
                await assetHandle;
                assets.Add(assetHandle.AssetObject as T);
                assetHandle.Release();
            }

            return assets;
        }

        /// <summary>
        /// 通过LoadAssetsAsync加载的资源不会缓存，资源此时会立刻释放
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="label"></param>
        /// <returns></returns>
        public override void LoadAssetsAsync<T>(string label, ZincAction<IEnumerable<T>> callback)
        {
            LoadAssetsAsyncInternal(label, callback).Forget();
        }


        private async ZincTask LoadAssetsAsyncInternal<T>(string label, ZincAction<IEnumerable<T>> callback) where T : Object
        {
            var assetInfos = _resourcePackage.GetAssetInfos(label);
            List<T> assets = new List<T>();

            for (int i = 0; i < assetInfos.Length; i++)
            {
                AssetHandle assetHandle = _resourcePackage.LoadAssetAsync(assetInfos[i]);
                await assetHandle;
                assets.Add(assetHandle.AssetObject as T);
                assetHandle.Release();
            }

            callback?.Invoke(assets);
        }


        public override void Release<T>(string key)
        {
            if (_assetHandles.Remove(key, out var assetHandle))
            {
                assetHandle.Release();
            }
        }
    }
}