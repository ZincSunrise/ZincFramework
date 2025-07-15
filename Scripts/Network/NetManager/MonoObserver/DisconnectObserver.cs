using ZincFramework.Loop;


namespace ZincFramework.Network
{
    public sealed partial class NetworkManager
    {
        private class DisconnectObserver : IMonoObserver
        {
            public bool Tick()
            {
                Instance.Disconnect(true);
                return false;
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
