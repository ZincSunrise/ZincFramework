using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;


namespace ZincFramework
{
    namespace InputListener
    {
        public static class InputExtension
        {
            /// <summary>
            /// 用于按下一个键之后的绑定
            /// </summary>
            /// <param name="inputListener"></param>
            /// <param name="actionName"></param>
            /// <param name="changeName">如果是一个组合类动作，则需要传这个组合的特定方向的名称</param>
            /// <param name="callback">返还更改后按键的字符串</param>
            public static void PressChangeKeyBinding(this InputListener inputListener, InputActionAsset asset ,string mapName, string actionName, string changeName = "", UnityAction<string> callback = null)
            {
                InputActionMap map = asset.FindActionMap(mapName);
                InputAction action = map[actionName];

                InputSystem.onAnyButtonPress.CallOnce((control) =>
                {
                    if (inputListener.CanBinding)
                    {
                        string[] strs = control.path.Split('/');
                        (var device, var keyName) = (strs[1], strs[2]);

                        string path = $"<{device}>/{keyName}";

                        if(changeName == "")
                        {
                            action
                            .ChangeBinding(0)
                            .WithPath(path);
                        }
                        else
                        {
                            action
                            .ChangeBinding(changeName)
                            .WithPath(path);
                        }


                        callback?.Invoke(path);
                    }
                    else
                    {
                        inputListener.CanBinding = true;
                    }
                });
            }

            public static void PressChangeKeyBinding(this InputListener inputListener, PlayerInput playerInput, string mapName, string actionName, string changeName = "", UnityAction<string> callback = null)
            {
                InputActionMap map = playerInput.actions.FindActionMap(mapName);
                InputAction action = map[actionName];

                InputSystem.onAnyButtonPress.CallOnce((control) =>
                {
                    if (inputListener.CanBinding)
                    {
                        string[] strs = control.path.Split('/');
                        (var device, var keyName) = (strs[1], strs[2]);

                        string path = $"<{device}>/{keyName}";

                        if (changeName == "")
                        {
                            action
                            .ChangeBinding(0)
                            .WithPath(path);
                        }
                        else
                        {
                            action
                            .ChangeBinding(changeName)
                            .WithPath(path);
                        }


                        callback?.Invoke(path);
                    }
                    else
                    {
                        inputListener.CanBinding = true;
                    }
                });
            }

            public static void CancelBinding(this InputListener inputListener)
            {
                inputListener.CanBinding = false;
            }

            /// <summary>
            /// 直接改变某一个键
            /// </summary>
            /// <param name="inputListener"></param>
            /// <param name="asset"></param>
            /// <param name="device">要输入大写的设备英文名</param>
            /// <param name="keyName"></param>
            /// <param name="actionName"></param>
            /// <param name="changeName"></param>
            /// <param name="callback"></param>
            public static void ChangeKeyBinding(this InputListener inputListener, InputActionAsset asset, string mapName,string device, string keyName, string actionName, string changeName = "", UnityAction<string> callback = null)
            {
                InputActionMap map = asset.FindActionMap(mapName);
                InputAction action = map[actionName];
                string path = $"<{device}>/{keyName}";

                action
                .ChangeBinding(changeName)
                .WithPath(path);

                callback?.Invoke(path);
            }

            public static void ChangeKeyBinding(this InputListener inputListener, PlayerInput playerInput, string mapName, string device, string keyName, string actionName, string changeName = "", UnityAction<string> callback = null)
            {
                InputActionMap map = playerInput.actions.FindActionMap(mapName);
                InputAction action = map[actionName];
                string path = $"<{device}>/{keyName}";

                action
                .ChangeBinding(changeName)
                .WithPath(path);

                callback?.Invoke(path);
            }
        }
    }
}

