namespace ZincFramework.TreeService
{
    public interface IMutipleNode<T> where T : BaseNode
    {
        int ChildCount { get; }
#if UNITY_EDITOR
        void AddChild(T node);

        void RemoveChild(T node);
#endif
    }
}