using UnityEngine;
using System.Collections.Generic;
using ZincFramework.TreeService;
using ZincFramework.DialogueSystem.TextData;


namespace ZincFramework.DialogueSystem
{
    public abstract class BaseTextNode : BaseNode
    {
        public abstract bool IsEndNode { get; }

        public string StaffName => _staffName;

        public string DialogueText => _dialogueText;

        public ref VisibleState[] VisibleStates { get => ref _visibleStates; }

        public int Index { get => _index; set => _index = value; }


        protected bool _isDestroying;

        [SerializeField]
        private string _staffName;

        [SerializeField]
        private string _dialogueText;

        [SerializeField]
        private VisibleState[] _visibleStates;

        [SerializeField]
        private int _index;

        public override void DestroyNode()
        {
            if (!_isDestroying)
            {
                _isDestroying = true;
                Destroy(this);
            }
        }

#if UNITY_EDITOR
        public virtual void Intialize(DialogueInfo dialogueInfo)
        {
            _visibleStates = dialogueInfo.VisibleStates;
            _staffName = dialogueInfo.CharacterName;
            _dialogueText = dialogueInfo.DialogueText;
            _index = dialogueInfo.TextId;
            position = dialogueInfo.Postion;
        }

        public virtual DialogueInfo GetDialogueInfo() => new()
        {
            TextId = _index,
            CharacterName = _staffName,
            DialogueText = _dialogueText,
            VisibleStates = _visibleStates,
            Postion = position,
        };
        public abstract List<BaseTextNode> GetChildren();
#endif
    }
}