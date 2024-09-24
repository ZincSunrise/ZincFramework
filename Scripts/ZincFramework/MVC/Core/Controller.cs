using System;
using System.Collections.Concurrent;
using System.Xml.Linq;
using ZincFramework.MVC.Interfaces;
using ZincFramework.MVC.Observation;



namespace ZincFramework
{
    namespace MVC
    {
        namespace Core
        {
            public sealed class Controller : Notifier, IController
            {
                public static Controller Instance => _instance.Value;

                private readonly static Lazy<Controller> _instance = new Lazy<Controller>(() => new Controller());

                private Controller() { }

                private IView View => Core.View.Instance;


                private readonly ConcurrentDictionary<string, Func<ICommand>> _commands = new ConcurrentDictionary<string, Func<ICommand>>();

                public void ExcuteCommand(INotification notification)
                {
                    if(_commands.TryGetValue(notification.Name, out var factory))
                    {
                        factory.Invoke().Execute(notification);
                    }
                }

                public void RegistCommand(string name, Func<ICommand> commandFactory)
                {
                    if (!_commands.ContainsKey(name))
                    {
                        View.RegistObserver(name, new Observer(ExcuteCommand, this));
                    }

                    _commands[name] = commandFactory;
                }

                public bool RemoveCommand(string name)
                {
                    if (!_commands.TryRemove(name, out _))
                    {
                        UnityEngine.Debug.LogWarning($"ÒÆ³ý{name}Ê§°Ü");
                        return false;
                    }

                    return true;
                }

                public bool IsHasCommand(string name)
                {
                    return _commands.ContainsKey(name);
                }
            }
        }
    }
}
