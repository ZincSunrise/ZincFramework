using ZincFramework.MVC.Interfaces;


namespace ZincFramework
{
    namespace MVC
    {
        namespace Observation
        {
            public class Notifier : INotifier
            {
                public IFacade Facade => MVC.Facade.GetFacade(() => new Facade());

                public void SendNotification(string name, object data = null, string type = null)
                {
                    Facade.SendNotification(name, data, type);
                }
            }
        }
    }
}
