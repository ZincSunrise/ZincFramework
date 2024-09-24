using System;


namespace ZincFramework
{
    namespace MVC
    {
        namespace Interfaces
        {
            public interface IController
            {
                void RegistCommand(string name, Func<ICommand> factory);

                bool RemoveCommand(string name);

                bool IsHasCommand(string name);

                void ExcuteCommand(INotification notification);
            } 
        }
    }
}
