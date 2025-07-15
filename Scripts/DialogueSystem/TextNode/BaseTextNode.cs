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

        /// <summary>
        /// 得到下一个对话节点的方法
        /// </summary>
        /// <returns></returns>
        public abstract BaseTextNode GetNextNode();

#if UNITY_EDITOR
        public abstract List<BaseTextNode> GetChildren();
#endif
    }
}