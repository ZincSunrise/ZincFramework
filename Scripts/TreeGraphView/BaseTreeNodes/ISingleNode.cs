namespace ZincFramework.TreeService
{
    public interface ISingleNode<T> where T : BaseNode
    {
        T Child { get; }

        void SetChild(T child);

        void DiscardChild();
    }
}