using System.Collections.Generic;
using ZincFramework.Threading.Tasks;



namespace ZincFramework.LoadServices.AssetBundles
{
    public class AssetBundleLoader : AssetLoader<AssetBundlePair>
    {
        public override ZincTask InitializeAsync()
        {
            return new ZincTask();
        }

        public override T LoadAsset<T>(AssetBundlePair key)
        {
            return AssetBundleManager.Instance.LoadAsset<T>(key.AssetName, key.BundleName);
        }

        public override async ZincTask<T> LoadAssetAsync<T>(AssetBundlePair key)
        {
            return await AssetBundleManager.Instance.LoadAssetAsync<T>(key.AssetName, key.BundleName);
        }

        public override void Release<T>(AssetBundlePair key)
        {
            //AssetBundleManager.Instance.RemoveAssetBundle(key.AssetName);
        }

        public override void LoadAssetAsync<T>(AssetBundlePair key, Events.ZincAction<T> callback)
        {
            throw new System.NotImplementedException();
        }

        public override ZincTask<IEnumerable<T>> LoadAssetsAsync<T>(AssetBundlePair label)
        {
            throw new System.NotImplementedException();
        }

        public override void LoadAssetsAsync<T>(AssetBundlePair label, Events.ZincAction<IEnumerable<T>> callback)
        {
            throw new System.NotImplementedException();
        }
    }
}
