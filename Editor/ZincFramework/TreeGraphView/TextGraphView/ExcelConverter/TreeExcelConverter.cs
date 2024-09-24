using System;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;
using ZincFramework.Excel;
using ZincFramework.Serialization.Excel;



namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public static class TreeExcelConverter
            {
                public static TextTree[] ExcelToTextTree(ExcelSheet[] tables, string savePath)
                {
                    TextTree[] textTrees = new TextTree[tables.Length];
                    for (int i = 0; i < tables.Length; i++)
                    {
                        byte[] bytes = ExcelStreamWriter.CreateBytes(tables[i]);
                        var dialogueData = ExcelDeserializer.Deserialize<DialogueData, int, DialogueInfo>(bytes);

                        string relativePath = savePath.Substring(Application.dataPath.Length - "Assets".Length).Replace(tables[0].TableName, tables[i].TableName);
                        var textTree = CreateTree(relativePath, dialogueData);
                        textTree.name = tables[i].TableName;
                        textTrees[i] = textTree;
                    }

                    return textTrees;
                }

                private static TextTree CreateTree(string savePath, DialogueData dialogueData)
                {
                    var textTree = ScriptableObject.CreateInstance<TextTree>();
                    AssetDatabase.CreateAsset(textTree, savePath);

                    Dictionary<int, BaseTextNode> nodeIndex = new Dictionary<int, BaseTextNode>(dialogueData.DialogueInfos.Count);

                    bool isAutoSet = true;
                    foreach (var nodeInfo in dialogueData.DialogueInfos.Values)
                    {
                        if(!nodeIndex.TryGetValue(nodeInfo.TextId, out var node))
                        {
                            node = textTree.CreateNodeInInfo(GetNodeType(nodeInfo));
                            node.Intialize(nodeInfo);
                            nodeIndex.Add(nodeInfo.TextId, node);
                        }

                        if (nodeInfo.NextTextId[0] == -1)
                        {
                            continue;
                        }

                        BaseTextNode[] nextNodes = new BaseTextNode[nodeInfo.NextTextId.Length];
                        for (int i = 0; i < nodeInfo.NextTextId.Length; i++)
                        {
                            if (!nodeIndex.TryGetValue(nodeInfo.NextTextId[i], out var nextNode))
                            {
                                DialogueInfo nextDialogueInfo = dialogueData.DialogueInfos[nodeInfo.NextTextId[i]];
                                nextNode = textTree.CreateNodeInInfo(GetNodeType(nextDialogueInfo));
                                nextNode.Intialize(nextDialogueInfo);
                                nodeIndex.Add(nextDialogueInfo.TextId, nextNode);
                            }

                            nextNodes[i] = nextNode;
                        }

                        if (nodeInfo.XPosition != 0 || nodeInfo.Yposition != 0)
                        {
                            isAutoSet = false;
                        }
                        textTree.SetChildren(node, nextNodes, nodeInfo);
                    }

                    if (isAutoSet)
                    {
                        textTree.BreadthFirstSearch(textTree.RootTextNode);
                        textTree.BreadthFirstSet(250, 300);
                    }
                    
                    return textTree;
                }


                private static Type GetNodeType(DialogueInfo dialogueInfo) => dialogueInfo switch
                {
                    not null when dialogueInfo.TextId == 1 => typeof(RootTextNode),
                    not null when dialogueInfo.NextTextId[0] == -1 => typeof(EndTextNode),
                    not null when !ArrayListUtility.IsNullOrEmpty(dialogueInfo.ChoiceTexts) => typeof(ChoiceNode),
                    not null when !ArrayListUtility.IsNullOrEmpty(dialogueInfo.ConditionExpressions) => typeof(ConditionNode),
                    not null when !string.IsNullOrEmpty(dialogueInfo.EventExpression) => typeof(EventNode),
                    not null when !string.IsNullOrEmpty(dialogueInfo.AnimationName) => typeof(AnimationNode),
                    not null when !string.IsNullOrEmpty(dialogueInfo.MusicName) => typeof(MusicNode),
                    _ => typeof(SingleTextNode),
                };
            }
        }
    }
}