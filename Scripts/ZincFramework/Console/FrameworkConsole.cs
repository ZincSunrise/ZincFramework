using UnityEngine;


namespace ZincFramework
{
    public class FrameworkConsole : BaseSafeSingleton<FrameworkConsole>
    {
        public FrameworkData SharedData { get; }

        private FrameworkConsole() 
        {
            SharedData = Resources.Load<FrameworkData>("FrameWork/FrameWorkData");
        }
    }
}
