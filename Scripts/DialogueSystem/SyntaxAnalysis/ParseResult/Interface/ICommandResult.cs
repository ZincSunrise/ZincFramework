using System;
using ZincFramework.Analysis;


namespace ZincFramework.DialogueSystem.Analysis.SyntaxParse
{
    public interface ICommandResult : IParseResult<Action>, IExcuteable
    {
        
    }
}
