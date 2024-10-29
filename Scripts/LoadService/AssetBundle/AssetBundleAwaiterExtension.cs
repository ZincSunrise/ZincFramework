using UnityEngine;


namespace ZincFramework.LoadServices.AssetBundles
{
    public static class AssetBundleAwaiterExtension
    {
        public static AssetBundleAwaiter GetAwaiter(this AssetBundleRequest assetBundleRequest)
        {
            return new AssetBundleAwaiter(assetBundleRequest);
        }

        public static AssetBundleCreateRequestAwaiter GetAwaiter(this AssetBundleCreateRequest assetBundleCreateRequest)
        {
            return new AssetBundleCreateRequestAwaiter(assetBundleCreateRequest);
        }
    }
}

