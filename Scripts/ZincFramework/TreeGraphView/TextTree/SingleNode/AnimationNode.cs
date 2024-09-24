using UnityEngine;

namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public class AnimationNode : SingleTextNode
            {
                public string AnimationPath { get => _animationPath; set => _animationPath = value; }

                [SerializeField]
                private string _animationPath;

                public string SoundName { get => _soundName; set => _soundName = value; }

                [SerializeField]
                private string _soundName;

                public override BaseTextNode Execute()
                {
                    return base.Execute();
                }

#if UNITY_EDITOR

                public override void Intialize(DialogueInfo dialogueInfo)
                {
                    _animationPath = dialogueInfo.AnimationName;
                    base.Intialize(dialogueInfo);
                }

                public override string InputHtmlColor => "#AFB3F7";

                public override string OutputHtmlColor => "#FF8630";
#endif
            }
        }
    }
}