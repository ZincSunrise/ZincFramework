using System.Collections.Generic;
using UnityEngine;
using ZincFramework.TreeExtension.TreeNode;


namespace ZincFramework
{
    namespace TreeExtension
    {
        namespace TextTree
        {
            [CreateAssetMenu(menuName = "TextTree/TextSingleNode", fileName = "TextSingleNode_")]
            public class TextSingleNode : SingleNode
            {
                public string text;

                public override void Excute()
                {

                }
            }
        }
    }
}
