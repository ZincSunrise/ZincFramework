using System.Collections.Generic;
using UnityEngine;
using ZincFramework.TreeService;


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

        public override BaseTextNode GetNextNode()
        {
            return _child;
        }

#if UNITY_EDITOR
        public override string InputHtmlColor => "#56EEF4";

        public override string OutputHtmlColor => "#90D7FF";

        public override List<BaseTextNode> GetChildren()
        {
            return IsEndNode ? null : new List<BaseTextNode>() { Child };
        }
#endif
    }
}
