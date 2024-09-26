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
            /// ���ڰ���һ����֮��İ�
            /// </summary>
            /// <param name="inputListener"></param>
            /// <param name="actionName"></param>
            /// <param name="changeName">�����һ������ද��������Ҫ�������ϵ��ض����������</param>
            /// <param name="callback">�������ĺ󰴼����ַ���</param>
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
            /// ֱ�Ӹı�ĳһ����
            /// </summary>
            /// <param name="inputListener"></param>
            /// <param name="asset"></param>
            /// <param name="device">Ҫ�����д���豸Ӣ����</param>
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

