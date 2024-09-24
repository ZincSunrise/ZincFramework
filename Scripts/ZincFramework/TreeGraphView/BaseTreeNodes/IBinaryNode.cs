namespace ZincFramework
{
    namespace TreeGraphView
    {
        public interface IBinaryNode<T> where T : BaseNode
        {
            T LeftChild { get; set; }

            T RightChild { get; set; }
        }
    }
}