using UnityEngine.ResourceManagement.AsyncOperations;


namespace ZincFramework.LoadServices.Addressable
{
    internal class AddressablesInfo
    {
        public ref AsyncOperationHandle AssetHandle => ref _assetHandle;

        public string AssetName { get; }

        private uint _assetRefCount;

        private AsyncOperationHandle _assetHandle;

        public void AddCount()
        {
            _assetRefCount++;
        }

        public void SubtractCount()
        {
            _assetRefCount--;
            if (_assetRefCount <= 0)
            {
                AddressablesManager.Instance.Release(this);
            }
        }

        public AddressablesInfo(AsyncOperationHandle assetHandle, string assetName)
        {
            _assetHandle = assetHandle;
            this.AssetName = assetName;
            _assetRefCount = 0;
        }
    }
}