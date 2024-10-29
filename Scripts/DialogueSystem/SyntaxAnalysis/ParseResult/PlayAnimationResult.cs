using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace ZincFramework.DialogueSystem.Analysis.SyntaxParse
{
    public readonly struct PlayAnimationResult : IAsyncCommandResult
    {
        private readonly Animation _animation;

        private readonly AnimationClip _animationClip;

        public PlayAnimationResult(Animation animation, AnimationClip animationClip) 
        {
            _animation = animation;
            _animationClip = animationClip;
        }

        public async Task ExcuteAsync(CancellationToken cancellationToken)
        {
            _animation.clip = _animationClip;
            _animation.Play();
            await Task.Delay(Mathf.CeilToInt(1000 * _animationClip.length), cancellationToken);
        }

        public Func<CancellationToken, Task> GetResult() => ExcuteAsync;
    }
}