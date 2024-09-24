using System;
using ZincFramework.MVC.Core;
using ZincFramework.MVC.Interfaces;
using ZincFramework.MVC.Observation;


namespace ZincFramework
{
    namespace MVC
    {
        public class Facade : IFacade
        {
            public static IFacade GetFacade(Func<IFacade> factory)
            {
                if (factory == null) 
                {
                    return factory.Invoke();
                }

                return _instance;
            }
      
            protected static IFacade _instance;


            protected IController controller;

            protected IView view;

            protected IModel model;

            public Facade()
            { 
                if(_instance != null)
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
                controller = Controller.Instance;
            }

            protected virtual void InitializeView()
            {
                view = View.Instance;
            }

            protected virtual void InitializeModel()
            {
                model = Model.Instance;
            }


            public void RegistMediator(IMediator mediator)
            {
                view.RegistMediator(mediator);
            }

            public void RegistProcessor(IProcessor processor)
            {
                model.RegistProcessor(processor);
            }

            public void RegistCommand(string name, Func<ICommand> factory)
            {
                controller.RegistCommand(name, factory);
            }

            public void RemoveProcessor(string processorName)
            {
                model.RemoveProcessor(processorName);
            }

            public bool IsHasProcessor(string processorName)
            {
                return model.IsHasProcessor(processorName);
            }

            public IProcessor GetProcessor(string processorName)
            {
                return model.GetProcessor(processorName);
            }

            public void RemoveMediator(string mediatorName)
            {
                view.RemoveMediator(mediatorName);
            }

            public bool IsHasMediator(string mediatorName)
            {
                return view.IsHasMediator(mediatorName);
            }

            public IMediator GetMediator(string mediatorName)
            {
                return view.GetMediator(mediatorName);
            }

            public void RemoveCommand(string name)
            {
                controller.RemoveCommand(name);
            }

            public bool IsHasCommand(string name)
            {
                return controller.IsHasCommand(name);
            }


            public void NotifyObserver(INotification notification)
            {
                view.NotifyObserver(notification);
            }

            public void SendNotification(string name, object data = null, string type = null)
            {
                NotifyObserver(new Notification(name, data, type));
            }

            public bool TryGetProcessor(string processorName, out IProcessor processor)
            {
                return model.TryGetProcessor(processorName, out processor);
            }

            public bool TryGetMediator(string mediatorName, out IMediator mediator)
            {
                return view.TryGetMediator(mediatorName, out mediator);
            }

            public T GetProcessor<T>(string mediatorName) where T : IProcessor
            {
               return (T)model.GetProcessor(mediatorName);
            }

            public T GetMediator<T>(string mediatorName) where T : IMediator
            {
                return (T)view.GetMediator(mediatorName);
            }
        }
    }
}