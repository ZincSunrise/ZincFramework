using System.Collections.Generic;

namespace ZincFramework
{
    namespace TreeExtension
    {
        namespace TreeNode
        {
            public abstract class SingleNode : BaseNode
            {
                public BaseNode childNode;

                private List<BaseNode> _childrenList;

                public override List<BaseNode> GetChildren()
                {
                    _childrenList ??= new List<BaseNode>() { childNode };
                    return _childrenList;
                }

                public override void RemoveChild(BaseNode baseNode)
                {
                    if (baseNode == childNode)
                    {
                        childNode = null;
                        _childrenList?.Remove(childNode);
                    }
                }

                public override void SetChild(BaseNode baseNode)
                {
                    if (baseNode != childNode)
                    {
                        _childrenList?.Remove(childNode);
                    }
                    childNode = baseNode;
                }
            }
        }
    }
}