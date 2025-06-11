using System;
using UnityEngine;

namespace ZincFramework.SpatialPartition
{
    public class CircleOverlaper : EventOverlaper
    {
        public override Bounds Bounds => new Bounds(Center, new Vector3(_radius * 2, _radius * 2, 0));

        public CircleBounds CircleBounds 
        {
            get 
            {
                _circleBounds.Center = Center;
                return _circleBounds;
            } 
        }

        public float Radius { get => _radius; set => _radius = value; }

        private CircleBounds _circleBounds;

        protected override void Awake()
        {
            base.Awake();
            _circleBounds = new CircleBounds(Center, Radius);
        }

        [SerializeField]
        private float _radius;

        public override ReadOnlySpan<IOverlapable> OverlapAll(IPartitionRunner partitionRunner)
        {
            return partitionRunner.OverlapCircleAll(Center, _radius);
        }

        public override IOverlapable Overlap(IPartitionRunner partition)
        {
            return partition.OverlapCircle(Center, _radius);
        }

        public override bool IsInner(Bounds bounds) => bounds.Contains(CircleBounds);

        public override bool IsInner(CircleBounds bounds) => bounds.Contains(CircleBounds);

        public override bool Intersects(Bounds bounds) => CircleBounds.Intersects(bounds);

        public override bool Intersects(CircleBounds circleBounds) => CircleBounds.Intersects(circleBounds);

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.DrawWireSphere(Center, Radius);
        }
#endif
    }
}
