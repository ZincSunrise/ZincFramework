using ZincFramework.MVC.Observation;

namespace ZincFramework.MVC.Interfaces
{
    public interface IFacade : INotifier
    {
        void RegistProcessor(IProcessor processor);

        void RemoveProcessor(string processorName);

        bool HasProcessor(string processorName);

        bool TryGetProcessor(string processorName, out IProcessor processor);

        IProcessor GetProcessor(string processorName);

        T GetProcessor<T>(string mediatorName) where T : IProcessor;



        void RegistMediator(IMediator mediator);

        void RemoveMediator(string mediatorName);

        bool HasMediator(string mediatorName);

        bool TryGetMediator(string mediatorName, out IMediator mediator);

        IMediator GetMediator(string mediatorName);

        T GetMediator<T>(string mediatorName) where T : IMediator;


        void RegistCommand(string name, ICommand factory);

        void RemoveCommand(string name);

        bool HasCommand(string name);

        void NotifyObserver(Notification notification);


        void RegistObserver(string name, IObserver observer);

        void UnregistObserver(string name, IObserver observer);
    }
}