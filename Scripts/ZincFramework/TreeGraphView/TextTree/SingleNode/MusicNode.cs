using UnityEngine;
using ZincFramework.AudioExtension;

namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public class MusicNode : SingleTextNode
            {
                public string AudioName => _audioName;

                [SerializeField]
                private string _audioName;

                public override BaseTextNode Execute()
                {
                    if(string.Compare(_audioName, "PauseMusic", true) == 0)
                    {
                        MusicManager.Instance.PauseMusic(true);
                    }
                    else
                    {
                        MusicManager.Instance.ChangeMusic(_audioName);
                    }
                    
                    return base.Execute();
                }

#if UNITY_EDITOR

                public override void Intialize(DialogueInfo dialogueInfo)
                {
                    _audioName = dialogueInfo.MusicName;
                    base.Intialize(dialogueInfo);
                }

                public override string InputHtmlColor => "#BA247A";

                public override string OutputHtmlColor => "#357DED";
#endif
            }
        }
    }
}