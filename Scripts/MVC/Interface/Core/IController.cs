namespace ZincFramework
{
    namespace MVC
    {
        namespace Interfaces
        {
            public interface IController
            {
                void RegistCommand(string name, ICommand factory);

                bool RemoveCommand(string name);

                bool IsHasCommand(string name);

                void ExcuteCommand(Notification notification);
            } 
        }
    }
}
