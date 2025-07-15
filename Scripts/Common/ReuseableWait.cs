using UnityEngine;
using ZincFramework.Pools;


namespace ZincFramework
{
    public class ReuseableWait : IReuseable
    {
        public WaitForSeconds WaitForSeconds { get; }

        public ReuseableWait(float waitSeconds)
        {
            WaitForSeconds = new WaitForSeconds(waitSeconds);
        }

        public void OnRent()
        {

        }

        public void OnReturn()
        {

        }
    }

    public class ReuseableRealWait : IReuseable
    {
        public WaitForSecondsRealtime WaitForSecondsRealtime { get; }

        public ReuseableRealWait(float waitSecondsRealtime) 
        {
            WaitForSecondsRealtime = new WaitForSecondsRealtime(waitSecondsRealtime);
        }

        public void OnRent()
        {
            WaitForSecondsRealtime.Reset();
        }

        public void OnReturn()
        {
            
        }
    }
}
