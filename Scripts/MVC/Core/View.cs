using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ZincFramework.MVC.Interfaces;
using ZincFramework.MVC.Observation;


namespace ZincFramework
{
    namespace MVC
    {
        namespace Core
        {
            public sealed class View : Notifier, IView
            {
                public static View Instance => _instance.Value;

                private readonly static Lazy<View> _instance = new Lazy<View>(() => new View());

                private readonly Dictionary<string, IMediator> _mediatorMap = new Dictionary<string, IMediator>();

                private readonly Dictionary<string, List<IObserver>> _observerMap = new Dictionary<string, List<IObserver>>();

                public void RegistMediator(IMediator mediator)
                {
                    if (_mediatorMap.TryAdd(mediator.MediatorName, mediator))
                    {
                        Observer observer = new Observer(x => mediator.HandleNotification(x), mediator);
                        var attentions = mediator.GetAttention();

                        for (int i = 0;i < attentions.Length; i++)
                        {
                            RegistObserver(attentions[i], observer);
                        }

                        mediator.OnRegist();
                    }
                }


                public void RemoveMediator(string mediatorName)
                {
                    if (_mediatorMap.Remove(mediatorName, out var mediator))
                    {
                        var attentions = mediator.GetAttention();

                        for (int i = 0; i < attentions.Length; i++)
                        {
                            RemoveObserver(attentions[i], mediator);
                        }

                        mediator.OnRemove();
                    }
                }

                public bool IsHasMediator(string mediatorName)
                {
                    return _mediatorMap.ContainsKey(mediatorName);
                }

                public IMediator GetMediator(string mediatorName)
                {
                    if (!_mediatorMap.TryGetValue(mediatorName, out var processor))
                    {
                        throw new ArgumentException($"不存在名字为{mediatorName}的中介对象");
                    }
                    return processor;
                }

                public void RegistObserver(string name, IObserver observer)
                {
                    if (!_observerMap.TryGetValue(name, out var observers))
                    {
                        observers = new List<IObserver>();
                        _observerMap.TryAdd(name, observers);
                    }

                    observers.Add(observer);
                }

                public void RemoveObserver(string name, object notifiyContext)
                {
                    if (_observerMap.TryGetValue(name, out var observers))
                    {
                        observers.RemoveAll((ob) => ob.CompareContext(notifiyContext));
                    }
                }

                public void NotifyObserver(in Notification notification)
                {
                    if (_observerMap.TryGetValue(notification.Name, out var observers))
                    {
                        for (int i = 0; i < observers.Count; i++)
                        {
                            observers[i].NotifiyObserver(notification);
                        }
                    }
                }

                public bool TryGetMediator(string mediatorName, out IMediator mediator)
                {
                    return _mediatorMap.TryGetValue(mediatorName, out mediator);
                }
            }
        }
    }
}
