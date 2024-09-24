using UnityEngine;


namespace ZincFramework
{
    namespace TreeGraphView
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

            public NodeState NowState { get => _nowState; set => _nowState = value; }

            [SerializeField]
            private NodeState _nowState;

            /// <summary>
            /// 节点触发时的特殊事件
            /// </summary>

            public abstract void ClearChild();

            public virtual BaseNode CloneNode()
            {
                return ScriptableObject.Instantiate(this);
            }

#if UNITY_EDITOR
            public abstract string InputHtmlColor { get; }

            public abstract string OutputHtmlColor { get; }

            public Vector2 position;

            public string guid;
#endif
        }
    }
}