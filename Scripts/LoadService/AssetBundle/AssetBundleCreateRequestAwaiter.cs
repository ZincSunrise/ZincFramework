using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using ZincFramework.Pools;


namespace ZincFramework.LoadServices.AssetBundles
{
    public readonly struct AssetBundleCreateRequestAwaiter : INotifyCompletion, ICriticalNotifyCompletion
    {
        private readonly AssetBundleCreateRequest _assetBundleCreateRequest;

        public bool IsCompleted => _assetBundleCreateRequest.isDone;

        public AssetBundleCreateRequestAwaiter(AssetBundleCreateRequest assetBundleCreateRequest)
        {
            _assetBundleCreateRequest = assetBundleCreateRequest;
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public readonly AssetBundle GetResult() => _assetBundleCreateRequest.assetBundle;

        public void UnsafeOnCompleted(Action continuation)
        {
            _assetBundleCreateRequest.completed += PooledDelegate<AsyncOperation>.Create(continuation);
        }
    }
}