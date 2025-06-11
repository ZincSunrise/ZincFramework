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
        /// ������ע���ƶ��¼��������ע�Ὣ�޷����з�Χ���
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
