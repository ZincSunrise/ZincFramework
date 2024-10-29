using UnityEngine;
using ZincFramework.Events;



namespace ZincFramework
{
    namespace InputListener
    {
        internal class KeyInfo : InputInfo
        {
            public KeyCode KeyCode { get; private set; }

            public override bool IsDoubleClick 
            { 
                get 
                {
                    if (Input.GetKeyDown(KeyCode))
                    {
                        float nowTime = Time.time;
                        if(nowTime - _lastPressTime >= _checkInterval)
                        {
                            _lastPressTime = nowTime;
                            return true;
                        }
                        return false;
                    }

                    return false;
                } 
            }


            private readonly ZincEvent<KeyCode> _inputEvent = new ZincEvent<KeyCode>();

            public KeyInfo(KeyCode keyCode, float checkInterval) : base(checkInterval)
            { 
                KeyCode = keyCode;
            }


            public void AddListener(ZincAction<KeyCode> inputAction)
            {
                _inputEvent.AddListener(inputAction);
            }

            public void RemoveListener(ZincAction<KeyCode> inputAction)
            {
                _inputEvent.RemoveListener(inputAction);
            }

            public void Clear()
            {
                _inputEvent.RemoveAllListeners();
            }

            public override void Excute()
            {
                if (Input.GetKeyDown(KeyCode))
                {
                    _inputEvent.Invoke(KeyCode);
                }
            }
        }
    }
}