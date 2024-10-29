using UnityEngine;
using System.Collections.Generic;
using ZincFramework.DialogueSystem.TextData;


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

        public override DialogueInfo GetDialogueInfo()
        {
            DialogueInfo dialogueInfo = base.GetDialogueInfo();
            dialogueInfo.TextId = 1;
            dialogueInfo.EffectNames = new string[1] { string.Join('/', _characters) + '/' + _backgroundId };
            return dialogueInfo;
        }

        public override void Intialize(DialogueInfo dialogueInfo)
        {
            base.Intialize(dialogueInfo);
/*            string str = dialogueInfo.EffectNames[0];

            _backgroundId = str[^1] - '0';

            for (int i = 0; i < str.Length - 1; i++)
            {
                if (char.IsNumber(str[i]))
                {
                    _characters.Add(str[i] - '0');
                }
            }*/
        }
#endif
    }
}