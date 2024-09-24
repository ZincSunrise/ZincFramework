using System;
using UnityEngine;



namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public class SingleTextNode : BaseTextNode, ISingleNode<BaseTextNode>
            {
                public BaseTextNode Child => _child;

                [SerializeField]
                protected BaseTextNode _child;

                public override BaseTextNode Execute() => _child;


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
#endif
            }
        }
    }
}
