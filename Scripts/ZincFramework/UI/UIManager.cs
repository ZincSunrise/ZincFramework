using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ZincFramework.Load;


namespace ZincFramework
{
    namespace UI
    {
        public class UIManager : BaseSafeSingleton<UIManager>
        {
            public static readonly int UILayer;

            private readonly Dictionary<string, BasePanelInfo> _panelDic = new Dictionary<string, BasePanelInfo>();

            private readonly Transform _topPosition;
            private readonly Transform _middlePosition;
            private readonly Transform _buttomPosition;
            private readonly Transform _systemPosition;

            private readonly Camera _UICamera;

            static UIManager()
            {
                UILayer = 1 << LayerMask.NameToLayer("UI");
            }

            private class BasePanelInfo
            {
                public bool IsHide { get; set; }
            }

            private class PanelInfo<T> : BasePanelInfo where T : BasePanel
            {
                public UnityAction<T> panelAction;

                public T Panel { get; set; }

                public PanelInfo(UnityAction<T> action)
                {
                    this.panelAction += action;
                }
            }

            private UIManager()
            {
                GameObject canvasObject = GameObject.Instantiate(ResourcesManager.Instance.Load<GameObject>("UI/Canvas"));
                GameObject UICameraObject = GameObject.Instantiate(ResourcesManager.Instance.Load<GameObject>("UI/UICamera"));

                string eventSystemName = FrameworkData.Shared.isInputSystem ? "UI/InputEventSystem" : "UI/EventSystem";

                GameObject eventSystemObject = GameObject.Instantiate(ResourcesManager.Instance.Load<GameObject>(eventSystemName));
                _UICamera = UICameraObject.GetComponent<Camera>();

                canvasObject.GetComponent<Canvas>().worldCamera = _UICamera;
                _UICamera.cullingMask = UILayer;
                _UICamera.clearFlags = CameraClearFlags.Depth;

                _topPosition = canvasObject.transform.Find("Top");
                _middlePosition = canvasObject.transform.Find("Middle");
                _buttomPosition = canvasObject.transform.Find("Bottom");
                _systemPosition = canvasObject.transform.Find("System");

                if (_topPosition == null)
                {
                    Debug.LogError("你没有为UI画布添加顶部层级");
                }
                if (_middlePosition == null)
                {
                    Debug.LogError("你没有为UI画布添加中部层级");
                }
                if (_buttomPosition == null)
                {
                    Debug.LogError("你没有为UI画布添加底部层级");
                }
                if (_systemPosition == null)
                {
                    Debug.LogError("你没有为UI画布添加系统层级");
                }

                GameObject.DontDestroyOnLoad(UICameraObject);
                GameObject.DontDestroyOnLoad(canvasObject);
                GameObject.DontDestroyOnLoad(eventSystemObject);
            }

            public void ShowPanel<T>(bool isAsync, E_UILayerType type = E_UILayerType.Middle, UnityAction<T> callback = null) where T : BasePanel
            {
                string name = typeof(T).Name;
                if (_panelDic.TryGetValue(name, out BasePanelInfo basePanelInfo))
                {
                    PanelInfo<T> panelInfo = basePanelInfo as PanelInfo<T>;

                    if (panelInfo.Panel != null)
                    {
                        callback?.Invoke(panelInfo.Panel);

                        if (!panelInfo.Panel.gameObject.activeSelf)
                        {
                            panelInfo.Panel.gameObject.SetActive(true);
                            panelInfo.Panel.ShowMe();
                        }
                    }

                    else
                    {
                        panelInfo.IsHide = false;
                        if (callback != null)
                        {
                            panelInfo.panelAction += callback;
                        }
                    }
                    return;
                }

                _panelDic.Add(name, new PanelInfo<T>(callback));

                AssetBundleManager.Instance.LoadAssetAsync<GameObject>("ui", name, (obj) =>
                {
                    GameObject panelObject = GameObject.Instantiate(obj);
                    panelObject.transform.SetParent(GetLayer(type), false);
                    PanelInfo<T> panelInfo = _panelDic[name] as PanelInfo<T>;

                    if (panelInfo.IsHide)
                    {
                        _panelDic.Remove(name);
                        return;
                    }

                    T panel = panelObject.GetComponent<T>();
                    panel.ShowMe();
                    if (panel == null)
                    {
                        Debug.LogError("该对象上不存在UI脚本");
                        return;
                    }

                    panelInfo.panelAction?.Invoke(panel);
                    panelInfo.panelAction = null;
                    panelInfo.Panel = panel;
                }, isAsync);
            }

            public void HidePanel<T>(bool isFade = true, bool isDestroy = false) where T : BasePanel
            {
                string name = typeof(T).Name;
                if (_panelDic.TryGetValue(name, out BasePanelInfo basePanelInfo))
                {
                    PanelInfo<T> panelInfo = basePanelInfo as PanelInfo<T>;

                    if (panelInfo.Panel == null)
                    {
                        panelInfo.IsHide = true;
                        panelInfo.panelAction = null;
                    }

                    else
                    {
                        if (isDestroy)
                        {
                            if (isFade)
                            {
                                panelInfo.Panel.HideMe(() =>
                                {
                                    GameObject.Destroy(panelInfo.Panel.gameObject);
                                    _panelDic.Remove(name);
                                });
                            }
                            else
                            {
                                GameObject.Destroy(panelInfo.Panel.gameObject);
                                _panelDic.Remove(name);
                            }
                        }
                        else
                        {
                            if (isFade)
                            {
                                panelInfo.Panel.HideMe(() =>
                                {
                                    panelInfo.Panel.gameObject.SetActive(false);
                                });
                            }
                            else
                            {
                                panelInfo.Panel.gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }

            public bool IsCantainPanel<T>() where T : BasePanel
            {
                string panelName = typeof(T).Name;
                if (_panelDic.TryGetValue(panelName, out BasePanelInfo basePanelInfo))
                {
                    PanelInfo<T> panelInfo = basePanelInfo as PanelInfo<T>;

                    if (panelInfo.Panel)
                    {
                        return panelInfo.Panel.gameObject.activeSelf;
                    }
                    return false;
                }

                return false;
            }

            public T GetPanel<T>() where T : BasePanel
            {
                string panelName = typeof(T).Name;
                if (_panelDic.TryGetValue(panelName, out BasePanelInfo basePanelInfo))
                {
                    PanelInfo<T> panelInfo = basePanelInfo as PanelInfo<T>;

                    if (panelInfo.Panel)
                    {
                        return panelInfo.Panel;
                    }
                    return null;
                }

                return null;
            }

            public static void AddInputEvent(UIBehaviour UIBehaviour, EventTriggerType eventTriggerType, UnityAction<BaseEventData> inputEvent)
            {
                if (!UIBehaviour.TryGetComponent(out EventTrigger eventTrigger))
                {
                    eventTrigger = UIBehaviour.gameObject.AddComponent<EventTrigger>();
                }

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = eventTriggerType;

                entry.callback.AddListener(inputEvent);

                eventTrigger.triggers.Add(entry);
            }

            public Transform GetLayer(E_UILayerType layerType) => layerType switch
            {
                E_UILayerType.Top => _topPosition,
                E_UILayerType.Middle => _middlePosition,
                E_UILayerType.Buttom => _buttomPosition,
                E_UILayerType.System => _systemPosition,
                _ => _topPosition
            };
        }
    }
}

