using System;
using UnityEngine;
using System.Collections.Generic;
using ZincFramework.TreeService;
using ZincFramework.DialogueSystem.TextData;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ZincFramework.DialogueSystem
{
    [CreateAssetMenu(menuName = "GameTool/CreateTextTree", order = 4)]
    public class TextTree : ScriptableObject
    {
        public RootTextNode RootTextNode => _rootNode;

        public List<BaseTextNode> Nodes => _nodes;

        [SerializeField]
        private List<BaseTextNode> _nodes = new List<BaseTextNode>();

        [SerializeField]
        private RootTextNode _rootNode;

        public TextTree CloneTree()
        {
            TextTree textTree = ScriptableObject.Instantiate(this);
            textTree._rootNode = _rootNode.CloneNode() as RootTextNode;
            return textTree;
        }

        public void DestoryTree()
        {
            _rootNode.DestroyNode();
            Destroy(this);
        }

#if UNITY_EDITOR
        public BaseTextNode CreateNodeInInfo(Type type)
        {
            BaseTextNode baseTextNode = ScriptableObject.CreateInstance(type) as BaseTextNode;
            baseTextNode.guid = GUID.Generate().ToString();
            baseTextNode.name = type.Name;

            _nodes.Add(baseTextNode);

            if (baseTextNode is RootTextNode rootNode)
            {
                _rootNode = rootNode;
            }

            AssetDatabase.AddObjectToAsset(baseTextNode, this);
            return baseTextNode;
        }

        public BaseTextNode CreateNode(Type type)
        {
            BaseTextNode baseTextNode = ScriptableObject.CreateInstance(type) as BaseTextNode;
            baseTextNode.guid = GUID.Generate().ToString();
            baseTextNode.name = type.Name;
            baseTextNode.Index = _nodes.Count;

            Undo.RecordObject(this, $"CreateNode({type.Name})");
            _nodes.Add(baseTextNode);

            if (baseTextNode is RootTextNode rootNode)
            {
                _rootNode = rootNode;
            }

            if (!EditorApplication.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(baseTextNode, this);
            }

            Undo.RegisterCreatedObjectUndo(baseTextNode, $"CreateNode({type.Name})");

            EditorUtility.SetDirty(this);
            SaveTree();
            return baseTextNode;
        }

        public void RemoveNode(BaseTextNode baseTextNode)
        {
            Undo.RecordObject(this, $"DeleteNode({baseTextNode.name})");

            _nodes.Remove(baseTextNode);
            Undo.DestroyObjectImmediate(baseTextNode);

            EditorUtility.SetDirty(this);
            SaveTree();
        }

        public void SetChild(BaseTextNode parent, BaseTextNode child)
        {
            Undo.RecordObject(parent, $"Connect({parent.name}To{child.name})");

            if (parent is ISingleNode<BaseTextNode> singleNode)
            {
                singleNode.SetChild(child);
            }
            else if (parent is IMutipleNode<BaseTextNode> mutiNode)
            {
                mutiNode.AddChild(child);
            }


            EditorUtility.SetDirty(parent);
            SaveTree();
        }

        public void SetChildren(BaseTextNode parent, BaseTextNode[] children, DialogueInfo dialogueInfo)
        {
            for (int i = 0; i < children.Length; i++)
            {
                switch (parent)
                {
                    case ConditionNode conditionNode:
                        conditionNode.AddChild(children[i], dialogueInfo.ConditionExpressions[i]);
                        break;
                    case ChoiceNode choiceNode:
                        choiceNode.AddChild(children[i], dialogueInfo.ChoiceTexts[i]);
                        break;
                    case ISingleNode<BaseTextNode> singleNode:
                        singleNode.SetChild(children[0]);
                        break;
                }
            }
        }

        public void BreakChild(BaseTextNode parent, BaseTextNode child)
        {
            Undo.RecordObject(parent, $"Disconnect({parent.name}To{child.name})");

            if (parent is ISingleNode<BaseTextNode> singleNode && singleNode.Child == child)
            {
                singleNode.DiscardChild();
            }
            else if (parent is IMutipleNode<BaseTextNode> mutiNode)
            {
                mutiNode.RemoveChild(child);
            }

            EditorUtility.SetDirty(parent);
            SaveTree();
        }

        public void SaveTree()
        {
            AssetDatabase.SaveAssetIfDirty(this);
        }

        public void BreadthFirstSet(int heightOffset, int widthOffset)
        {
            HashSet<BaseTextNode> visited = new HashSet<BaseTextNode>()
                    {
                        _rootNode,
                    };

            _rootNode.position = new Vector2();
            BreadthFirstSetInternal(_rootNode, visited, heightOffset, widthOffset);

            void BreadthFirstSetInternal(BaseTextNode parent, HashSet<BaseTextNode> visited, int heightOffset, int widthOffset)
            {
                if (parent.IsEndNode)
                {
                    return;
                }

                List<BaseTextNode> children = parent.GetChildren();

                int startIndex = -(children.Count / 2);
                int startYPos = children.Count % 2 == 0 ? heightOffset / 2 : 0;
                int nowIndex = 0;

                do
                {
                    BaseTextNode nowChild = children[nowIndex];
                    if (!visited.Contains(nowChild))
                    {
                        visited.Add(nowChild);
                        children[nowIndex].position = parent.position +
                            new Vector2(parent is MutipleTextNode ? widthOffset * 1.2f : widthOffset, startYPos + heightOffset * startIndex++);
                    }

                    nowIndex++;
                } while (nowIndex < children.Count);


                for (int i = 0; i < children.Count; i++)
                {
                    BreadthFirstSetInternal(children[i], visited, heightOffset, widthOffset);
                }
            }
        }

        public void Rearrange()
        {
            _nodes.Sort((a, b) => a.Index - b.Index);
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].Index = i + 1;
            }
        }

        private void SetAllNone(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingPlayMode)
            {
                for (int i = 0; i < _nodes.Count; i++)
                {
                    _nodes[i].NowState = BaseNode.NodeState.None;
                }
            }
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= SetAllNone;
            EditorApplication.playModeStateChanged += SetAllNone;
        }
#endif
    }
}