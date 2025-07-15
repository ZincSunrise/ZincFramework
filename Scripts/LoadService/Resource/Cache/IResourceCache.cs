using UnityEngine;

namespace ZincFramework.LoadServices.Resource
{
    internal interface IResourceCache
    {
        Object Asset { get; }
        
        bool IsCompleted { get; }

        bool IsDeleting { get; }
    }
}