using UnityEngine;

namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public abstract class BaseTextNode : BaseNode
            {
                public string StaffName => _staffName;

                public string Statement => _statement;

                public int Differential => _differential;

                public int Index => _index;
                
                [SerializeField]
                private string _staffName;

                [SerializeField]
                private string _statement;

                [SerializeField]
                private int _differential;

                [SerializeField]
                public int _index;

                /// <summary>
                /// 选择下一个节点的方法
                /// </summary>
                /// <returns></returns>
                public abstract BaseTextNode Execute();

#if UNITY_EDITOR
                public virtual void Intialize(DialogueInfo dialogueInfo)
                {
                    _differential = dialogueInfo.Differential;
                    _staffName = dialogueInfo.CharacterName;
                    _statement = dialogueInfo.DialogueText;
                    _index = dialogueInfo.TextId;
                }
#endif
            }
        }
    }
}