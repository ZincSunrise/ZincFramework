using System;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace ZincFramework.LoadServices.AssetBundles
{
    public readonly struct AssetBundleAwaiter : INotifyCompletion
    {
        public bool IsCompleted => _assetBundleRequest.isDone;

        private readonly AssetBundleRequest _assetBundleRequest;

        public AssetBundleAwaiter(AssetBundleRequest assetBundleRequest)
        {
            _assetBundleRequest = assetBundleRequest;
        }

        public void OnCompleted(Action continuation)
        {
            if (_assetBundleRequest.isDone)
            {
                continuation.Invoke();
            }
            else
            {
                _assetBundleRequest.completed += (x) => continuation.Invoke();
            }
        }

        public UnityEngine.Object GetResult() 
        {
            return _assetBundleRequest.asset;
        }
    }
}