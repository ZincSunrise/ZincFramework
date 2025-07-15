using YooAsset;

namespace ZincFramework.LoadServices.YooAsset
{
    public static class AssetHandleExtension
    {
        public static AssetHandleAwaiter GetAwaiter(this AssetHandle assetHandle)
        {
            return new AssetHandleAwaiter(assetHandle);
        }
    }

}