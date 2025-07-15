using UnityEngine;

namespace ZincFramework.LoadServices.Resource
{
    internal class ResourceSyncCache : IResourceCache
    {
        public Object Asset { get; private set; }

        public bool IsCompleted => true;

        public bool IsDeleting => false;

        public ResourceSyncCache(Object asset) 
        {
            Asset = asset;
        }
    }
}