using ZincFramework.Events;

namespace ZincFramework.Loop
{
    /// <summary>
    /// Mono观察器，不会自动移除
    /// </summary>
    public class MonoObserver : IMonoObserver
    {
        public event ZincAction Observation;

        public MonoObserver(ZincAction observation)
        {
            Observation = observation;
        }

        public void OnRegist()
        {

        }

        public void OnRemove()
        {
            Observation = null;
        }

        public bool Tick()
        {
            Observation?.Invoke();
            return true;
        }
    }
}