using UnityEngine;


namespace ZincFramework.SpatialPartition
{
    public interface IOverlapable
    {
        /// <summary>
        /// ��ǰ��λ��
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// λ����һ��������
        /// </summary>
        IPartition Container { get; set; }

        /// <summary>
        /// �Ƿ���ȫλ��ĳ�����α߽���
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        bool IsInner(Bounds bounds);

        /// <summary>
        /// �Ƿ���ȫλ��ĳ��Բ�α߽���
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        bool IsInner(CircleBounds bounds);

        /// <summary>
        /// �Ƿ���ĳ�����α߽��ཻ
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        bool Intersects(Bounds bounds);

        /// <summary>
        /// �Ƿ���ĳ��Բ�α߽��ཻ
        /// </summary>
        /// <param name="circleBounds"></param>
        /// <returns></returns>
        bool Intersects(CircleBounds circleBounds);

        /// <summary>
        /// ���ڴ���λ�õĸ���
        /// </summary>
        Vector3 UpdatePosition(out Vector3 prePosition);
    }
}
