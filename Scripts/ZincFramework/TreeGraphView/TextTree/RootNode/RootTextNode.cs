namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public class RootTextNode : SingleTextNode, IRootNode
            {
#if UNITY_EDITOR
                public override string InputHtmlColor => "#FFFFFF";

                public override string OutputHtmlColor => "#C9F9FF";
#endif
            }
        }
    }
}