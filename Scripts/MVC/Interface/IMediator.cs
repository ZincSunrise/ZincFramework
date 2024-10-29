using ZincFramework.MVC.Observation;
using ZincFramework.UI;



namespace ZincFramework
{
    namespace MVC
    {
        namespace Interfaces
        {
            public interface IMediator : INotifier
            {
                string MediatorName { get; }

                IViewBase View { get; set; }

                string[] GetAttention();

                void HandleNotification(in Notification notification);

                void OnRegist();

                void OnRemove();
            }
        }
    }
}

