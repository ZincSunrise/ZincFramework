namespace ZincFramework.MonoModel
{
    public interface IMonoObserver
    {
        /// <summary>
        /// �����¼�����ĺ���
        /// </summary>
        void NotifyObserver();

        void OnRemove();

        void OnRegist();
    }
}