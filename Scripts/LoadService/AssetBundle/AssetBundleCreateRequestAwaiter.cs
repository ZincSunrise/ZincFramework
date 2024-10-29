using System;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace ZincFramework.LoadServices.AssetBundles
{
    public struct AssetBundleCreateRequestAwaiter : INotifyCompletion
    {
        private readonly AssetBundleCreateRequest _assetBundleCreateRequest;

        public bool IsCompleted { get; private set; }

        public AssetBundleCreateRequestAwaiter(AssetBundleCreateRequest assetBundleCreateRequest)
        {
            _assetBundleCreateRequest = assetBundleCreateRequest;
            IsCompleted = false;
        }

        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }

        public readonly AssetBundle GetResult() => _assetBundleCreateRequest.assetBundle;
    }
}