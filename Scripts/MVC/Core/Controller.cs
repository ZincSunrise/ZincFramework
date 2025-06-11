using System;
using ZincFramework.MVC.Interfaces;
using ZincFramework.MVC.Observation;
using System.Collections.Generic;



namespace ZincFramework.MVC.Core
{
    /// <summary>
    /// 如果可能出现多线程请将 Dictionary<string, ICommand> 
    /// 改为                 ConcurrentDictionary<string, Func<ICommand>>
    /// 如果要复用命令，字典键值用传入对象池类 DataPool<ICommand>，但是要让ICommand继承IResumeable
    /// </summary>
    public sealed class Controller : Notifier, IController
    {
        public static Controller Instance => _instance.Value;

        private readonly static Lazy<Controller> _instance = new Lazy<Controller>(() => new Controller());

        private Controller() { }

        private IView View => Core.View.Instance;


        private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        public void ExcuteCommand(Notification notification)
        {
            if (_commands.TryGetValue(notification.Name, out var command))
            {
                command.Execute(notification);
            }
        }

        public void RegistCommand(string name, ICommand command)
        {
            if (!_commands.ContainsKey(name))
            {
                View.RegistObserver(name, new Observer(ExcuteCommand, this));
            }

            _commands[name] = command;
        }

        public bool RemoveCommand(string name)
        {
            if (!_commands.Remove(name, out _))
            {
                UnityEngine.Debug.LogWarning($"移除{name}失败");
                return false;
            }

            View.RemoveObserver(name, this);
            return true;
        }

        public bool IsHasCommand(string name)
        {
            return _commands.ContainsKey(name);
        }
    }
}
