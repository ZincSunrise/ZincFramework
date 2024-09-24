namespace ZincFramework
{
    namespace TreeGraphView
    {
        public interface IMutipleNode<T> where T : BaseNode
        {
            int ChildCount { get; }

            T[] GetChildren();

#if UNITY_EDITOR
            void AddChild(T node);

            void RemoveChild(T node);         
#endif
        }
    }
}