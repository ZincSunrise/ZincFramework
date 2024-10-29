using ZincFramework.DataPools;

namespace ZincFramework
{
    namespace MVC
    {
        namespace Interfaces
        {
            public interface ICommand : IReuseable
            {
                void Execute(in Notification notification);
            }
        }
    }
}