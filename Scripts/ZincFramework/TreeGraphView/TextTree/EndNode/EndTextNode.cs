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
                public int EndIndex => _endIndex;

                public override string InputHtmlColor => "#8ACB88";

                public override string OutputHtmlColor => "#FFFFFF";

                [SerializeField]
                protected int _endIndex;

                public override BaseTextNode Execute() => null;

#if UNITY_EDITOR
                public sealed override void ClearChild()
                {

                }
#endif
            }
        }
    }
}