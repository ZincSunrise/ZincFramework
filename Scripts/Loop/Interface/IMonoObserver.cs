namespace ZincFramework.Loop
{
    public interface IMonoObserver : ILoopItem
    {
        void OnRemove();

        void OnRegist();
    }
}