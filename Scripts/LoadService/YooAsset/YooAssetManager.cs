using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using ZincFramework.Events;
using ZincFramework.LoadServices.YooAsset.Packages;
using ZincFramework.Threading.Tasks;


namespace ZincFramework.LoadServices.YooAsset
{
    public static class YooAssetManager
    {
        private readonly static Dictionary<string, AssetHandle> _assetHandles = new Dictionary<string, AssetHandle>();

        private const string _defaultPackageName = "DefaultPackage";

        private static ResourcePackage _defaultPackage;

        public static async ZincTask Initialize(string defaultPackageName = _defaultPackageName, E_BuildType buildType = E_BuildType.EditorSimulate, params string[] packageNames)
        {
            YooAssets.Initialize();
            await InitializePackage(defaultPackageName, buildType);
            _defaultPackage = YooAssets.GetPackage(defaultPackageName);

            if (packageNames != null && packageNames.Length > 0)
            {
                for (int i = 0; i < packageNames.Length; i++)
                {
                    await InitializePackage(packageNames[i], buildType);
                }
            }

            YooAssets.SetDefaultPackage(_defaultPackage);
        }

        private static async ZincTask InitializePackage(string name, E_BuildType buildType)
        {
            var package = YooAssets.CreatePackage(name);

            if (Application.isEditor && buildType == E_BuildType.EditorSimulate)
            {
                await new EditorSimulateOperation().IntializePackageAsync(package);
            }
            else if(buildType == E_BuildType.OfflineBuild)
            {
                await new OfflineBuildOperation().IntializePackageAsync(package);
            }
        }

        public static T LoadAsset<T>(string name) where T : Object
        {
            if (!_assetHandles.TryGetValue(name, out var assetHandle))
            {
                assetHandle = YooAssets.LoadAssetSync<T>(name);
                _assetHandles.Add(name, assetHandle);
            }

            if (!assetHandle.IsDone)
            {
                throw new System.InvalidOperationException("不可以在加载的时候同步加载资源");
            }

            return assetHandle.AssetObject as T;
        }

        public static async ZincTask<T> LoadAssetAsync<T>(string name) where T : Object
        {
            if(!_assetHandles.TryGetValue(name, out var assetHandle))
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

        public static async ZincTask LoadAssetAsync<T>(string name, ZincAction<T> callback) where T : Object
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

            callback?.Invoke(assetHandle.AssetObject as T);
        }

        /// <summary>
        /// 通过LoadAssetsAsync加载的资源不会缓存，资源此时会立刻释放
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="label"></param>
        /// <returns></returns>
        public static async ZincTask<IEnumerable<T>> LoadAssetsAsync<T>(string label) where T : Object
        {
            var assetInfos = _defaultPackage.GetAssetInfos(label);
            List<T> assets = new List<T>();

            for (int i = 0; i < assetInfos.Length; i++)
            {
                AssetHandle assetHandle = _defaultPackage.LoadAssetAsync(assetInfos[i]);
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
        public static async ZincTask LoadAssetsAsync<T>(string label, ZincAction<IEnumerable<T>> callback) where T : Object
        {
            var assetInfos = _defaultPackage.GetAssetInfos(label);
            List<T> assets = new List<T>();

            for (int i = 0; i < assetInfos.Length; i++)
            {
                AssetHandle assetHandle = _defaultPackage.LoadAssetAsync(assetInfos[i]);
                await assetHandle;
                assets.Add(assetHandle.AssetObject as T);
                assetHandle.Release();
            }

            callback?.Invoke(assets);
        }

        public static void Release(string key)
        {
            if (_assetHandles.Remove(key, out var assetHandle))
            {
                assetHandle.Release();
            }
        }
    }
}