using System.Threading.Tasks;


namespace ZincFramework.LoadServices.Addressable
{
    public class AddressablesLoader : AssetLoader<string>
    {
        public override T LoadAsset<T>(string key)
        {
            return AddressablesManager.Instance.LoadAsset<T>(key);
        }

        public override async Task<T> LoadAssetAsync<T>(string key)
        {
            return await AddressablesManager.Instance.LoadAssetAsync<T>(key);
        }

        public override void Release<T>(string key)
        {
            AddressablesManager.Instance.Release<T>(key);
        }
    }
}
