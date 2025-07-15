using System;
using UnityEngine;
using ZincFramework.Pools;
using ZincFramework.Threading.Tasks;


namespace ZincFramework.LoadServices.AssetBundles
{
    public class AssetBundlePromise : CorePromise, IReuseable
    {
        private readonly static DataPool<AssetBundlePromise> _promisePool = new DataPool<AssetBundlePromise>(() => new AssetBundlePromise());

        private AssetBundlePromise() { }

        private AssetBundleRequest _assetBundleRequest;

        public void Initialize(AssetBundleRequest assetBundleRequest) 
        {
            _assetBundleRequest = assetBundleRequest;
            _assetBundleRequest.completed += OnLoadComplete;
        }

        private void OnLoadComplete(AsyncOperation obj)
        {
            _sourceCore.TrySetResult(obj);
        }

        public override void GetResult()
        {
            try
            {
                _sourceCore.GetResult();
            }
            finally
            {
                if (!_isDisposed)
                {
                    _isDisposed = true;
                    _promisePool.ReturnValue(this);
                }
                else
                {
                    throw new ObjectDisposedException("不可以重复获取同一个对象内的资源");
                }
            }
        }

        public override void OnReturn()
        {
            base.OnReturn();
            _assetBundleRequest = null;
        }
    }
}
