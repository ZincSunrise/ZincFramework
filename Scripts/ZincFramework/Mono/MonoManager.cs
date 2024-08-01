using UnityEngine.Events;


namespace ZincFramework
{
    public class MonoManager : BaseAutoMonoSingleton<MonoManager>
    {
        public event UnityAction UpdateEvent;
        public event UnityAction FixedUpdateEvent;
        public event UnityAction LateUpdateEvent;
        public event UnityAction OnApplicationQuitEvent;

        public void ClearAllEvent()
        {
            UpdateEvent = null; 
            FixedUpdateEvent = null; 
            LateUpdateEvent = null; 
            OnApplicationQuitEvent = null;
        }

        private void Update()
        {
            UpdateEvent?.Invoke();
        }

        private void FixedUpdate()
        {
            FixedUpdateEvent?.Invoke();
        }

        private void LateUpdate()
        {
            LateUpdateEvent?.Invoke();
        }

        private void OnApplicationQuit()
        {
            OnApplicationQuitEvent?.Invoke();
        }


        /// <summary>
        /// 一定不要传lambda表达式除非你有特殊需求
        /// </summary>
        public void AddOnApplicationQuitListener(UnityAction quitAction)
        {
            OnApplicationQuitEvent += quitAction;
        }

        /// <summary>
        /// 一定不要传lambda表达式除非你有特殊需求
        /// </summary>
        public void AddUpdateListener(UnityAction updateAction)
        {
            UpdateEvent += updateAction;
        }

        /// <summary>
        /// 一定不要传lambda表达式除非你有特殊需求
        /// </summary>
        public void RemoveUpdateListener(UnityAction updateAction)
        {
            UpdateEvent -= updateAction;
        }

        /// <summary>
        /// 一定不要传lambda表达式除非你有特殊需求
        /// </summary>
        public void AddFixedUpdateListener(UnityAction fixedUpdateAction)
        {
            FixedUpdateEvent += fixedUpdateAction;
        }

        public void AddLateUpdateListener(UnityAction lateUpdateAction)
        {
            LateUpdateEvent += lateUpdateAction;
        }


        public void RemoveFixedUpdateListener(UnityAction fixedUpdateAction)
        {
            FixedUpdateEvent -= fixedUpdateAction;
        }

        public void RemoveLateUpdateListener(UnityAction lateUpdateAction)
        {
            LateUpdateEvent -= lateUpdateAction;
        }

        public void RemoveOnApplicationQuitListener(UnityAction quitAction)
        {
            OnApplicationQuitEvent -= quitAction;
        }

    }
}

