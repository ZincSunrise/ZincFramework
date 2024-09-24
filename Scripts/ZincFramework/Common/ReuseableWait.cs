using UnityEngine;
using ZincFramework.DataPool;


namespace ZincFramework
{
    public class ReuseableWait : IResumable
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

    public class ReuseableRealWait : IResumable
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
