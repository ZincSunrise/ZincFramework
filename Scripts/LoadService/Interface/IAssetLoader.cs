using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Events;
using ZincFramework.Threading.Tasks;


namespace ZincFramework.LoadServices
{
    public interface IAssetLoader
    {
        ZincTask InitializeAsync();

        T LoadAsset<T>(object key) where T : Object;

        ZincTask<T> LoadAssetAsync<T>(object key) where T : Object;

        void LoadAssetAsync<T>(object key, ZincAction<T> callback) where T : Object;

        ZincTask<IEnumerable<T>> LoadAssetsAsync<T>(object label) where T : Object;

        void LoadAssetsAsync<T>(object label, ZincAction<IEnumerable<T>> callback) where T : Object;

        void Release<T>(object key) where T : Object;
    }
}