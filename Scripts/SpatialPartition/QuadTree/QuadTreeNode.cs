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
        /// �ӽڵ���
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

        #region ��ʼ��
        public void Initialize(QuadTreeSettings settings, int depth, Bounds bounds)
        {
            _quadTreeSettings = settings;
            _depth = depth;
            _bounds = bounds;
        }
        #endregion

        #region Ԫ�ز������
        public void AddElement(IOverlapable overlapable)
        {
            //�����ǰԪ�ص�����û�е������ֵ�����Ѿ��������ȣ���ôֱ�����
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
            // �����ڵ�ǰ�ڵ�ֱ��ɾ��
            if (Elements.Remove(overlapable))
            {
                overlapable.Container = null;
                return true;
            }
            else if (!IsLeafNode)
            {
                // �����ǰ�ڵ����ӽڵ㣬�ݹ鵽��ȷ���ӽڵ㲢ɾ��Ԫ��
                int intersectionCount = GetChildrenIntersectionsCount(overlapable, out var quadTreeNode);

                if (intersectionCount > 1)
                {
                    return false; // ���Ԫ�ؿ�Խ����ӽڵ㣬����ɾ��
                }
                else if (quadTreeNode.RemoveElement(overlapable))
                {
                    // ��������ӽڵ㶼��Ҷ�ӽڵ㣬����Ԫ������С��������������кϲ�
                    if (CheckMerge())
                    {
                        MergeElement();
                    }

                    return true;
                }
            }

            // �޷�ɾ�������
            return false; 
        }

        public bool RepositionElement(IOverlapable overlapable, Vector3 offset)
        {
            // �����ڵ�ǰ�ڵ�ֱ��ɾ��
            if (Elements.Remove(overlapable))
            {
                overlapable.Container = null;
                return true;
            }
            else if (!IsLeafNode)
            {
                // �����ǰ�ڵ����ӽڵ㣬�ݹ鵽��ȷ���ӽڵ㲢ɾ��Ԫ��
                int intersectionCount = GetChildrenIntersectionsCount(overlapable, offset, out var quadTreeNode);

                if (intersectionCount != 1)
                {
                    return false; // ���Ԫ�ؿ�Խ����ӽڵ㣬����ɾ��
                }
                if (quadTreeNode.RepositionElement(overlapable, offset))
                {
                    // ��������ӽڵ㶼��Ҷ�ӽڵ㣬����Ԫ������С��������������кϲ�
                    if (CheckMerge())
                    {
                        MergeElement();
                    }

                    return true;
                }
            }

            // �޷�ɾ�������
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
        /// ����Ƿ���Ҫ�ϲ��ӽڵ�
        /// </summary>
        /// <returns></returns>
        private bool CheckMerge()
        {
            // ɾ�����Ժϲ��ӽڵ��Ԫ��
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
        /// �ϲ��ӽڵ�
        /// </summary>
        private void MergeElement()
        {
            // �ϲ��ӽڵ��Ԫ�ص����ڵ���
            for (int i = 0; i < _children.Length; i++)
            {
                var childNode = _children[i];
                for(int j = 0; j < childNode.Elements.Count; j++)
                {
                    AddElementInternal(childNode.Elements[j]);
                }

                //�����ڵ㵽�����
                QuadTreeServices.ReturnNode(childNode);
            }


            // �������鵽�����
            QuadTreeServices.ReturnNodeArray(_children);
            _children = null;
        }

        /// <summary>
        /// �����ӽڵ�
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
        /// ��ȡ��ö����ཻ���ӽڵ�����
        /// </summary>
        /// <param name="overlapable"></param>
        /// <param name="quadTreeNode">��һ���ཻ�Ľڵ�</param>
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

        #region Ԫ�ز�ѯ���
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
                Debug.LogWarning("�������һ�������Ԫ�أ�������ʧ��");
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