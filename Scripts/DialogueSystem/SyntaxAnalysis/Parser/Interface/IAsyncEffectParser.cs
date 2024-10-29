using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ZincFramework.DialogueSystem.Analysis.SyntaxParse
{
    public interface IAsyncEffectParser
    {
        Task ExceteAsync(GameObject target, DialogueSyntax textSyntax, CancellationToken cancellationToken);
    }
}
