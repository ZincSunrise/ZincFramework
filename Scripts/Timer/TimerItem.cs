using ZincFramework.Events;
using ZincFramework.DataPools;


namespace ZincFramework
{
    internal class TimeEvent : ZincEvent{ }

    /// <summary>
    ///计量单位为毫秒
    /// </summary>
    internal class TimerItem : IReuseable
    {
        public TimeEvent OverEvent { get; private set; }
        public TimeEvent IntervalEvent { get; private set; }

        public int Id { get; private set; }

        public bool IsStopping { get; set; }

        public bool IsRunning { get; private set; }

        public bool IsInterval => IntervalEvent != null && IntervalEvent.EventCount != 0;

        private float _nowTime;

        private int _offsetTime;

        private float _intervalTime;

        private int _wholeIntervalTime;

        private bool _isLoop = false;

        public void Initialize(int id, int offsetTime, bool isLoop, ZincAction overAction, int intervalTime = 0, ZincAction intervalAction = null)
        {
            OverEvent ??= new TimeEvent();

            this.Id = id;
            this._nowTime = this._offsetTime = offsetTime;
            this._isLoop = isLoop;
            this.OverEvent.AddListener(overAction);

            if (intervalAction != null )
            {
                this._wholeIntervalTime = intervalTime;
                this._intervalTime = intervalTime;
                this.IntervalEvent ??= new TimeEvent();
                this.IntervalEvent.AddListener(intervalAction);
            }

            IsRunning = true;
        }

        public void ResetTime()
        {
            this._nowTime = this._offsetTime;
            this._intervalTime = this._wholeIntervalTime;
            IsRunning = true;

            IntervalEvent?.RemoveAllListeners();
            OverEvent?.RemoveAllListeners();
        }

        public void OnReturn()
        {
            OverEvent.RemoveAllListeners();
            IntervalEvent?.RemoveAllListeners();
        }

        public void OnRent()
        {

        }

        public void SubTime(float intervalTime)
        {
            if (IsInterval)
            {
                _intervalTime -= intervalTime;
                if (_intervalTime <= 0)
                {
                    IntervalEvent?.Invoke();
                    _intervalTime = _wholeIntervalTime;
                }
            }

            _nowTime -= intervalTime;
            if (_nowTime <= 0)
            {
                OverEvent?.Invoke();
                if (_isLoop)
                {
                    ResetTime();
                }
                else
                {
                    IsRunning = false;
                }
            }
        }
    }
}

