using System;
using System.Collections.Generic;
using UnityEngine;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
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

#if UNITY_EDITOR
                private Dictionary<BaseTextNode, NodeParentCollection> _parentsInfo = new Dictionary<BaseTextNode, NodeParentCollection>();

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
                        if (!_parentsInfo.TryGetValue(children[i], out var collection))
                        {
                            collection = new NodeParentCollection(parent);
                            _parentsInfo[children[i]] = collection;
                        }

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

                        collection.ParentNodes.Add(parent);
                    }
                }

                public BaseTextNode[] GetChildren(BaseTextNode parent) => parent switch
                {
                    ISingleNode<BaseTextNode> singleNode => new[] { singleNode.Child },
                    IMutipleNode<BaseTextNode> mutipleNode => mutipleNode.GetChildren(),
                    _ => Array.Empty<BaseTextNode>()
                };

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


                public void BreadthFirstSearch(BaseTextNode baseTextNode)
                {
                    var queue = new Queue<KeyValuePair<int, BaseTextNode>>();
                    var visited = new HashSet<BaseTextNode>();

                    // 将根节点加入队列  
                    queue.Enqueue(new KeyValuePair<int, BaseTextNode>(0, baseTextNode));
                    visited.Add(baseTextNode);

                    while (queue.Count > 0)
                    {
                        var currentNodePair = queue.Dequeue();

                        // 在这个结构中，我们可以选择遍历子节点或父节点（或两者都遍历）  
                        // 这里我们遍历子节点来模拟传统的BFS  
                        foreach (var child in GetChildren(currentNodePair.Value))
                        {
                            int level = currentNodePair.Key;
                            if (!visited.Contains(child))
                            {
                                queue.Enqueue(new KeyValuePair<int, BaseTextNode>(level + 1, child));
                                visited.Add(child);
                            }
                        }
                    }
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
                        if (parent is EndTextNode)
                        {
                            return;
                        }

                        BaseTextNode[] children = GetChildren(parent);

                        int startIndex = -(children.Length / 2);
                        int startYPos = children.Length % 2 == 0 ? heightOffset / 2 : 0;
                        int nowIndex = 0;

                        do
                        {
                            BaseTextNode nowChild = children[nowIndex];
                            if (!visited.Contains(nowChild))
                            {
                                visited.Add(nowChild);
                                children[nowIndex].position = parent.position +
                                    new Vector2(parent is IMutipleNode<BaseTextNode> ? widthOffset * 1.2f : widthOffset, startYPos + heightOffset * startIndex++);
                            }

                            nowIndex++;
                        } while (nowIndex < children.Length);


                        for (int i = 0; i < children.Length; i++)
                        {
                            BreadthFirstSetInternal(children[i], visited, heightOffset, widthOffset);
                        }
                    }
                }
#endif
            }
        }
    }
}