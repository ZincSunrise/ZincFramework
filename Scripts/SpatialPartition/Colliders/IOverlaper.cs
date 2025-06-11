using System;
using UnityEngine;

namespace ZincFramework.SpatialPartition
{
    public interface IOverlaper
    {
        Bounds Bounds { get; }

        /// <summary>
        /// ���ڸ���������ײ��Ľ��룬���ڣ��뿪�¼�
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