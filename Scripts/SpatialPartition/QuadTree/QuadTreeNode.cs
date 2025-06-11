using System;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.DataPools;

namespace ZincFramework.SpatialPartition.QuadTree
{
    public class QuadTreeNode : IPartition, IReuseable
    {
        public bool IsLeafNode => _children == null;


        private Bounds _bounds;

        public List<IOverlapable> Elements { get; private set; } = new List<IOverlapable>();

        /// <summary>
        /// 子节点们
        /// </summary>
        private QuadTreeNode[] _children;


        private QuadTreeSettings _quadTreeSettings;


        private int _depth;

        public QuadTreeNode()
        {

        }

        public QuadTreeNode(QuadTreeSettings settings, int depth, Bounds bounds)
        {
            Initialize(settings, depth, bounds);
        }

        #region 初始化
        public void Initialize(QuadTreeSettings settings, int depth, Bounds bounds)
        {
            _quadTreeSettings = settings;
            _depth = depth;
            _bounds = bounds;
        }
        #endregion

        #region 元素操作相关
        public void AddElement(IOverlapable overlapable)
        {
            //如果当前元素的容量没有到达最大值或者已经是最大深度，那么直接添加
            if(Elements.Count < _quadTreeSettings.MaxCapacity || _depth > _quadTreeSettings.MaxDepth)
            {
                AddElementInternal(overlapable);
            }
            else
            {
                SelectAddChildren(overlapable, IsLeafNode);
            }
        }

        public bool RemoveElement(IOverlapable overlapable)
        {
            // 尝试在当前节点直接删除
            if (Elements.Remove(overlapable))
            {
                overlapable.Container = null;
                return true;
            }
            else if (!IsLeafNode)
            {
                // 如果当前节点有子节点，递归到正确的子节点并删除元素
                int intersectionCount = GetChildrenIntersectionsCount(overlapable, out var quadTreeNode);

                if (intersectionCount > 1)
                {
                    return false; // 如果元素跨越多个子节点，不能删除
                }
                else if (quadTreeNode.RemoveElement(overlapable))
                {
                    // 如果所有子节点都是叶子节点，并且元素数量小于最大容量，进行合并
                    if (CheckMerge())
                    {
                        MergeElement();
                    }

                    return true;
                }
            }

            // 无法删除的情况
            return false; 
        }

        public bool RepositionElement(IOverlapable overlapable, Vector3 offset)
        {
            // 尝试在当前节点直接删除
            if (Elements.Remove(overlapable))
            {
                overlapable.Container = null;
                return true;
            }
            else if (!IsLeafNode)
            {
                // 如果当前节点有子节点，递归到正确的子节点并删除元素
                int intersectionCount = GetChildrenIntersectionsCount(overlapable, offset, out var quadTreeNode);

                if (intersectionCount != 1)
                {
                    return false; // 如果元素跨越多个子节点，不能删除
                }
                if (quadTreeNode.RepositionElement(overlapable, offset))
                {
                    // 如果所有子节点都是叶子节点，并且元素数量小于最大容量，进行合并
                    if (CheckMerge())
                    {
                        MergeElement();
                    }

                    return true;
                }
            }

            // 无法删除的情况
            return false;
        }

        public bool CanUpdate(IOverlapable overlapable) => !overlapable.IsInner(_bounds);

        public void ClearElement()
        {
            for(int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Container = null;
            }

            Elements.Clear();
            _quadTreeSettings = null;

            if (!IsLeafNode)
            {
                for (int i = 0; i < _children.Length; i++)
                {
                    _children[i].ClearElement();
                }

                QuadTreeServices.ReturnNodeArray(_children);
                _children = null;
            }

            QuadTreeServices.ReturnNode(this);
        }

        /// <summary>
        /// 检查是否需要合并子节点
        /// </summary>
        /// <returns></returns>
        private bool CheckMerge()
        {
            // 删除后尝试合并子节点的元素
            bool isAllLeafChild = true;
            int childrenCount = 0;

            for (int i = 0; i < _children.Length; i++)
            {
                isAllLeafChild &= _children[i].IsLeafNode;
                childrenCount += _children[i].Elements.Count;
            }

            return isAllLeafChild && childrenCount < _quadTreeSettings.MergeThreshold;
        }

