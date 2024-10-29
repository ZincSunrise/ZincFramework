using System.Threading.Tasks;
using ZincFramework.LoadServices.Addressable;



namespace ZincFramework.LoadServices.Resource
{
    public class ResourcesLoader : AssetLoader<string>
    {
        public override T LoadAsset<T>(string key)
        {
            return ResourcesManager.Instance.Load<T>(key);
        }

        public override async Task<T> LoadAssetAsync<T>(string key)
        {
            return await ResourcesManager.Instance.LoadAsync<T>(key);
        }

        public override void Release<T>(string key)
        {
            ResourcesManager.Instance.Release<T>(key);
        }
    }
}