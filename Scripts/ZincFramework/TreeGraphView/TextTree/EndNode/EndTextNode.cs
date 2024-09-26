using System;
using UnityEngine;



namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            //所有没有子节点的结点的父类
            public class EndTextNode : BaseTextNode, IEndNode
            {
                private readonly static int[] _endTextId = new int[] { -1 };
                public string NextTextTreeName => _nextTextTreeName;

                public override string InputHtmlColor => "#8ACB88";

                public override string OutputHtmlColor => "#FFFFFF";

                [SerializeField]
                protected string _nextTextTreeName;

#if UNITY_EDITOR
                public sealed override void ClearChild()
                {

                }

                public override DialogueInfo GetDialogueInfo()
                {
                    DialogueInfo dialogueInfo = base.GetDialogueInfo();
                    dialogueInfo.NextTextId = _endTextId;
                    dialogueInfo.NextTextTreeName = _nextTextTreeName;
                    return dialogueInfo;
                }
#endif
            }
        }
    }
}