using System;
using ZincFramework.MVC.Core;
using ZincFramework.MVC.Interfaces;


namespace ZincFramework.MVC
{
    public class Facade : IFacade
    {
        public static IFacade GetFacade(Func<IFacade> factory) => _instance ??= factory.Invoke();

        protected static IFacade _instance;

        protected IController _controller;

        protected IView _view;

        protected IModel _model;

        public Facade()
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("不可以在单例模式实例化之后调用静态构造函数");
            }

            _instance = this;
            InitializeFacade();
        }

        protected virtual void InitializeFacade()
        {
            InitializeController();
            InitializeView();
            InitializeModel();
        }

        protected virtual void InitializeController()
        {
            _controller = Controller.Instance;
        }

        protected virtual void InitializeView()
        {
            _view = View.Instance;
        }

        protected virtual void InitializeModel()
        {
            _model = Model.Instance;
        }

        public void RegistMediator(IMediator mediator)
        {
            _view.RegistMediator(mediator);
        }

        public void RegistProcessor(IProcessor processor)
        {
            _model.RegistProcessor(processor);
        }

        public void RegistCommand(string name, Func<ICommand> factory)
        {
            _controller.RegistCommand(name, factory);
        }

        public void RemoveProcessor(string processorName)
        {
            _model.RemoveProcessor(processorName);
        }

        public bool HasProcessor(string processorName)
        {
            return _model.IsHasProcessor(processorName);
        }

        public IProcessor GetProcessor(string processorName)
        {
            return _model.GetProcessor(processorName);
        }

        public void RemoveMediator(string mediatorName)
        {
            _view.RemoveMediator(mediatorName);
        }

        public bool HasMediator(string mediatorName)
        {
            return _view.IsHasMediator(mediatorName);
        }

        public IMediator GetMediator(string mediatorName)
        {
            return _view.GetMediator(mediatorName);
        }

        public void RemoveCommand(string name)
        {
            _controller.RemoveCommand(name);
        }

        public bool HasCommand(string name)
        {
            return _controller.IsHasCommand(name);
        }

        public void NotifyObserver(Notification notification)
        {
            _view.NotifyObserver(notification);
        }

        public void SendNotification(string name, object data = null, string type = null)
        {
            NotifyObserver(new Notification(name, data, type));
        }

        public bool TryGetProcessor(string processorName, out IProcessor processor)
        {
            return _model.TryGetProcessor(processorName, out processor);
        }

        public bool TryGetMediator(string mediatorName, out IMediator mediator)
        {
            return _view.TryGetMediator(mediatorName, out mediator);
        }

        public T GetProcessor<T>(string mediatorName) where T : IProcessor
        {
            return (T)_model.GetProcessor(mediatorName);
        }

        public T GetMediator<T>(string mediatorName) where T : IMediator
        {
            return (T)_view.GetMediator(mediatorName);
        }
    }
}