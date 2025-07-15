using System;
using UnityEditor;
using UnityEngine;
using ZincFramework.Excel;
using ZincFramework.Binary.Excel;
using System.Collections.Generic;
using ZincFramework.DialogueSystem.TextData;


namespace ZincFramework.DialogueSystem.GraphView
{
    public static class ExcelToTreeConverter
    {
/*        public static TextTree[] ExcelToTextTree(ExcelSheet[] tables, string savePath)
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
                if (!nodeIndex.TryGetValue(nodeInfo.TextId, out var node))
                {
                    node = textTree.CreateNodeInInfo(GetNodeType(nodeInfo));
                    node.Intialize(nodeInfo);
                    nodeIndex.Add(nodeInfo.TextId, node);
                }

                if (nodeInfo.NextTextId[0] == -1)
                {
                    continue;
                }

                BaseTextNode[] nextNodes = nodeInfo.NextTextId.Length == 0 ? Array.Empty<BaseTextNode>()
                    : new BaseTextNode[nodeInfo.NextTextId.Length];

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

                if (node.position != Vector2.zero)
                {
                    isAutoSet = false;
                }

                textTree.SetChildren(node, nextNodes, nodeInfo);
            }

            if (isAutoSet)
            {
                textTree.BreadthFirstSet(300, 450);
            }
            return textTree;
        }


        private static Type GetNodeType(DialogueInfo dialogueInfo) => dialogueInfo switch
        {
            not null when dialogueInfo.TextId == 1 => typeof(RootTextNode),
            not null when !ArrayListUtility.IsNullOrEmpty(dialogueInfo.ChoiceTexts) => typeof(ChoiceNode),
            not null when !ArrayListUtility.IsNullOrEmpty(dialogueInfo.ConditionExpressions) => typeof(ConditionNode),
            not null when !ArrayListUtility.IsNullOrEmpty(dialogueInfo.EventExpression) => typeof(EventNode),
            not null when !ArrayListUtility.IsNullOrEmpty(dialogueInfo.EffectNames) => typeof(EffectNode),
            _ => typeof(SingleTextNode),
        };*/
    }
}