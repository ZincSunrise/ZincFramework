using System;

namespace ZincFramework
{
    namespace MVC
    {
        namespace Interfaces
        {
            public interface IFacade : INotifier
            {
                void RegistProcessor(IProcessor processor);

                void RemoveProcessor(string processorName);

                bool IsHasProcessor(string processorName);

                bool TryGetProcessor(string processorName, out IProcessor processor);

                IProcessor GetProcessor(string processorName);

                T GetProcessor<T>(string mediatorName) where T : IProcessor;



                void RegistMediator(IMediator mediator);

                void RemoveMediator(string mediatorName);

                bool IsHasMediator(string mediatorName);

                bool TryGetMediator(string mediatorName, out IMediator mediator);

                IMediator GetMediator(string mediatorName);

                T GetMediator<T>(string mediatorName) where T : IMediator;


                void RegistCommand(string name, Func<ICommand> factory);

                void RemoveCommand(string name);

                bool IsHasCommand(string name);

                void NotifyObserver(INotification notification);
            }
        }
    }
}