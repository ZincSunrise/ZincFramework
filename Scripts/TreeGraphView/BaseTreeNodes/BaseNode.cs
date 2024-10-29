using UnityEngine;
using ZincFramework.Events;


namespace ZincFramework.TreeService
{
    public abstract class BaseNode : ScriptableObject
    {
        public enum NodeState
        {
            None = 0,
            Start = 2,
            Running = 4,
            Done = 8,
        }
        public event ZincAction<NodeState> OnStateChanged;

        public NodeState NowState
        {
            get => _nowState;
            set
            {
                if (_nowState != value)
                {
                    _nowState = value;
                    OnStateChanged?.Invoke(_nowState);
                }
            }
        }

        [SerializeField]
        private NodeState _nowState;

        /// <summary>
        /// 节点触发时的特殊事件
        /// </summary>

        public abstract void ClearChild();

        public abstract BaseNode CloneNode();

        public abstract void DestroyNode();

#if UNITY_EDITOR
        public abstract string InputHtmlColor { get; }

        public abstract string OutputHtmlColor { get; }

        public Vector2 position;

        public string guid;
#endif
    }
}