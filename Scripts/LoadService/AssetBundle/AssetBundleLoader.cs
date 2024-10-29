using System.Threading.Tasks;
using UnityEngine;



namespace ZincFramework.LoadServices.Addressable
{
    public class AssetBundleLoader : AssetLoader<AssetBundlePair>
    {
        public override T LoadAsset<T>(object key) => LoadAsset<T>(new AssetBundlePair(key.ToString()));

        public override async Task<T> LoadAssetAsync<T>(object key) => await LoadAssetAsync<T>(new AssetBundlePair(key.ToString()));

        public override T LoadAsset<T>(AssetBundlePair key)
        {
            return AssetBundleManager.Instance.LoadAsset<T>(key.AssetName, key.BundleName);
        }

        public override async Task<T> LoadAssetAsync<T>(AssetBundlePair key)
        {
            return await AssetBundleManager.Instance.LoadAssetAsync<T>(key.AssetName, key.BundleName);
        }

        public override void Release<T>(AssetBundlePair key)
        {
            //AssetBundleManager.Instance.RemoveAssetBundle(key.AssetName);
        }
    }
}
