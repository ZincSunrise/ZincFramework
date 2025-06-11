using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZincFramework.SpatialPartition
{
    public abstract class Overlaper : MonoBehaviour, IOverlaper, IOverlapable
    {
        public bool IsSleeping { get; private set; }

        public abstract Bounds Bounds { get; }

        /// <summary>
        /// Åö×²Æ÷µÄÖÐÐÄ
        /// </summary>
        public Vector2 Center => _offset + (Vector2)_myTransform.position;

        public Vector3 Position => _myTransform.position;

        public IPartition Container { get; set; }

        [SerializeField]
        protected Vector2 _offset;

        protected Transform _myTransform;


        protected HashSet<IOverlapable> _nowOverlapableSet;

        protected HashSet<IOverlapable> _preOverlapableSet;


        protected virtual void Awake()
        {
            _nowOverlapableSet = new HashSet<IOverlapable>();
            _preOverlapableSet = new HashSet<IOverlapable>();
            _myTransform = transform;
        }

        public void Wakeup()
        {
            IsSleeping = false;
        }

        public void Sleep()
        {
            IsSleeping = true;
            _nowOverlapableSet.Clear();
            _preOverlapableSet.Clear();
        }


        public virtual void UpdateOverlaper(IPartitionRunner partition)
        {
            if (IsSleeping)
            {
                return;
            }

            var overlapables = OverlapAll(partition);

            _nowOverlapableSet.Clear();

            for (int i = 0; i < overlapables.Length; i++)
            {
                var overlapable = overlapables[i];
                _nowOverlapableSet.Add(overlapable);

                if (_preOverlapableSet.Contains(overlapable))
                {
                    OverlapableStay(overlapable);
                }
                else
                {
                    OverlapableEnter(overlapable);
                }
            }

            _preOverlapableSet.RemoveWhere(RemoveExit);
            (_nowOverlapableSet, _preOverlapableSet) = (_preOverlapableSet, _nowOverlapableSet);
        }

        private bool RemoveExit(IOverlapable overlapable)
        {
            if (!_nowOverlapableSet.Contains(overlapable))
            {
                OverlapableExit(overlapable);
                return true;
            }

            return false;
        }

        public abstract IOverlapable Overlap(IPartitionRunner partition);

        public abstract ReadOnlySpan<IOverlapable> OverlapAll(IPartitionRunner partition);

        public abstract Vector3 UpdatePosition(out Vector3 prePosition);

        public abstract bool IsInner(Bounds bounds);

        public abstract bool IsInner(CircleBounds bounds);

        public abstract bool Intersects(Bounds bounds);

        public abstract bool Intersects(CircleBounds circleBounds);

        public abstract void OverlapableEnter(IOverlapable overlapable);

        public abstract void OverlapableStay(IOverlapable overlapable);

        public abstract void OverlapableExit(IOverlapable overlapable);

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            if(_myTransform == null)
            {
                _myTransform = transform;
            }
            Gizmos.color = Color.green;
        }
#endif
    }
}