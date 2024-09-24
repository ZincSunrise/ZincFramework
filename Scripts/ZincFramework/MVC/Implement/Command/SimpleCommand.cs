using System;
using ZincFramework.MVC.Interfaces;



namespace ZincFramework
{
    namespace MVC
    {
        public readonly struct SimpleCommand : INotifiedCommand
        {
            private readonly Action<INotification> _commandMethod;

            public IFacade Facade => MVC.Facade.GetFacade(() => new Facade());

            public SimpleCommand(Action<INotification> commandMethod)
            {
                _commandMethod = commandMethod;
            }

            public void Execute(INotification notification)
            {
                _commandMethod.Invoke(notification);
            }

            public void SendNotification(string name, object data = null, string type = null)
            {
                Facade.SendNotification(name, data, type);
            }
        }
    }
}
