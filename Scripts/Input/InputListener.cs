using System;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Events;


namespace ZincFramework
{
    namespace InputListener
    {
        /// <summary>
        /// 如果启用了新输入系统，这个类只能用于改键
        /// </summary>
        public class InputListener : BaseAutoMonoSingleton<InputListener>
        {
            private readonly List<InputInfo> _inputInfos = new List<InputInfo>(8);

            public bool CanBinding { get; private set; }

            private bool _isInputSystem = false;

            private readonly float _intervalTime = 0.2f;


            private void Awake()
            {
                _isInputSystem = FrameworkConsole.Instance.SharedData.isInputSystem;
                if (_isInputSystem)
                {
                    return;
                }
            }


            private void Update()
            {
                if (_isInputSystem)
                {
                    return;
                }
                for(int i = 0;i < _inputInfos.Count; i++)
                {
                    _inputInfos[i].Excute();
                }
            }


            public bool IsDoublePress(KeyCode keyCode) => (bool)FindInputInfo(keyCode)?.IsDoubleClick;

            public bool IsDoubleClick(int mouseButton)=> (bool)FindInputInfo(mouseButton)?.IsDoubleClick;


            private void CheckIsInputSystem()
            {
                if (_isInputSystem)
                {
                    throw new ArgumentException("启用的是新输入系统，请不要使用这些API");
                }
            }


            private InputInfo FindInputInfo(KeyCode keyCode)
            {
                return _inputInfos.Find((x) =>
                {
                    if (x is KeyInfo keyInfo)
                    {
                        if (keyInfo.KeyCode == keyCode)
                        {
                            return true;
                        }
                    }

                    return false;
                });
            }

            private InputInfo FindInputInfo(int mouseButtonCode)
            {
                return _inputInfos.Find((x) =>
                {
                    if (x is MouseInfo mouseInfo)
                    {
                        if (mouseInfo.MouseButtonCode == mouseButtonCode)
                        {
                            return true;
                        }
                    }

                    return false;
                });
            }


            public void AddListeningKey(KeyCode keyCode)
            {
                CheckIsInputSystem();
                InputInfo inputInfo = FindInputInfo(keyCode);
                if(inputInfo == null)
                {
                    inputInfo = new KeyInfo(keyCode, _intervalTime);
                    _inputInfos.Add(inputInfo);
                }
            }


            public void RemoveListeningKey(KeyCode keyCode)
            {
                CheckIsInputSystem();
                InputInfo inputInfo = FindInputInfo(keyCode);
                if (inputInfo != null) 
                {
                    _inputInfos.Remove(inputInfo);
                }
            }



            public void AddKeyListener(KeyCode keyCode, ZincAction<KeyCode> inputAction)
            {
                CheckIsInputSystem();

                InputInfo inputInfo = FindInputInfo(keyCode);
                if (inputInfo == null)
                {
                    inputInfo = new KeyInfo(keyCode, _intervalTime);
                    _inputInfos.Add(inputInfo);
                }

                (inputInfo as KeyInfo).AddListener(inputAction);
            }

            public void RemoveKeyListener(KeyCode keyCode, ZincAction<KeyCode> inputAction)
            {
                CheckIsInputSystem();
                InputInfo inputInfo = FindInputInfo(keyCode);
                if (inputInfo != null)
                {
                    (inputInfo as KeyInfo).RemoveListener(inputAction);
                }
            }


            public void AddMouseListener(int buttonType, ZincAction<int> inputAction)
            {
                CheckIsInputSystem();

                InputInfo inputInfo = FindInputInfo(buttonType);
                if (inputInfo == null)
                {
                    inputInfo = new MouseInfo(buttonType, _intervalTime);
                    _inputInfos.Add(inputInfo);
                }

                (inputInfo as MouseInfo).AddListener(inputAction);
            }


            public void RemoveMouseListener(int buttonType, ZincAction<int> inputAction)
            {
                CheckIsInputSystem();
                InputInfo inputInfo = FindInputInfo(buttonType);
                if (inputInfo != null)
                {
                    (inputInfo as MouseInfo).RemoveListener(inputAction);
                }
            }

            public void Clear()
            {
                CheckIsInputSystem();
                _inputInfos.Clear();
            }
        }
    }
}

