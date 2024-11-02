using System;
using System.Collections.Generic;
using ZincFramework.DialogueSystem.TextData;
using ZincFramework.TreeService;
using UnityEngine;


namespace ZincFramework.DialogueSystem
{
    public class ChoiceNode : MutipleTextNode
    {
        public override int ChildCount => _choiceInfos.Count;

        public List<ChoiceInfo> ChoiceInfos => _choiceInfos;

        [SerializeField]
        private List<ChoiceInfo> _choiceInfos = new List<ChoiceInfo>();

        public override void ClearChild()
        {
            _choiceInfos = null;
        }

        public override BaseNode CloneNode()
        {
            ChoiceNode choiceNode = ScriptableObject.Instantiate(this);
            for (int i = 0; i < _choiceInfos.Count; i++)
            {
                var node = _choiceInfos[i].ChoiceNode.CloneNode();
                choiceNode.SetChild(i, node as BaseTextNode);
            }

            return choiceNode;
        }

        public override void DestroyNode()
        {
            for (int i = 0; i < ChoiceInfos.Count; i++)
            {
                ChoiceInfos[i].ChoiceNode.DestroyNode();
            }

            base.DestroyNode();
        }

#if UNITY_EDITOR

        public override string InputHtmlColor => "#F9A03F";

        public override string OutputHtmlColor => "#EF233C";

        public void AddChild(BaseTextNode baseTextNode, string choiceText)
        {
            _choiceInfos.Add(new ChoiceInfo(baseTextNode, choiceText));
        }

        public override void AddChild(BaseTextNode baseTextNode)
        {
            _choiceInfos.Add(new ChoiceInfo(baseTextNode));
        }

        public override void RemoveChild(BaseTextNode baseTextNode)
        {
            _choiceInfos.RemoveAll(x => x.ChoiceNode == baseTextNode);
        }

        public override DialogueInfo GetDialogueInfo()
        {
            DialogueInfo dialogueInfo = base.GetDialogueInfo();
            if (_choiceInfos != null)
            {
                ChoiceInfo[] choiceInfos = _choiceInfos.ToArray();
                dialogueInfo.ChoiceTexts = Array.ConvertAll(choiceInfos, x => x.ChoiceText);
                dialogueInfo.NextTextId = Array.ConvertAll(choiceInfos, x => x.ChoiceNode.Index);
            }


            return dialogueInfo;
        }

        public override List<BaseTextNode> GetChildren() => _choiceInfos.ConvertAll(x => x.ChoiceNode);

        public override void SetChild(int index, BaseTextNode node)
        {
            _choiceInfos[index].SetNode(node);
        }
#endif
    }
}