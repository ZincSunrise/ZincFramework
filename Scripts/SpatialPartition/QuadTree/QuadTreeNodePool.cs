using ZincFramework.Pools;

namespace ZincFramework.SpatialPartition.QuadTree
{
    public class QuadTreeNodePool : DataPool<QuadTreeNode>
    {
        public QuadTreeNodePool() : base(() => new QuadTreeNode())
        {

        }
    }
}