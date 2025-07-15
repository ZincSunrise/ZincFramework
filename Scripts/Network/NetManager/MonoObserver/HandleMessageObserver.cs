using ZincFramework.Loop;
using ZincFramework.Network.Protocol;

namespace ZincFramework.Network
{
    public sealed partial class NetworkManager
    {
        public class HandleMessageObserver : IMonoObserver
        {
            public bool Tick()
            {
                while (Instance._handlerQueue.Count > 0)
                {
                    if (Instance._handlerQueue.TryDequeue(out IHandleMessage handler))
                    {
                        handler.HandleMessage();
                    }
                }

                return true;
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