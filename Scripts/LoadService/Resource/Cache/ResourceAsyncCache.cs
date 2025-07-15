using UnityEngine;
using ZincFramework.Events;


namespace ZincFramework.LoadServices.Resource
{
    internal class ResourceAsyncCache<T> : IResourceCache where T : Object
    {
        public Object Asset => ResourceRequest.asset;

        public bool IsCompleted => ResourceRequest.isDone;


        public event ZincAction<T> Completed;

        public ResourceRequest ResourceRequest { get; private set; }

        public bool IsDeleting { get; private set; }


        public ResourceAsyncCache(ResourceRequest resourceRequest)
        {
            ResourceRequest = resourceRequest;
        }

        public ResourceAsyncCache(ResourceRequest resourceRequest, ZincAction<T> callback)
        {
            ResourceRequest = resourceRequest;
            Completed = callback;

            ResourceRequest.completed += OnCompleted;
        }

        private void OnCompleted(AsyncOperation asyncOperation)
        {
            if (!IsDeleting)
            {
                Completed?.Invoke(ResourceRequest.asset as T);
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

