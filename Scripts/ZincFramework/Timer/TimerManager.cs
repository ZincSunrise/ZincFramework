using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Events;
using ZincFramework.DataPool;


namespace ZincFramework
{
    public class TimerManager : BaseSafeSingleton<TimerManager>
    {
        private readonly Dictionary<int, TimerItem> _gameTimerDic = new Dictionary<int, TimerItem>();
        private readonly Dictionary<int, TimerItem> _realTimerDic = new Dictionary<int, TimerItem>();
        private readonly List<TimerItem> _willDeleteTimerList = new List<TimerItem>(12);

        private Coroutine _gameTimer;
        private Coroutine _realTimer;


        private long _timerId = 0;
        private bool _isStopping = true;

        private readonly WaitForSeconds _gameWait;
        private readonly WaitForSecondsRealtime _realWait;

        private readonly float _intervalCheckTime = 0.05f;
        private TimerManager()
        {
            _intervalCheckTime = FrameworkConsole.Instance.SharedData.intervalCheckTime;
            _gameWait = new WaitForSeconds(_intervalCheckTime);
            _realWait = new WaitForSecondsRealtime(_intervalCheckTime);
        }

        public void Start()
        {
            _isStopping = false;
            _realWait.Reset();
            _gameTimer = MonoManager.Instance.StartCoroutine(R_Start(false, _gameTimerDic));
            _realTimer = MonoManager.Instance.StartCoroutine(R_Start(true, _realTimerDic));
        }


        public void StopAllTiming()
        {
            _isStopping = true;
            MonoManager.Instance.StopCoroutine(_gameTimer);
            MonoManager.Instance.StopCoroutine(_realTimer);
        }

        private IEnumerator R_Start(bool isReal, Dictionary<int, TimerItem> timeDic)
        {
            while (true)
            {
                yield return isReal ? _realWait : _gameWait;

                foreach (TimerItem item in timeDic.Values)
                {
                    item.SubTime(_intervalCheckTime);
                    if (!item.IsRunning && !item.IsStopping)
                    {
                        _willDeleteTimerList.Add(item);
                    }
                }

                if (_willDeleteTimerList.Count > 0)
                {
                    foreach (TimerItem item in _willDeleteTimerList)
                    {
                        RemoveTimer(item.Id);
                    }

                    _willDeleteTimerList.Clear();
                }
            }
        }

        public int CreateTimer(bool isRealTimer, int offsetTime,bool isLoop ,ZincAction overAction, int intervalTime = 0, ZincAction intervalAction = null)
        {
            if (_isStopping)
            {
                Start();
            }

            _timerId++;
            TimerItem item = DataPoolManager.RentInfo<TimerItem>();
            item.Initialize((int)_timerId, offsetTime, isLoop, overAction, intervalTime, intervalAction);

            if (isRealTimer)
            {
                _realTimerDic.Add(item.Id, item);
            }
            else
            {
                _gameTimerDic.Add(item.Id, item);
            }

            return item.Id;
        }

        public void RemoveTimer(int id)
        {
            StopTimer(id);
            if (_gameTimerDic.TryGetValue(id, out TimerItem timerItem))
            {
                _gameTimerDic.Remove(id);
                DataPoolManager.ReturnInfo(timerItem);
            }
            else if (_realTimerDic.TryGetValue(id, out timerItem))
            {
                _realTimerDic.Remove(id);
                DataPoolManager.ReturnInfo(timerItem);
            }
        }

        public void ResetTimer(int id)
        {
            if (_gameTimerDic.TryGetValue(id, out TimerItem timerItem))
            {
                timerItem.ResetTime();
            }
            else if (_realTimerDic.TryGetValue(id, out timerItem))
            {
                timerItem.ResetTime();
            }
        }


        public void StartTimer(int id)
        {
            if (_gameTimerDic.TryGetValue(id, out TimerItem timerItem))
            {
                timerItem.IsStopping = false;
            }
            else if (_realTimerDic.TryGetValue(id, out timerItem))
            {
                timerItem.IsStopping = false;
            }
        }

        public void StopTimer(int id)
        {
            if (_gameTimerDic.TryGetValue(id, out TimerItem timerItem))
            {
                timerItem.IsStopping = true;
            }
            else if (_realTimerDic.TryGetValue(id, out timerItem))
            {
                timerItem.IsStopping = true;
            }
        }
    }
}

