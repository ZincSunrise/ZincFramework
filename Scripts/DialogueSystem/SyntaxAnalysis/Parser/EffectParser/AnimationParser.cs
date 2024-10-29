using System.Threading;
using System.Threading.Tasks;
using UnityEngine;



namespace ZincFramework.DialogueSystem.Analysis.SyntaxParse
{
    public class PlayAnimationParser : WaitableEffectParser<PlayAnimationResult>
    {
        public override async Task ExceteAsync(GameObject target, DialogueSyntax textSyntax, CancellationToken cancellationToken)
        {
            await ParseSyntaxTyped(target, textSyntax).ExcuteAsync(cancellationToken);
        }

        public override PlayAnimationResult ParseSyntaxTyped(GameObject obj, DialogueSyntax textSyntax)
        {
            Animation animation = AssetRepository.GetComponent<Animation>(obj);
            AnimationClip animationClip = AssetRepository.GetAsset<AnimationClip>(textSyntax.Argument);
            animation.clip = animationClip;
            animation.Play();
            
            return new PlayAnimationResult();
        }
    }
}