        /// <summary>
        /// 合并子节点
        /// </summary>
        private void MergeElement()
        {
            // 合并子节点的元素到父节点中
            for (int i = 0; i < _children.Length; i++)
            {
                var childNode = _children[i];
                for(int j = 0; j < childNode.Elements.Count; j++)
                {
                    AddElementInternal(childNode.Elements[j]);
                }

                //返还节点到对象池
                QuadTreeServices.ReturnNode(childNode);
            }


            // 返还数组到数组池
            QuadTreeServices.ReturnNodeArray(_children);
            _children = null;
        }

        /// <summary>
        /// 分裂子节点
        /// </summary>
        private void SplitElment()
        {
            int nextDepth = _depth + 1;
            float quadX = _bounds.size.x / 4;
            float quadY = _bounds.size.y / 4;
            Vector2 halfSize = _bounds.size / 2;

            _children = QuadTreeServices.RentNodeArray();
            Bounds topLeft = new Bounds(_bounds.center + new Vector3(-quadX, quadY), halfSize);
            var topLeftNode = QuadTreeServices.RentNode(_quadTreeSettings, nextDepth, topLeft);
            _children[0] = topLeftNode;

            Bounds topRight = new Bounds(_bounds.center + new Vector3(quadX, quadY), halfSize);
            var topRightNode = QuadTreeServices.RentNode(_quadTreeSettings, nextDepth, topRight);
            _children[1] = topRightNode;

            Bounds bottomLeft = new Bounds(_bounds.center + new Vector3(-quadX, -quadY), halfSize);
            var bottomLeftNode = QuadTreeServices.RentNode(_quadTreeSettings, nextDepth, bottomLeft);
            _children[2] = bottomLeftNode;

            Bounds bottomRight = new Bounds(_bounds.center + new Vector3(quadX, -quadY), halfSize);
            var bottomRightNode = QuadTreeServices.RentNode(_quadTreeSettings, nextDepth, bottomRight);
            _children[3] = bottomRightNode;
        }

        /// <summary>
        /// 获取与该对象相交的子节点数量
        /// </summary>
        /// <param name="overlapable"></param>
        /// <param name="quadTreeNode">第一个相交的节点</param>
        /// <returns></returns>
        private int GetChildrenIntersectionsCount(IOverlapable overlapable, out QuadTreeNode quadTreeNode)
        {
            quadTreeNode = null;
            int intersectionCount = 0;
            for (int i = 0; i < _children.Length; i++)
            {
                if (_children[i].Intersects(overlapable))
                {
                    intersectionCount++;   
                    quadTreeNode = _children[i];
                }
            }

            return intersectionCount;
        }

        private int GetChildrenIntersectionsCount(IOverlapable overlapable, Vector3 offset, out QuadTreeNode quadTreeNode)
        {
            quadTreeNode = null;
            int intersectionCount = 0;
            for (int i = 0; i < _children.Length; i++)
            {
                if (_children[i].Intersects(overlapable, offset))
                {
                    intersectionCount++;
                    quadTreeNode = _children[i];
                }
            }

            return intersectionCount;
        }

        #endregion

        #region 元素查询相关
        public bool Intersects(IOverlapable overlapable) => overlapable.Intersects(_bounds);

        public bool Intersects(IOverlapable overlapable, Vector3 offset)
        {
            Bounds bounds = new Bounds(_bounds.center + offset, _bounds.size);
            return overlapable.Intersects(bounds);
        }

