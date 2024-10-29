using System.Threading;
using System.Threading.Tasks;

namespace ZincFramework.Analysis
{
    public interface IAsyncExcuteable
    {
        Task ExcuteAsync(CancellationToken cancellationToken);
    }
}
