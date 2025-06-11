using System;
using UnityEngine;
using ZincFramework.SpatialPartition;


namespace ZincFramework.Serialization
{
    public class PointOverlaper : EventOverlaper
    {
        public override Bounds Bounds => new Bounds(Center, Vector2.zero);

        public override bool Intersects(Bounds bounds)
        {
            return bounds.Contains(Center);
        }

        public override bool Intersects(CircleBounds circleBounds)
        {
            return circleBounds.Contains(Center);
        }

        public override bool IsInner(Bounds bounds)
        {
            return bounds.Contains(Center);
        }

        public override bool IsInner(CircleBounds bounds)
        {
            return bounds.Contains(Center);
        }

        public override IOverlapable Overlap(IPartitionRunner partition)
        {
            return null;
        }

        public override ReadOnlySpan<IOverlapable> OverlapAll(IPartitionRunner partition)
        {
            return new ReadOnlySpan<IOverlapable>();
        }
    }
}