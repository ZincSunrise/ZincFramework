using UnityEngine;
using System.Threading.Tasks;


namespace ZincFramework.LoadServices.Addressable
{
    public interface IAssetLoader
    {
        T LoadAsset<T>(object key) where T : Object;

        Task<T> LoadAssetAsync<T>(object key) where T : Object;

        void Release<T>(object key) where T : Object;
    }
}