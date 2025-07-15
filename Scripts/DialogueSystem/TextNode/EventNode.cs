using System.Collections.Generic;
using UnityEngine;


namespace ZincFramework.DialogueSystem
{
    public class EventNode : SingleTextNode
    {
        public List<int> EventIds => _eventIds;

        /// <summary>
        /// 所有事件ID
        /// </summary>
        [SerializeField]
        private List<int> _eventIds = new List<int>();


#if UNITY_EDITOR
        public override string InputHtmlColor => "#F5E960";

        public override string OutputHtmlColor => "#E7E08B";
#endif
    }
}