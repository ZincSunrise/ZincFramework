using System;
using UnityEngine;

namespace ZincFramework.SpatialPartition
{
    public abstract class EventOverlaper : Overlaper
    {
        public event Action<IOverlapable> OnOverlapableEnter;

        public event Action<IOverlapable> OnOverlapableStay;

        public event Action<IOverlapable> OnOverlapableExit;

        /// <summary>
        /// 在这里注册移动事件，如果不注册将无法进行范围检测
        /// </summary>
        public event Action OnPositionUpdate;

        public override void OverlapableEnter(IOverlapable overlapable)
        {
            OnOverlapableEnter?.Invoke(overlapable);
        }

        public override void OverlapableStay(IOverlapable overlapable)
        {
            OnOverlapableStay?.Invoke(overlapable);
        }

        public override void OverlapableExit(IOverlapable overlapable)
        {
            OnOverlapableExit?.Invoke(overlapable);
        }

        public override Vector3 UpdatePosition(out Vector3 prePosition)
        {
            prePosition = Position;
            OnPositionUpdate?.Invoke();
            return Position;
        }

        protected virtual void OnEnable()
        {
            OverlapServices.RegistOverlaper(this);
        }

        protected virtual void OnDisable()
        {
            OverlapServices.UnRegistOverlaper(this);
        }
    }
}
