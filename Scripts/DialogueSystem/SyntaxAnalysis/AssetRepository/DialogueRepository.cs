using System;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Analysis;
using ZincFramework.LoadServices;
using ZincFramework.LoadServices.AssetBundles;
using UnityObject = UnityEngine.Object;


namespace ZincFramework.DialogueSystem.Analysis
{
    public class DialogueRepository : IAssetRepository<UnityObject>
    {
        private readonly static object _syncLock = new object();

        public static DialogueRepository GetInstance(Func<DialogueRepository> factory = null)
        {
            lock (_syncLock)
            {
                if (_instance == null)
                {
                    lock (_syncLock)
                    {
                        _instance = factory?.Invoke() ?? new DialogueRepository();
                    }
                }

                return _instance;
            }
        }


        protected static DialogueRepository _instance;


        private readonly Dictionary<int, DialoguePreserver> _assetUsers = new Dictionary<int, DialoguePreserver>();


        private readonly Dictionary<string, UnityObject> _assetMap = new Dictionary<string, UnityObject>();


        public IAssetLoader AssetLoader 
        {
            get => _assetLoader ??= new AssetBundleLoader();
            set => _assetLoader = value;
        }


        private IAssetLoader _assetLoader;

        private DialogueRepository()
        {

        }


        public void RegistUser(UnityObject assetUser)
        {
            int instanceId = assetUser.GetInstanceID();
            if (_assetUsers.ContainsKey(instanceId))
            {
                ThrowHelper.ThrowContainsArgumentException(instanceId);
            }

            _assetUsers.Add(instanceId, new DialoguePreserver(assetUser as GameObject));
        }

        public void UnRegistUser(UnityObject assetUser)
        {
            int instanceId = assetUser.GetInstanceID();
            _assetUsers.Remove(instanceId);
        }


        public T GetComponent<T>(UnityObject assetUser) where T : UnityObject
        {
            int instanceId = assetUser.GetInstanceID();
            if (_assetUsers.TryGetValue(instanceId, out var preserver))
            {
                return preserver.GetUserComponent<T>();
            }

            throw ThrowHelper.ThrowNonArgumentException(instanceId);
        }


        public T GetAsset<T>(string argument) where T : UnityObject
        {
            if(!_assetMap.TryGetValue(argument, out var asset))
            {
                asset = _assetLoader.LoadAsset<T>(argument);
                _assetMap.Add(argument, asset);
            }

            return asset as T;
        } 

        public void AddAsset<T>(string argument, T asset) where T : UnityObject
        {
            if (!_assetMap.TryAdd(argument, asset))
            {
                ThrowHelper.ThrowContainsArgumentException(argument);
            }
        }

        public void DeleteAsset<T>(string argument) where T : UnityObject
        {
            _assetMap.Remove(argument);
        }

        public bool UpdateAsset<T>(string argument, T asset) where T : UnityObject
        {
            return _assetMap[argument] = asset;
        }


        private static class ThrowHelper
        {
            public static ArgumentException ThrowNonArgumentException(object index) => throw new ArgumentException($"没有注册标号{index}");

            public static ArgumentException ThrowContainsArgumentException(object index) => throw new ArgumentException($"请不要添加同一标号{index}");
        }
    }
}