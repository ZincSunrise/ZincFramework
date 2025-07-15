using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using ZincFramework.Pools;


namespace ZincFramework.LoadServices.AssetBundles
{
    public readonly struct AssetBundleAwaiter : INotifyCompletion, ICriticalNotifyCompletion 
    {
        public bool IsCompleted => _assetBundleRequest.isDone;

        private readonly AssetBundleRequest _assetBundleRequest;

        public AssetBundleAwaiter(AssetBundleRequest assetBundleRequest)
        {
            _assetBundleRequest = assetBundleRequest;
        }

        public UnityEngine.Object GetResult() 
        {
            return _assetBundleRequest.asset;
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _assetBundleRequest.completed += PooledDelegate<AsyncOperation>.Create(continuation);
        }
    }
}