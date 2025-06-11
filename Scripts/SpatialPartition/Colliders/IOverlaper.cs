using System;
using UnityEngine;

namespace ZincFramework.SpatialPartition
{
    public interface IOverlaper
    {
        Bounds Bounds { get; }

        /// <summary>
        /// 用于更新其他碰撞体的进入，存在，离开事件
        /// </summary>
        /// <param name="partition"></param>
        void UpdateOverlaper(IPartitionRunner partition);

        ReadOnlySpan<IOverlapable> OverlapAll(IPartitionRunner partition);

        IOverlapable Overlap(IPartitionRunner partition);

        void OverlapableEnter(IOverlapable overlapable);

        void OverlapableStay(IOverlapable overlapable);

        void OverlapableExit(IOverlapable overlapable);
    }
}