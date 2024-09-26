using System;

namespace ZincFramework
{
    namespace MVC
    {
        namespace Interfaces
        {
            public interface IObserver
            {
                Action<INotification> NotifiyMethod { get; }

                object NotifiyContext { get; }

                void NotifiyObserver(in INotification notification);

                bool CompareContext(object context);
            }
        }
    }
}
