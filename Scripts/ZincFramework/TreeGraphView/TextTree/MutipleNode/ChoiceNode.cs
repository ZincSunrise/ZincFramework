using System;
using System.Collections.Generic;
using UnityEngine;



namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public class ChoiceNode : BaseTextNode, IMutipleNode<BaseTextNode>
            {
                [System.Serializable]
                public class ChoiceInfo
                {
                    public string ChoiceText => _choiceText;

                    public BaseTextNode ChoiceNode => _choiceNode;

                    [SerializeField]
                    private string _choiceText;

                    [SerializeField]
                    private BaseTextNode _choiceNode;

                    public ChoiceInfo() => _choiceText = string.Empty;

                    public ChoiceInfo(BaseTextNode baseTextNode) : this() => _choiceNode = baseTextNode;

                    public ChoiceInfo(BaseTextNode baseTextNode, string text) : this(baseTextNode)
                    {
                        _choiceText = text;
                    }
                }

                public int ChildCount => _choiceInfos.Count;

                public List<ChoiceInfo> ChoiceInfos => _choiceInfos;

                [SerializeField]
                private List<ChoiceInfo> _choiceInfos = new List<ChoiceInfo>();


                public BaseTextNode[] GetChildren()
                {
                    if(_choiceInfos == null || _choiceInfos.Count == 0)
                    {
                        return Array.Empty<BaseTextNode>();
                    }
                   
                    return _choiceInfos.ConvertAll((x) => x.ChoiceNode).ToArray();
                }

                public override void ClearChild()
                {
                    _choiceInfos = null;
                }

#if UNITY_EDITOR

                public override string InputHtmlColor => "#F9A03F";

                public override string OutputHtmlColor => "#EF233C";

                public void AddChild(BaseTextNode baseTextNode, string choiceText)
                {
                    _choiceInfos.Add(new ChoiceInfo(baseTextNode, choiceText));
                }

                public void AddChild(BaseTextNode baseTextNode)
                {
                    _choiceInfos.Add(new ChoiceInfo(baseTextNode));
                }

                public void RemoveChild(BaseTextNode baseTextNode)
                {
                    _choiceInfos.RemoveAll(x => x.ChoiceNode == baseTextNode);
                }

                public override DialogueInfo GetDialogueInfo()
                {
                    DialogueInfo dialogueInfo = base.GetDialogueInfo();
                    if(_choiceInfos != null)
                    {
                        ChoiceInfo[] choiceInfos = _choiceInfos.ToArray();
                        dialogueInfo.ChoiceTexts = Array.ConvertAll(choiceInfos, x => x.ChoiceText);
                        dialogueInfo.NextTextId = Array.ConvertAll(choiceInfos, x => x.ChoiceNode.Index);
                    }
                    

                    return dialogueInfo;
                }
#endif
            }
        }
    }
}