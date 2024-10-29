namespace ZincFramework.Analysis
{
    public interface IAssetRepository<TBase>
    {
        void RegistUser(TBase assetUser);

        void UnRegistUser(TBase assetUser);

        T GetComponent<T>(TBase assetUser) where T : TBase;


        TAsset GetAsset<TAsset>(string argument) where TAsset : TBase;

        void AddAsset<TAsset>(string argument, TAsset asset) where TAsset : TBase;

        void DeleteAsset<TAsset>(string argument) where TAsset : TBase;

        bool UpdateAsset<TAsset>(string argument, TAsset asset) where TAsset : TBase;
    }
}