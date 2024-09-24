using ZincFramework.MVC.Interfaces;

namespace ZincFramework
{
    namespace MVC
    {
        public interface INotifiedCommand : ICommand, INotifier
        {
            IFacade Facade { get; }
        }
    }
}
