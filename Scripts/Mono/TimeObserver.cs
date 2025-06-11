using System;
using UnityEngine;

namespace ZincFramework.MonoModel
{
    public class TimeObserver : IMonoObserver
    {
        public bool IsTriggered { get; private set; }

        private Action _observation;

        private float _targetTime;

        private bool _isPause;

        public TimeObserver(Action observation, float targetTime) 
        {
            _observation = observation;
            _targetTime = targetTime;
        }

        public void Continue() => _isPause = false;

        public void Pause() => _isPause = true;

        public void NotifyObserver()
        {
            if (IsTriggered || _isPause)
            {
                return;
            }

            if(_targetTime <= 0)
            {
                IsTriggered = true;
                _observation?.Invoke();
            }

            _targetTime -= Time.fixedDeltaTime;
        }

        public void OnRegist()
        {

        }

        public void OnRemove()
        {

        }
    }
}
