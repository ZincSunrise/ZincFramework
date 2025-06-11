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
                /// 方法
                /// </summary>
                Action<Notification> NotifiyMethod { get; }

                /// <summary>
                /// 谁创建的这个观察者
                /// </summary>
                object NotifiyContext { get; }


                void NotifiyObserver(in Notification notification);

                bool CompareContext(object context);
            }
        }
    }
}
