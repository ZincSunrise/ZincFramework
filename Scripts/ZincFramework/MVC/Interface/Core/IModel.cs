using System.Collections.Generic;


namespace ZincFramework
{
    namespace MVC
    {
        namespace Interfaces
        {
            public interface IModel : INotifier
            {
                void RegistProcessor(IProcessor processor);

                bool RemoveProcessor(string processorName);

                bool IsHasProcessor(string processorName);

                bool TryGetProcessor(string processorName, out IProcessor processor);

                IProcessor GetProcessor(string processorName);
            }
        }
    }
}
