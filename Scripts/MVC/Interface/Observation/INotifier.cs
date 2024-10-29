


namespace ZincFramework
{
    namespace MVC
    {
        namespace Interfaces
        {
            public interface INotifier
            {
                void SendNotification(string name, object data = null, string type = null);
            }
        }
    }
}
