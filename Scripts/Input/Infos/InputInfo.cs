using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ZincFramework
{
    namespace InputListener
    {
        internal abstract class InputInfo
        {
            public abstract bool IsDoubleClick { get; }

            protected float _lastPressTime;

            protected float _checkInterval;

            public abstract void Excute();

            public InputInfo(float checkInterval)
            {
                _lastPressTime = Time.time;
                _checkInterval = checkInterval;
            }
        }
    }
}

