using UnityEngine;
using ZincFramework.Events;

namespace ZincFramework.DialogueSystem
{
    public interface ITextShower 
    {
        ZincAction OnTextBegin { get; set; }

        ZincAction OnTextEnd { get; set; }

        bool IsShowingText { get; }

        void SetRoleSprite(Sprite sprite);

        void StartShowText();

        void ShowTextAsync(string name, string text);

        void EndShowText();

        void SkipText();

        void CompleteImmidately();
    }
}