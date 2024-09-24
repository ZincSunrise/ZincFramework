using Unity.VisualScripting;
using UnityEngine.Events;


namespace ZincFramework
{
    public class MonoManager : BaseAutoMonoSingleton<MonoManager>
    {
        public event UnityAction OnUpdate;
        public event UnityAction OnFixedUpdate;
        public event UnityAction OnLateUpdate;
        public event UnityAction OnQuitApplication;
        public event UnityAction OnGizmosDraw;

        public void ClearAllEvent()
        {
            OnGizmosDraw = null;
            OnUpdate = null; 
            OnFixedUpdate = null; 
            OnLateUpdate = null; 
            OnQuitApplication = null;
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        private void OnApplicationQuit()
        {
            OnQuitApplication?.Invoke();
        }

        private void OnDrawGizmos()
        {
            OnGizmosDraw?.Invoke();
        }

        /// <summary>
        /// 一定不要传lambda表达式除非你有特殊需求
        /// </summary>
        public void AddOnApplicationQuitListener(UnityAction quitAction)
        {
            OnQuitApplication += quitAction;
        }

        /// <summary>
        /// 一定不要传lambda表达式除非你有特殊需求
        /// </summary>
        public void AddUpdateListener(UnityAction updateAction)
        {
            OnUpdate += updateAction;
        }

        /// <summary>
        /// 一定不要传lambda表达式除非你有特殊需求
        /// </summary>
        public void RemoveUpdateListener(UnityAction updateAction)
        {
            OnUpdate -= updateAction;
        }

        /// <summary>
        /// 一定不要传lambda表达式除非你有特殊需求
        /// </summary>
        public void AddFixedUpdateListener(UnityAction fixedUpdateAction)
        {
            OnFixedUpdate += fixedUpdateAction;
        }

        public void AddLateUpdateListener(UnityAction lateUpdateAction)
        {
            OnLateUpdate += lateUpdateAction;
        }


        public void RemoveFixedUpdateListener(UnityAction fixedUpdateAction)
        {
            OnFixedUpdate -= fixedUpdateAction;
        }

        public void RemoveLateUpdateListener(UnityAction lateUpdateAction)
        {
            OnLateUpdate -= lateUpdateAction;
        }

        public void RemoveOnApplicationQuitListener(UnityAction quitAction)
        {
            OnQuitApplication -= quitAction;
        }

    }
}

