using System;
using UnityEngine;


namespace ZincFramework.SpatialPartition.QuadTree
{
    public class QuadTreeRunner : IPartitionRunner
    {
        public QuadTreeNode RootNode { get; private set; }


        private IOverlapable[] _queryElements = new IOverlapable[16];


        private QuadTreeSettings _quadTreeSettings;

        public void Initialize(Bounds bounds) => Initialize(new QuadTreeSettings()
        {
            InitialBounds = bounds,
            MaxCapacity = 8,
            MaxDepth = 5,
            MaxQueryCount = 100,
        });

        public void Initialize(QuadTreeSettings quadTreeSettings)
        {
            _quadTreeSettings = quadTreeSettings;
            RootNode = new QuadTreeNode(quadTreeSettings, 0, quadTreeSettings.InitialBounds);
        }

        public void AddElement(IOverlapable overlapable)
        {
            RootNode.AddElement(overlapable);
        }

        public void RemoveElement(IOverlapable overlapable) 
        {
            if (!RootNode.RemoveElement(overlapable))
            {
                Debug.LogError("移除失败");
            }
        }

        public void UpdateElement(IOverlapable overlapable)
        {
            Vector3 nowPosition = overlapable.UpdatePosition(out var prePosition);

            // 检查是否需要删除并重新添加元素
            if (overlapable.Container != null && overlapable.Container.CanUpdate(overlapable))
            {
                // 删除节点并重新定位元素
                if(!RootNode.RepositionElement(overlapable, nowPosition - prePosition))
                {
                    overlapable.Container.RemoveElement(overlapable);
                }
            }

            // 如果元素在四叉树范围内，重新添加节点
            if (overlapable.Container == null && RootNode.Intersects(overlapable))
            {
                RootNode.AddElement(overlapable);
            }
        }

        public void ClearElements()
        {
            RootNode.ClearElement();
            RootNode = QuadTreeServices.RentNode(_quadTreeSettings, 0, _quadTreeSettings.InitialBounds);
        }

        public Span<IOverlapable> OverlapBoxAll(Vector2 center, Vector2 size)
        {
            int count = 0;
            RootNode.QueryElements(new Bounds(center, size), ref _queryElements, ref count);
            return new Span<IOverlapable>(_queryElements, 0, count);
        }

        public Span<IOverlapable> OverlapCircleAll(Vector2 center, float radius)
        {
            int count = 0;
            RootNode.QueryElements(new CircleBounds(center, radius), ref _queryElements, ref count);
            return new Span<IOverlapable>(_queryElements, 0, count);
        }

        public IOverlapable OverlapBox(Vector2 center, Vector2 size)
        {
            return RootNode.QueryElement(new Bounds(center, size));
        }

        public IOverlapable OverlapCircle(Vector2 center, float radius)
        {
            return RootNode.QueryElement(new CircleBounds(center, radius));
        }
    }
}
