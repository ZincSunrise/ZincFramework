using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;


namespace ZincFramework.LoadServices.Resource
{
    public class ResourceCache
    {
        public Object Asset { get; private set; }

        public bool IsCompleted => ResourceRequest?.isDone ?? true;


        public event UnityAction<Object> Completed;

        public ResourceRequest ResourceRequest { get; private set; }


        public bool IsDeleting { get; private set; }


        public ResourceCache(Object asset)
        {
            Asset = asset;
        }

        public ResourceCache(ResourceRequest resourceRequest)
        {
            ResourceRequest = resourceRequest;
        }

        public ResourceCache(ResourceRequest resourceRequest, UnityAction<Object> callback)
        {
            ResourceRequest = resourceRequest;
            Completed = callback;

            ResourceRequest.completed += OnCompleted;
        }

        private void OnCompleted(AsyncOperation asyncOperation)
        {
            if (!IsDeleting)
            {
                Asset = ResourceRequest.asset;
                Completed?.Invoke(ResourceRequest.asset);
            }
        }

        public void StopLoad()
        {
            ResourceRequest.completed -= OnCompleted;
            Completed = null;
            IsDeleting = true;
        }
    }
}

