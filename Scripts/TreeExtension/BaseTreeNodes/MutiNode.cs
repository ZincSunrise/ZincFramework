using System.Collections.Generic;
using UnityEngine;



namespace ZincFramework
{
    namespace TreeExtension
    {
        namespace TreeNode
        {
            public abstract class MutiNode : BaseNode
            {
                public List<BaseNode> Children => _children;

                [SerializeField]
                private List<BaseNode> _children = new List<BaseNode>();


                public override void RemoveChild(BaseNode baseNode)
                {
                    _children.Remove(baseNode);
                }

                public override void SetChild(BaseNode baseNode)
                {
                    _children.Add(baseNode);
                }
            }
        }
    }
}