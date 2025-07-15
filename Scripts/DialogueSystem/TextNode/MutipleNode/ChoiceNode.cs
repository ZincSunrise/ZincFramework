using System.Collections.Generic;
using UnityEngine;
using ZincFramework.DialogueSystem.TextData;
using ZincFramework.TreeService;


namespace ZincFramework.DialogueSystem
{
    public class ChoiceNode : MutipleTextNode
    {
        public override int ChildCount => _choiceInfos.Count;

        public int ChoiceIndex { get; set; }

        public List<ChoiceInfo> ChoiceInfos => _choiceInfos;

        [SerializeField]
        private List<ChoiceInfo> _choiceInfos = new List<ChoiceInfo>();

        public override void ClearChild()
        {
            _choiceInfos.Clear();
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

        public override void SetChild(int index, BaseTextNode node)
        {
            _choiceInfos[index].SetNode(node);
        }

        public override BaseTextNode GetNextNode()
        {
            return _choiceInfos[ChoiceIndex].ChoiceNode;
        }

#if UNITY_EDITOR
        public override string InputHtmlColor => "#F9A03F";

        public override string OutputHtmlColor => "#EF233C";

        public override List<BaseTextNode> GetChildren() => _choiceInfos.ConvertAll(x => x.ChoiceNode);
#endif
    }
}