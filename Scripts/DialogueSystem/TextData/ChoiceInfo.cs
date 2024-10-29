using UnityEngine;


namespace ZincFramework.DialogueSystem.TextData
{
    [System.Serializable]
    public class ChoiceInfo
    {
        public string ChoiceText => _choiceText;

        public BaseTextNode ChoiceNode => _choiceNode;

        [SerializeField]
        private string _choiceText;

        [SerializeField]
        private BaseTextNode _choiceNode;

        public ChoiceInfo() => _choiceText = string.Empty;

        public ChoiceInfo(BaseTextNode baseTextNode) : this() => _choiceNode = baseTextNode;

        public ChoiceInfo(BaseTextNode baseTextNode, string text) : this(baseTextNode)
        {
            _choiceText = text;
        }

#if UNITY_EDITOR
        public void SetNode(BaseTextNode baseTextNode)
        {
            _choiceNode = baseTextNode;
        }
#endif
    }
}