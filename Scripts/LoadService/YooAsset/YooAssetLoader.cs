using System.Collections.Generic;
using YooAsset;
using ZincFramework.Threading.Tasks;

namespace ZincFramework.LoadServices.YooAsset
{
    public class YooAssetLoader : AssetLoader<string>
    {
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
            return YooAssetManager.LoadAsset<T>(key);
        }

        public override async ZincTask<T> LoadAssetAsync<T>(string key)
        {
            return await YooAssetManager.LoadAssetAsync<T>(key);
        }

        public override void LoadAssetAsync<T>(string key, Events.ZincAction<T> callback)
        {
            YooAssetManager.LoadAssetAsync<T>(key, callback).Forget();
        }

        public override async ZincTask<IEnumerable<T>> LoadAssetsAsync<T>(string label)
        {
            return await YooAssetManager.LoadAssetsAsync<T>(label);
        }

        public override void LoadAssetsAsync<T>(string label, Events.ZincAction<IEnumerable<T>> callback)
        {
            YooAssetManager.LoadAssetsAsync<T>(label, callback).Forget();
        }

        public override void Release<T>(string key)
        {
            YooAssetManager.Release(key);
        }
    }
}