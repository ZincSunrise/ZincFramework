using ZincFramework.MonoModel;
using ZincFramework.Network.Protocol;

namespace ZincFramework.Network
{
    public sealed partial class NetworkManager
    {
        public class HandleMessageObserver : IMonoObserver
        {
            public void NotifyObserver()
            {
                while (Instance._handlerQueue.Count > 0)
                {
                    if (Instance._handlerQueue.TryDequeue(out IHandleMessage handler))
                    {
                        handler.HandleMessage();
                    }
                }
            }

            public void OnRegist()
            {

            }

            public void OnRemove()
            {

            }
        }
    }
}