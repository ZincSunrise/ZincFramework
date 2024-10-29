using ZincFramework.MVC.Interfaces;


namespace ZincFramework.MVC
{
    public abstract class SimpleCommand : ICommand, INotifier
    {
        public IFacade Facade => MVC.Facade.GetFacade(() => new Facade());

        public abstract void Execute(in Notification notification);

        public void SendNotification(string name, object data = null, string type = null)
        {
            Facade.SendNotification(name, data, type);
        }

        public virtual void OnRent()
        {

        }

        public virtual void OnReturn()
        {

        }
    }
}
