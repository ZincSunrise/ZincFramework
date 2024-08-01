using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.TreeExtension.TreeNode;


namespace ZincFramework
{
    namespace TreeExtension
    {
        namespace TextTree
        {
            [CreateAssetMenu(fileName = "TextTree_", menuName = "GameTrees/CreateTextTree")]
            public class TextTree : ScriptableObject
            {
                public RootNode rootNode;

                [SerializeField]
                private List<BaseNode> _baseNodes = new List<BaseNode>();

                public T CreateNode<T>() where T : BaseNode 
                {
                    T node = ScriptableObject.CreateInstance<T>();
                    _baseNodes.Add(node);

                    return node;
                }

#if UNITY_EDITOR
                public void SetChild(BaseNode parent, BaseNode child)
                {

                }

                public void RemoveChild(BaseNode parent, BaseNode child)
                {

                }

                public List<BaseNode> GetChilren(BaseNode baseNode)
                {
                    return baseNode.GetChildren();
                }
#endif
            }
        }
    }
}