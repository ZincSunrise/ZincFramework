using System;
using UnityEngine;


namespace ZincFramework.SpatialPartition
{
    public class BoxOverlaper : EventOverlaper
    {
        public override Bounds Bounds
        {
            get
            {
                _bounds.center = Center;
                return _bounds;
            }
        } 

        public Vector2 Size 
        { 
            get => _size; 
            set 
            {
                _bounds = new Bounds(Center, value);
                _size = value;
            }
        }

        [SerializeField]
        private Vector2 _size;

        private Bounds _bounds;

        protected override void Awake()
        {
            base.Awake();
            _bounds = new Bounds(Center, _size);
        }

        public override ReadOnlySpan<IOverlapable> OverlapAll(IPartitionRunner partition)
        {
            return partition.OverlapBoxAll(Center, _size);
        }

        public override IOverlapable Overlap(IPartitionRunner partition)
        {
            return partition.OverlapBox(Center, _size);
        }

        public override bool IsInner(Bounds bounds) => bounds.Contains(Bounds);

        public override bool IsInner(CircleBounds bounds) => bounds.Contains(Bounds);

        public override bool Intersects(Bounds bounds) => Bounds.Intersects(bounds);

        public override bool Intersects(CircleBounds circleBounds) => Bounds.Intersects(circleBounds);

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.DrawWireCube(transform.position, _size);
        }
#endif
    }
}