using System.Collections.Generic;
using ZincFramework.MonoModel;


namespace ZincFramework
{
    public class MonoManager : BaseAutoMonoSingleton<MonoManager>
    {
        private readonly List<IMonoObserver> _fixedUpdateObversers = new List<IMonoObserver>(0);

        private readonly List<IMonoObserver> _updateObversers = new List<IMonoObserver>(0);

        private readonly List<IMonoObserver> _lateUpdateObversers = new List<IMonoObserver>(0);

        private readonly HashSet<IMonoObserver> _willDeteleUpdateObversers = new HashSet<IMonoObserver>();


        private readonly List<IMonoObserver> _applicationQuitObversers = new List<IMonoObserver>(0);


        public void AddUpdateListener(IMonoObserver updateObserver)
        {
            _updateObversers.Add(updateObserver);
            updateObserver.OnRegist();
        }

        public void RemoveUpdateListener(IMonoObserver updateObserver)
        {
            _updateObversers.Remove(updateObserver);
            updateObserver.OnRemove();
        }

        public void AddFixedUpdateObserver(IMonoObserver fixedUpdateObserver)
        {
            _fixedUpdateObversers.Add(fixedUpdateObserver);
            fixedUpdateObserver.OnRegist();
        }

        public void RemoveFixedUpdateObserver(IMonoObserver fixedUpdateObserver, bool isImmediately = false)
        {
            if (isImmediately)
            {
                _fixedUpdateObversers.Remove(fixedUpdateObserver);
            }
            else
            {
                _willDeteleUpdateObversers.Add(fixedUpdateObserver);
            }

            fixedUpdateObserver.OnRemove();
        }


        public void AddLateUpdateListener(IMonoObserver lateUpdateObserver)
        {
            _lateUpdateObversers.Add(lateUpdateObserver);
            lateUpdateObserver.OnRegist();
        }

        public void RemoveLateUpdateListener(IMonoObserver lateUpdateObserver)
        {
            _lateUpdateObversers.Remove(lateUpdateObserver);
            lateUpdateObserver.OnRemove();
        }


        public void AddOnApplicationQuitListener(IMonoObserver quitObserver)
        {
            _applicationQuitObversers.Add(quitObserver);
            quitObserver.OnRegist();
        }

        public void RemoveOnApplicationQuitListener(IMonoObserver quitObserver)
        {
            _applicationQuitObversers.Remove(quitObserver);
            quitObserver.OnRemove();
        }


        private void Update()
        {
            for (int i = _updateObversers.Count - 1; i >= 0; i--)
            {
                _updateObversers[i].NotifyObserver();
            }
        }

        private void FixedUpdate()
        {
            if(_willDeteleUpdateObversers.Count > 0)
            {
                _fixedUpdateObversers.RemoveAll(_willDeteleUpdateObversers.Contains);
                _willDeteleUpdateObversers.Clear();
            }

            for (int i = _fixedUpdateObversers.Count - 1; i >= 0; i--)
            {
                _fixedUpdateObversers[i].NotifyObserver();
            }
        }

        private void LateUpdate()
        {
            for (int i = _lateUpdateObversers.Count - 1; i >= 0; i--)
            {
                _lateUpdateObversers[i].NotifyObserver();
            }
        }

        private void OnApplicationQuit()
        {
            for (int i = _applicationQuitObversers.Count - 1; i >= 0; i--)
            {
                _applicationQuitObversers[i].NotifyObserver();
            }
        }
    }
}
