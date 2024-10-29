using ZincFramework.Events;

namespace ZincFramework.DialogueSystem
{
    public interface ITextShower 
    {
        void ShowTextAsync(string text, ZincAction OnShowComplete = null, float showOffset = -1);

        void CompleteImmidately(ZincAction OnShowComplete = null);
    }
}