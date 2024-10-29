using ZincFramework.MonoModel;


namespace ZincFramework.Network
{
    public sealed partial class NetworkManager
    {
        private class DisconnectObserver : IMonoObserver
        {
            public void NotifyObserver()
            {
                Instance.Disconnect(true);
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
