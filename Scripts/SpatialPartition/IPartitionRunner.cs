using System;
using UnityEngine;

namespace ZincFramework.SpatialPartition
{
    public interface IPartitionRunner
    {
        void AddElement(IOverlapable overlapable);

        void RemoveElement(IOverlapable overlapable);

        void UpdateElement(IOverlapable overlapable);

        IOverlapable OverlapCircle(Vector2 center, float radius);

        IOverlapable OverlapBox(Vector2 center, Vector2 size);

        Span<IOverlapable> OverlapCircleAll(Vector2 center, float radius);

        Span<IOverlapable> OverlapBoxAll(Vector2 center, Vector2 size);
    }
}