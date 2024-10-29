namespace ZincFramework
{
    namespace MVC
    {
        namespace Interfaces
        {
            public interface IView
            {
                void RegistMediator(IMediator mediator);

                void RemoveMediator(string mediatorName);

                bool IsHasMediator(string mediatorName);

                bool TryGetMediator(string mediatorName, out IMediator mediator);

                IMediator GetMediator(string mediatorName);

                
                void RegistObserver(string name, IObserver observer);

                void RemoveObserver(string name, object notifiyContext);

                void NotifyObserver(in Notification notification);
            }
        }
    }
}