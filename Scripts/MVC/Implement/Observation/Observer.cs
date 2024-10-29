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
                public Action<Notification> NotifiyMethod { get; }

                public object NotifiyContext { get; }

                public bool CompareContext(object context)
                {
                    return context.Equals(NotifiyContext);
                }

                public void NotifiyObserver(in Notification notification)
                {
                    NotifiyMethod.Invoke(notification);
                }


                public Observer(Action<Notification> notifiyMethod, object notifiyContext) 
                {
                    NotifiyContext = notifiyContext;
                    NotifiyMethod = notifiyMethod;
                }
            }
        }
    }
}