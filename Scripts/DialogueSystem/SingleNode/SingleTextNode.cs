using UnityEngine;
using System.Collections.Generic;
using ZincFramework.TreeService;
using ZincFramework.DialogueSystem.TextData;


namespace ZincFramework.DialogueSystem
{
    public class SingleTextNode : BaseTextNode, ISingleNode<BaseTextNode>
    {
        public override bool IsEndNode => _child == null;

        public BaseTextNode Child => _child;

        [SerializeField]
        protected BaseTextNode _child;

        public override BaseNode CloneNode()
        {
            SingleTextNode singleTextNode = ScriptableObject.Instantiate(this);

            if (!IsEndNode)
            {
                singleTextNode.SetChild(_child.CloneNode() as BaseTextNode);
            }

            return singleTextNode;
        }

        public override void DestroyNode()
        {
            Child?.DestroyNode();
            base.DestroyNode();
        }


#if UNITY_EDITOR
        public override void ClearChild()
        {
            _child = null;
        }

        public virtual void SetChild(BaseTextNode child)
        {
            _child = child;
        }

        public virtual void DiscardChild()
        {
            _child = null;
        }

        public override string InputHtmlColor => "#56EEF4";

        public override string OutputHtmlColor => "#90D7FF";


        public override DialogueInfo GetDialogueInfo()
        {
            DialogueInfo dialogueInfo = base.GetDialogueInfo();
            dialogueInfo.NextTextId = new int[1] { IsEndNode ? -1 : Child.Index };

            return dialogueInfo;
        }

        public override List<BaseTextNode> GetChildren()
        {
            return IsEndNode ? null : new List<BaseTextNode>() { Child };
        }
#endif
    }
}
