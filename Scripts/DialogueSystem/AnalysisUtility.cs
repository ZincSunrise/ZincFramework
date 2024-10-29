using System;


namespace ZincFramework.DialogueSystem.Analysis
{
    public static class AnalysisUtility
    {
        public static ReadOnlySpan<char> GetArguments(string str)
        {
            return str.AsSpan()[(str.IndexOf('(') + 1)..^1];
        }
    }
}