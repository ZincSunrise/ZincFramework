using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Events;


namespace ZincFramework
{
    namespace InputListener
    {
        internal class MouseInfo : InputInfo
        {
            public int MouseButtonCode { get; private set; }

            public override bool IsDoubleClick
            {
                get
                {
                    if (Input.GetMouseButton(MouseButtonCode))
                    {
                        float nowTime = Time.time;
                        if (nowTime - _lastPressTime >= _checkInterval)
                        {
                            _lastPressTime = nowTime;
                            return true;
                        }
                        return false;
                    }

                    return false;
                }
            }


            private readonly ZincEvent<int> _inputEvent = new ZincEvent<int>();

            public MouseInfo(int mouseButtonCode, float checkInterval) : base(checkInterval)
            {
                if(mouseButtonCode != 1 && mouseButtonCode != 2 && mouseButtonCode != 0)
                {
                    throw new ArgumentException("字母按钮不能为1，2，0以外的数字");
                }

                MouseButtonCode = mouseButtonCode;
            }

            public void AddListener(ZincAction<int> inputAction)
            {
                _inputEvent.AddListener(inputAction);
            }

            public void RemoveListener(ZincAction<int> inputAction)
            {
                _inputEvent.RemoveListener(inputAction);
            }

            public void Clear()
            {
                _inputEvent.RemoveAllListeners();
            }

            public override void Excute()
            {
                _inputEvent.Invoke(MouseButtonCode);
            }
        }
    }
}