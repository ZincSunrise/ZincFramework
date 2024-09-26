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

                public string DialogueText => _dialogueText;

                public int Differential => _differential;

                public int Index => _index;
                
                [SerializeField]
                private string _staffName;

                [SerializeField]
                private string _dialogueText;

                [SerializeField]
                private int _differential;

                [SerializeField]
                private int _index;

#if UNITY_EDITOR
                public virtual void Intialize(DialogueInfo dialogueInfo)
                {
                    _differential = dialogueInfo.Differential;
                    _staffName = dialogueInfo.CharacterName;
                    _dialogueText = dialogueInfo.DialogueText;
                    _index = dialogueInfo.TextId;
                }

                public virtual DialogueInfo GetDialogueInfo()
                {
                    DialogueInfo dialogueInfo = new DialogueInfo()
                    {
                        TextId = _index,
                        CharacterName = _staffName,
                        DialogueText = _dialogueText,
                        Differential = _differential,
                        XPosition = position.x,
                        Yposition = position.y,
                    };

                    return dialogueInfo;
                }
#endif
            }
        }
    }
}