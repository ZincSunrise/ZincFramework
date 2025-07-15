using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Events;
using ZincFramework.Threading.Tasks;


namespace ZincFramework.LoadServices
{
    public abstract class AssetLoader<TKey> : IAssetLoader
    {
        #region 显式引用
        T IAssetLoader.LoadAsset<T>(object key) => LoadAsset<T>((TKey)key);

        ZincTask<T> IAssetLoader.LoadAssetAsync<T>(object key) => LoadAssetAsync<T>((TKey)key);

        void IAssetLoader.LoadAssetAsync<T>(object key, ZincAction<T> callback) => LoadAssetAsync<T>((TKey)key, callback);


        void IAssetLoader.Release<T>(object key) => Release<T>((TKey)key);

        ZincTask<IEnumerable<T>> IAssetLoader.LoadAssetsAsync<T>(object key) => LoadAssetsAsync<T>((TKey)key);

        void IAssetLoader.LoadAssetsAsync<T>(object key, ZincAction<IEnumerable<T>> callback) => LoadAssetsAsync<T>((TKey)key, callback);
        #endregion

        /// <summary>
        /// 初始化函数
        /// </summary>
        public abstract ZincTask InitializeAsync();

        /// <summary>
        /// 同步加载某一个资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract T LoadAsset<T>(TKey key) where T : Object;

        /// <summary>
        /// 异步加载某一个资源，可以等待
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract ZincTask<T> LoadAssetAsync<T>(TKey key) where T : Object;

        /// <summary>
        /// 异步加载某一个资源，完成后触发回调函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        public abstract void LoadAssetAsync<T>(TKey key, ZincAction<T> callback) where T : Object;

        /// <summary>
        /// 异步加载所有同标签资源，可以等待
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="label"></param>
        /// <returns></returns>
        public abstract ZincTask<IEnumerable<T>> LoadAssetsAsync<T>(TKey label) where T : Object;


        /// <summary>
        /// 异步加载所有同标签资源，完成后触发回调函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="label"></param>
        /// <param name="callback"></param>
        public abstract void LoadAssetsAsync<T>(TKey label, ZincAction<IEnumerable<T>> callback) where T : Object;


        /// <summary>
        /// 释放某一个资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public abstract void Release<T>(TKey key) where T : Object;
    }
}