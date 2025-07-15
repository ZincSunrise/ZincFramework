using System.Collections.Generic;
using UnityEngine;


namespace ZincFramework.DialogueSystem
{
    public class RootTextNode : SingleTextNode
    {
        public List<int> Characters => _characters;

        public int BackgroundId => _backgroundId;

        [SerializeField]
        private List<int> _characters = new List<int>();

        [SerializeField]
        private int _backgroundId;

#if UNITY_EDITOR
        public override string InputHtmlColor => "#FFFFFF";

        public override string OutputHtmlColor => "#C9F9FF";
#endif
    }
}