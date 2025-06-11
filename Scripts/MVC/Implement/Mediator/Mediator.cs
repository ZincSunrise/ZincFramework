using System;
using ZincFramework.MVC.Interfaces;
using ZincFramework.MVC.Observation;


namespace ZincFramework
{
    namespace MVC
    {
        public abstract class Mediator : Notifier, IMediator
        {
            public string MediatorName { get; protected set; }

            public IViewBase View { get; set; }

            public Mediator(string mediatorName, IViewBase viewBase = null)
            {
                MediatorName = mediatorName;
                View = viewBase;
            }

            public virtual void HandleNotification(in Notification notification)
            {

            }

            public virtual void OnRegist()
            {
                
            }

            public virtual void OnRemove()
            {
                
            }

            public virtual string[] GetAttention() => Array.Empty<string>();
        }
    }
}