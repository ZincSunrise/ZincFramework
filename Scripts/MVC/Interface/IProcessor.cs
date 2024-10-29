

namespace ZincFramework
{
    namespace MVC
    {
        namespace Interfaces
        {
            public interface IProcessor : INotifier
            {
                string ProcessorName { get; }


                object MyData { get; }


                void OnRegister();


                void OnRemove();
            }
        }
    }
}
