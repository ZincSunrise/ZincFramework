using UnityEngine;


namespace ZincFramework.LoadServices.Resource
{
    public static class ResourcesAwaiterExtension 
    {
        public static ResourceRequestAwaiter GetAwaiter(this ResourceRequest resourceRequest)
        {
            return new ResourceRequestAwaiter(resourceRequest);       
        }
    }
}