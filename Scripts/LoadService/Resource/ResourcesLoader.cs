using System.Collections.Generic;
using ZincFramework.Threading.Tasks;



namespace ZincFramework.LoadServices.Resource
{
    public class ResourcesLoader : AssetLoader<string>
    {
        public override ZincTask InitializeAsync()
        {
            return new ZincTask();
        }

        public override T LoadAsset<T>(string key)
        {
            return ResourcesManager.LoadAsset<T>(key);
        }

        public override async ZincTask<T> LoadAssetAsync<T>(string key)
        {
            return await ResourcesManager.LoadAssetAsync<T>(key);
        }

        public override void LoadAssetAsync<T>(string key, Events.ZincAction<T> callback)
        {
            ResourcesManager.LoadAssetAsync<T>(key, callback);
        }

        public override ZincTask<IEnumerable<T>> LoadAssetsAsync<T>(string key)
        {
            throw new System.NotImplementedException("Resources不支持批量加载!");
        }

        public override void LoadAssetsAsync<T>(string label, Events.ZincAction<IEnumerable<T>> callback)
        {
            throw new System.NotImplementedException("Resources不支持批量加载!");
        }

        public override void Release<T>(string key)
        {
            ResourcesManager.Release<T>(key);
        }
    }
}