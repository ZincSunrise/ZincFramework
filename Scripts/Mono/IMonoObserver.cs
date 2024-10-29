namespace ZincFramework.MonoModel
{
    public interface IMonoObserver
    {
        /// <summary>
        /// 控制事件输入的函数
        /// </summary>
        void NotifyObserver();

        void OnRemove();

        void OnRegist();
    }
}