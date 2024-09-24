

namespace ZincFramework
{
    namespace MVC
    {
        namespace Interfaces
        {
            public interface ICommand
            {
                void Execute(INotification notification);
            }
        }
    }
}