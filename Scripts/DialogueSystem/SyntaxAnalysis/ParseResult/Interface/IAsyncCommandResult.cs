using System;
using System.Threading;
using System.Threading.Tasks;
using ZincFramework.Analysis;


namespace ZincFramework.DialogueSystem.Analysis.SyntaxParse
{
    public interface IAsyncCommandResult : IParseResult<Func<CancellationToken, Task>>, IAsyncExcuteable
    {
        
    }
}
