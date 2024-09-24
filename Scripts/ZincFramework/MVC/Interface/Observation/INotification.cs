



namespace ZincFramework
{
    namespace MVC 
    {
        namespace Interfaces
        {
            public interface INotification
            {
                string Name { get; }

                object Data { get; }

                string Type { get; }
            }
        }
    }
}
