using System.Collections.Generic;
using ZincFramework.MonoModel;

namespace ZincFramework
{
    public class MonoManager : BaseAutoMonoSingleton<MonoManager>
    {
        private readonly List<IMonoObserver> _fixedUpdateObservers = new List<IMonoObserver>(0);
        private readonly List<IMonoObserver> _updateObservers = new List<IMonoObserver>(0);
        private readonly List<IMonoObserver> _lateUpdateObservers = new List<IMonoObserver>(0);
        private readonly List<IMonoObserver> _applicationQuitObservers = new List<IMonoObserver>(0);

        private readonly HashSet<IMonoObserver> _deleteFixedUpdateObservers = new HashSet<IMonoObserver>();
        private readonly HashSet<IMonoObserver> _deleteUpdateObservers = new HashSet<IMonoObserver>();
        private readonly HashSet<IMonoObserver> _deleteLateUpdateObservers = new HashSet<IMonoObserver>();

        public void AddUpdateListener(IMonoObserver updateObserver)
        {
            _updateObservers.Add(updateObserver);
            updateObserver.OnRegist();
        }

        public void RemoveUpdateListener(IMonoObserver updateObserver)
        {
            _deleteUpdateObservers.Add(updateObserver);
            updateObserver.OnRemove();
        }

        public void AddFixedUpdateObserver(IMonoObserver fixedUpdateObserver)
        {
            _fixedUpdateObservers.Add(fixedUpdateObserver);
            fixedUpdateObserver.OnRegist();
        }

        public void RemoveFixedUpdateObserver(IMonoObserver fixedUpdateObserver)
        {
            _deleteFixedUpdateObservers.Add(fixedUpdateObserver);
            fixedUpdateObserver.OnRemove();
        }

        public void AddLateUpdateListener(IMonoObserver lateUpdateObserver)
        {
            _lateUpdateObservers.Add(lateUpdateObserver);
            lateUpdateObserver.OnRegist();
        }

        public void RemoveLateUpdateListener(IMonoObserver lateUpdateObserver)
        {
            _deleteLateUpdateObservers.Add(lateUpdateObserver);
            lateUpdateObserver.OnRemove();
        }

        public void AddOnApplicationQuitListener(IMonoObserver quitObserver)
        {
            _applicationQuitObservers.Add(quitObserver); // ÐÞÕý
            quitObserver.OnRegist();
        }

        public void RemoveOnApplicationQuitListener(IMonoObserver quitObserver)
        {
            _applicationQuitObservers.Remove(quitObserver);
            quitObserver.OnRemove();
        }

        private void CleanupObservers(List<IMonoObserver> observers, HashSet<IMonoObserver> deleteObservers)
        {
            if (deleteObservers.Count > 0)
            {
                observers.RemoveAll(deleteObservers.Contains);
                deleteObservers.Clear();
            }
        }

        private void Update()
        {
            CleanupObservers(_updateObservers, _deleteUpdateObservers);
            for (int i = 0; i < _updateObservers.Count; i++)
            {
                _updateObservers[i].NotifyObserver();
            }
        }

        private void FixedUpdate()
        {
            CleanupObservers(_fixedUpdateObservers, _deleteFixedUpdateObservers);
            for (int i = 0; i < _fixedUpdateObservers.Count; i++)
            {
                _fixedUpdateObservers[i].NotifyObserver();
            }
        }

        private void LateUpdate()
        {
            CleanupObservers(_lateUpdateObservers, _deleteLateUpdateObservers);
            for (int i = _lateUpdateObservers.Count - 1; i >= 0; i--)
            {
                _lateUpdateObservers[i].NotifyObserver();
            }
        }

        private void OnApplicationQuit()
        {
            _applicationQuitObservers.ForEach(x => x.NotifyObserver());
        }
    }
}