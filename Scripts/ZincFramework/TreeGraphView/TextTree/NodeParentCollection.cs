using System.Collections.Generic;


namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public class NodeParentCollection
            {
                public BaseTextNode Child { get; private set; }

                public HashSet<BaseTextNode> ParentNodes { get; private set; } = new HashSet<BaseTextNode>();

                public bool IsSingleChild 
                {
                    get
                    {
                        foreach(var node in ParentNodes)
                        {
                            if(node is IMutipleNode<BaseTextNode> mutiNode && mutiNode.ChildCount != 1)
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                }


                public NodeParentCollection(BaseTextNode baseTextNode) 
                {
                    Child = baseTextNode;
                }
            }
        }
    }
}