using System;
using System.Runtime.CompilerServices;
using YooAsset;
using ZincFramework.Pools;

namespace ZincFramework.LoadServices.YooAsset
{
    public readonly struct AssetHandleAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        public bool IsCompleted => _assetHandle.IsDone;


        private readonly AssetHandle _assetHandle;

        public Object GetResult()
        {
            return _assetHandle.AssetObject;
        }

        public AssetHandleAwaiter(AssetHandle assetHandle)
        {
            _assetHandle = assetHandle;
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _assetHandle.Completed += PooledDelegate<AssetHandle>.Create(continuation);
        }
    }

}