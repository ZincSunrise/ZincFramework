using System;

namespace ZincFramework
{
    namespace MVC
    {
        namespace Interfaces
        {
            public interface IObserver
            {
                /// <summary>
                /// ����
                /// </summary>
                Action<Notification> NotifiyMethod { get; }

                /// <summary>
                /// ˭����������۲���
                /// </summary>
                object NotifiyContext { get; }


                void NotifiyObserver(in Notification notification);

                bool CompareContext(object context);
            }
        }
    }
}
