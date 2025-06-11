using UnityEngine;


namespace ZincFramework.SpatialPartition
{
    public interface IOverlapable
    {
        /// <summary>
        /// 当前的位置
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// 位于哪一个区域内
        /// </summary>
        IPartition Container { get; set; }

        /// <summary>
        /// 是否完全位于某个矩形边界内
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        bool IsInner(Bounds bounds);

        /// <summary>
        /// 是否完全位于某个圆形边界内
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        bool IsInner(CircleBounds bounds);

        /// <summary>
        /// 是否与某个矩形边界相交
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        bool Intersects(Bounds bounds);

        /// <summary>
        /// 是否与某个圆形边界相交
        /// </summary>
        /// <param name="circleBounds"></param>
        /// <returns></returns>
        bool Intersects(CircleBounds circleBounds);

        /// <summary>
        /// 用于处理位置的更新
        /// </summary>
        Vector3 UpdatePosition(out Vector3 prePosition);
    }
}
