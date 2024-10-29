using System;


namespace ZincFramework.DialogueSystem.Analysis
{
    /// <summary>
    /// 语法结构为 Animation(index,animationName)
    /// </summary>
    public class PlayAnimationAnalyzer : DialogueAnalyzer<PlayAnimationSyntax>
    {
        public override PlayAnimationSyntax AnalyzeTyped(string text)
        {
            var syntax = new PlayAnimationSyntax();
            ReadOnlySpan<char> charSpan = AnalysisUtility.GetArguments(text);
            int index = charSpan.IndexOf(',');
            syntax.Reset(new string(charSpan[..index]), new string(charSpan[(index + 1)..]));

            return syntax;
        }
    }
}
