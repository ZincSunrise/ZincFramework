namespace ZincFramework
{
    namespace Events
    {
        public abstract class ZincEventBase
        {
            internal InvokableList _callsList = new InvokableList();
            public int EventCount => _callsList.Count;

            public virtual void RemoveAllListeners()
            {
                _callsList.Clear();
            }
        }
    }
}
