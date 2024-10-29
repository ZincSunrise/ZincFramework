using System;

namespace ZincFramework
{
    namespace MVC
    {
        namespace Interfaces
        {
            public interface IObserver
            {
                Action<Notification> NotifiyMethod { get; }

                object NotifiyContext { get; }

                void NotifiyObserver(in Notification notification);

                bool CompareContext(object context);
            }
        }
    }
}
