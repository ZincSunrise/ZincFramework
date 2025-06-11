using UnityEngine;

namespace ZincFramework.SpatialPartition.QuadTree
{
    public class QuadTreeSettings
    {
        public int MaxDepth { get; init; }

        public int MaxCapacity { get; init; }

        public Bounds InitialBounds { get; init; }

        public int MaxQueryCount { get; init; } = 100;

        /// <summary>
        /// �ϲ�����ֵ
        /// </summary>
        public int MergeThreshold { get; init; } = 60;
    }
}
