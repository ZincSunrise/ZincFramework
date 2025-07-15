using System.Collections.Generic;


namespace ZincFramework
{
    public class TimerManager : BaseSafeSingleton<TimerManager>
    {
        private readonly List<TimerItem> _willDeleteTimerList = new List<TimerItem>(12);

        private readonly float _intervalCheckTime = 0.05f;

        private TimerManager()
        {

        }
    }
}

