using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZincFramework.Threading.Tasks;



namespace ZincFramework.LoadServices
{
    /// <summary>
    /// 加载系统总核心，需要通过Initialize方法传入一个加载器来进行加载
    /// </summary>
    public class AssetLoadManager
    {
        private static IAssetLoader _assetLoader;

        private static AssetLoader<string> _strLoader;

        public static ZincTask Initialize(IAssetLoader assetLoader)
        {
            if(assetLoader is AssetLoader<string> strLoader)
            {
                _strLoader = strLoader;
            }

            _assetLoader = assetLoader;
            return _assetLoader.InitializeAsync();
        }

        public static T LoadAsset<T>(object key) where T : Object
        {
            return _assetLoader.LoadAsset<T>(key);
        }

        public static async Task<T> LoadAssetAsync<T>(object key) where T : Object
        {
            return await _assetLoader.LoadAssetAsync<T>(key);
        }

        public static async Task<IEnumerable<T>> LoadAssetsAsync<T>(object key) where T : Object
        {
            return await _assetLoader.LoadAssetsAsync<T>(key);
        }

        public static T LoadAsset<T>(string key) where T : Object
        {
            return _strLoader?.LoadAsset<T>(key);
        }

        public static async ZincTask<T> LoadAssetAsync<T>(string key) where T : Object
        {
            return await _strLoader.LoadAssetAsync<T>(key);
        }

        public static async ZincTask<IEnumerable<T>> LoadAssetsAsync<T>(string key) where T : Object
        {
            return await _strLoader.LoadAssetsAsync<T>(key);
        }
    }
}