        public IOverlapable QueryElement(Bounds bounds)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i].Intersects(bounds))
                {
                    return Elements[i];
                }
            }

            if (!IsLeafNode)
            {
                for (int i = 0; i < _children.Length; i++)
                {
                    if (_children[i]._bounds.Intersects(bounds))
                    {
                        return _children[i].QueryElement(bounds);
                    }
                }
            }

            return null;
        }

        public IOverlapable QueryElement(CircleBounds circleBounds)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i].Intersects(circleBounds))
                {
                    return Elements[i];
                }
            }

            if (!IsLeafNode)
            {
                for (int i = 0; i < _children.Length; i++)
                {
                    if (_children[i]._bounds.Intersects(circleBounds))
                    {
                        return _children[i].QueryElement(circleBounds);
                    }
                }
            }

            return null;
        }

        public void QueryElements(Bounds bounds, ref IOverlapable[] overlapables, ref int count)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i].Intersects(bounds))
                {
                    if (count + 1 > overlapables.Length && !ResizeArray(ref overlapables))
                    {
                        break;
                    }

                    overlapables[count++] = Elements[i];
                }
            }

            if (!IsLeafNode)
            {
                for(int i = 0; i < _children.Length; i++)
                {
                    if (_children[i]._bounds.Intersects(bounds))
                    {
                        _children[i].QueryElements(bounds, ref overlapables, ref count);
                    }
                }
            }
        }

        public void QueryElements(CircleBounds circleBounds, ref IOverlapable[] overlapables, ref int count)    
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i].Intersects(circleBounds))
                {
                    if (count + 1 > overlapables.Length && !ResizeArray(ref overlapables))
                    {
                        break;
                    }

                    overlapables[count++] = Elements[i];
                }
            }

            if (!IsLeafNode)
            {
                for (int i = 0; i < _children.Length; i++)
                {
                    if (_children[i]._bounds.Intersects(circleBounds))
                    {
                        _children[i].QueryElements(circleBounds, ref overlapables, ref count);
                    }
                }
            }
        }


        private bool ResizeArray(ref IOverlapable[] overlapables)
        {
            if (overlapables.Length >= _quadTreeSettings.MaxQueryCount)
            {
                return false;
            }
            else if (overlapables.Length < _quadTreeSettings.MaxQueryCount)
            {
                if (overlapables.Length * 2 < _quadTreeSettings.MaxQueryCount)
                {
                    Array.Resize(ref overlapables, overlapables.Length * 2);
                }
                else
                {
                    Array.Resize(ref overlapables, _quadTreeSettings.MaxQueryCount);
                }
            }

            return true;
        }

        public IOverlapable QueryElement(IOverlaper overlaper) => overlaper switch
        {
            BoxOverlaper boxOverlaper => QueryElement(boxOverlaper.Bounds),
            CircleOverlaper circleOverlaper => QueryElement(circleOverlaper.CircleBounds),
            _ => QueryElement(overlaper.Bounds),
        };

        public void QueryElements(IOverlaper overlaper, ref IOverlapable[] overlapables, ref int count)
        {
            switch (overlaper)
            {
                case BoxOverlaper boxOverlaper:
                    QueryElements(boxOverlaper.Bounds, ref overlapables, ref count);
                    break;
                case CircleOverlaper circleOverlaper:
                    QueryElements(circleOverlaper.CircleBounds, ref overlapables, ref count);
                    break;
                default:
                    QueryElements(overlaper.Bounds, ref overlapables, ref count);
                    break;
            }
        }

        #endregion
        private void SelectAddChildren(IOverlapable overlapable, bool isLeafNode)
        {
            if (isLeafNode)
            {
                SplitElment();
            }

            int intersectionCount = GetChildrenIntersectionsCount(overlapable, out var quadTreeNode);
            if (intersectionCount > 1)
            {
                AddElementInternal(overlapable);

                if (isLeafNode)
                {
                    MergeElement();
                }
                else
                {
                    Elements.RemoveAll(CheckSpilt);
                }
            }
            else if(quadTreeNode != null)
            {
                quadTreeNode.AddElement(overlapable);
            }
            else
            {
                Debug.LogWarning("你添加了一个出界的元素，因此添加失败");
            }
        }


        private bool CheckSpilt(IOverlapable overlapable)
        {
            int count = GetChildrenIntersectionsCount(overlapable, out var childNode);
            if(count == 1)
            {
                childNode.AddElement(overlapable);
                return true;
            }

            return false;
        }

        private void AddElementInternal(IOverlapable overlapable)
        {
            Elements.Add(overlapable);
            overlapable.Container = this;
        }

        public void OnReturn()
        {
            if (Elements.Count > 0) 
            {
                Elements.Clear();
                _quadTreeSettings = null;
            }
        }

        public void OnRent()
        {

        }

#if UNITY_EDITOR
        public void DrawGizmos()
        {
            if (Elements.Count == 0)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.yellow;
            }

            Gizmos.DrawWireCube(_bounds.center, _bounds.size);

            if (!IsLeafNode)
            {
                for (int i = 0; i < _children.Length; ++i)
                {
                    _children[i].DrawGizmos();
                }
            }
        }
#endif
    }
}