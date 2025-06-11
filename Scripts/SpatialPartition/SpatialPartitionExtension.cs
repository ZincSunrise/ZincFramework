using UnityEngine;

namespace ZincFramework.SpatialPartition
{
    public static class SpatialPartitionExtension
    {
        public static bool Intersects(this Bounds bounds, CircleBounds circle)
        {
            var circleCenter = circle.Center;
            Vector2 max = bounds.max;
            Vector2 min = bounds.min;

            Vector3 nearestPoint = new Vector2(
                Mathf.Clamp(circleCenter.x, min.x, max.x),
                Mathf.Clamp(circleCenter.y, min.y, max.y)
            );

            // ����Բ���������ľ���
            float distance = Vector3.SqrMagnitude(circleCenter - nearestPoint);

            // �������С�ڵ���Բ�İ뾶��˵���ཻ
            return distance <= circle.RadiusSquared;
        }

        public static bool Contains(this Bounds bounds1, in Bounds bounds2)
        {
            Vector3 minDiff = bounds2.min - bounds1.min;
            Vector3 maxDiff = bounds1.max - bounds2.max;

            return minDiff.x >= 0 && maxDiff.x >= 0 &&
                   minDiff.y >= 0 && maxDiff.y >= 0 &&
                   minDiff.z >= 0 && maxDiff.z >= 0;
        }

        public static bool Contains(this Bounds bounds1, in CircleBounds bounds2)
        {
            Vector3 minDiff = bounds2.Center - bounds1.min;
            Vector3 maxDiff = bounds1.max - bounds2.Center;
            float radius = bounds2.Radius;

            return minDiff.x >= radius && maxDiff.x >= radius &&
                   minDiff.y >= radius && maxDiff.y >= radius;
        }
    }
}