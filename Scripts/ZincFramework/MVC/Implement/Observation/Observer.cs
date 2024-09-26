using System;
using ZincFramework.MVC.Interfaces;



namespace ZincFramework
{
    namespace MVC
    {
        namespace Observation
        {
            public class Observer : IObserver
            {
                public Action<INotification> NotifiyMethod { get; }

                public object NotifiyContext { get; }

                public bool CompareContext(object context)
                {
                    return context.Equals(NotifiyContext);
                }

                public void NotifiyObserver(in INotification notification)
                {
                    NotifiyMethod.Invoke(notification);
                }


                public Observer(Action<INotification> notifiyMethod, object notifiyContext) 
                {
                    NotifiyContext = notifiyContext;
                    NotifiyMethod = notifiyMethod;
                }
            }
        }
    }
}