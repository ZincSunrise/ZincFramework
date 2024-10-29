using UnityEngine;
using System.Threading.Tasks;


namespace ZincFramework.LoadServices.Addressable
{
    public abstract class AssetLoader<TKey> : IAssetLoader
    {
        public virtual T LoadAsset<T>(object key) where T : Object => LoadAsset<T>((TKey)key);

        public virtual Task<T> LoadAssetAsync<T>(object key) where T : Object => LoadAssetAsync<T>((TKey)key);

        public virtual void Release<T>(object key) where T : Object => Release<T>((TKey)key);

        public abstract T LoadAsset<T>(TKey key) where T : Object;

        public abstract Task<T> LoadAssetAsync<T>(TKey key) where T : Object;

        public abstract void Release<T>(TKey key) where T : Object;
    }
}