using UnityEngine;

namespace ZincFramework.SpatialPartition
{
    public struct CircleBounds
    {
        public Vector3 Center { get; set; }

        public float Radius
        {
            readonly get => _radius;
            set 
            {
                _radius = value;
                RadiusSquared = _radius * _radius;
            }
        }

        public float RadiusSquared { get; private set; }

        private float _radius;

        public CircleBounds(Vector3 center, float radius)
        {
            Center = center;
            _radius = radius;
            RadiusSquared = radius * radius;
        }

        public bool Contains(Vector3 point)
        {
            // 使用平方值进行比较，避免计算平方根
            float distanceSquared = (Center - point).sqrMagnitude;
            return distanceSquared <= RadiusSquared;
        }

        public bool Intersects(CircleBounds other)
        {
            float magnitude = Vector3.SqrMagnitude(Center - other.Center);
            return magnitude <= RadiusSquared + other.RadiusSquared;
        }

        public bool Intersects(Bounds bounds)
        {
            Vector2 max = bounds.max;
            Vector2 min = bounds.min;

            Vector3 nearestPoint = new Vector2(
                Mathf.Clamp(Center.x, min.x, max.x),
                Mathf.Clamp(Center.y, min.y, max.y)
            );

            // 计算圆心与最近点的距离
            float magnitude = Vector3.SqrMagnitude(Center - nearestPoint);

            // 如果距离小于等于圆的半径，说明相交
            return magnitude <= RadiusSquared;
        }

        public bool Contains(Bounds bounds)
        {
            Vector3 minDiff = Center - bounds.min;
            Vector3 maxDiff = bounds.max - Center;
            float radius = Radius;

            return minDiff.x >= radius && maxDiff.x >= radius &&
                   minDiff.y >= radius && maxDiff.y >= radius;
        }

        public bool Contains(CircleBounds circleBounds)
        {
            float magnitude = Vector3.SqrMagnitude(Center - circleBounds.Center);
            return magnitude <= RadiusSquared - circleBounds.RadiusSquared;
        }
    }
}