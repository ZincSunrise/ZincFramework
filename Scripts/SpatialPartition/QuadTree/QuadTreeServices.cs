using System;
using UnityEngine;
using ZincFramework.DataPools;

namespace ZincFramework.SpatialPartition.QuadTree
{
    public static class QuadTreeServices
    {
        private static readonly QuadTreeNodePool _quadNodePool = new QuadTreeNodePool();

        private static readonly ObjectPool<QuadTreeNode[]> _arrayPool = new ObjectPool<QuadTreeNode[]>(() => new QuadTreeNode[4]);

        public static QuadTreeNode RentNode(QuadTreeSettings quadTreeSettings, int depth, Bounds bounds)
        {
            QuadTreeNode quadTreeNode = _quadNodePool.RentValue();
            quadTreeNode.Initialize(quadTreeSettings, depth, bounds);
            return quadTreeNode;

        }

        public static void ReturnNode(QuadTreeNode quadTreeNode) 
        {
            _quadNodePool.ReturnValue(quadTreeNode);
        }

        public static QuadTreeNode[] RentNodeArray()
        {
            return _arrayPool.RentValue();
        }

        public static void ReturnNodeArray(QuadTreeNode[] quadTreeNodes)
        {
            Array.Clear(quadTreeNodes, 0, quadTreeNodes.Length);
            _arrayPool.ReturnValue(quadTreeNodes);
        }
    }
}