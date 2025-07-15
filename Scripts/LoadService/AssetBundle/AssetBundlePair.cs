namespace ZincFramework.LoadServices.AssetBundles
{
    public class AssetBundlePair
    {
        public string BundleName { get; }

        public string AssetName { get; }

        public AssetBundlePair(string bundleName, string assetName)
        {
            BundleName = bundleName;
            AssetName = assetName;
        }

        public AssetBundlePair(string normalText)
        {
            int index = normalText.IndexOf('/');
            BundleName = normalText[..index];
            AssetName = normalText[(index + 1)..];
        }

        public override string ToString() => $"{BundleName}/{AssetName}";
    }
}