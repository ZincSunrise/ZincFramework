using ZincFramework.DataPools;

namespace ZincFramework.SpatialPartition.QuadTree
{
    public class QuadTreeNodePool : DataPool<QuadTreeNode>
    {
        public QuadTreeNodePool() : base(() => new QuadTreeNode())
        {

        }
    }
}