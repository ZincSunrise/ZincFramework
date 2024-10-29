using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using ZincFramework.LoadServices;



namespace ZincFramework
{
    namespace UI
    {
        public class UIManager : BaseSafeSingleton<UIManager>
        {
            public static int UILayer { get; private set; }
            public Dictionary<string, IPanelInfo> PanelDic { get; } = new Dictionary<string, IPanelInfo>();

            private readonly Transform _topPosition;
            private readonly Transform _middlePosition;
            private readonly Transform _buttomPosition;
            private readonly Transform _systemPosition;

            public Camera UICamera { get; private set; }

            private UIManager()
            {
                UILayer = 1 << LayerMask.NameToLayer("UI");

                GameObject canvasObject = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
                GameObject UICameraObject = GameObject.Instantiate(Resources.Load<GameObject>("UI/UICamera2D"));

                GameObject eventSystemObject = GameObject.Instantiate(ResourcesManager.Instance.Load<GameObject>("UI/EventSystem"));

                UICamera = UICameraObject.GetComponent<Camera>();

                canvasObject.GetComponent<Canvas>().worldCamera = UICamera;
                UICamera.cullingMask = UILayer;
                UICamera.clearFlags = CameraClearFlags.Depth;

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

            public void ShowPanel(BasePanel basePanel, E_UILayerType layerType)
            {
                basePanel.transform.SetParent(GetLayer(layerType), false);

                if (!basePanel.gameObject.activeSelf)
                {
                    basePanel.gameObject.SetActive(true);
                    basePanel.ShowMe();
                }
            }

            public void ShowPanel<T>(E_UILayerType layerType = E_UILayerType.Middle, UnityAction<T> callback = null) where T : BasePanel
            {
                string panelName = typeof(T).Name;

                if (PanelDic.TryGetValue(panelName, out IPanelInfo basePanelInfo))
                {
                    PanelInfo<T> panelInfo = basePanelInfo as PanelInfo<T>;
                    if (panelInfo.Panel != null)
                    {
                        ShowPanel(panelInfo.Panel, layerType);
                        callback.Invoke(panelInfo.Panel);
                    }

                    else
                    {
                        panelInfo.PanelAction += callback;
                    }

                    panelInfo.WillDestroy = false;
                    return;
                }

                PanelDic.Add(panelName, new PanelInfo<T>(callback));
                AssetBundleManager.Instance.LoadAssetAsync<GameObject>("ui", panelName, (prefab) => InitializePanel<T>(prefab, layerType, panelName));
            }

            public void InitializePanel<T>(GameObject panelPrefab, E_UILayerType layerType, string panelName) where T : BasePanel
            {
                GameObject panelObject = GameObject.Instantiate(panelPrefab);
                panelObject.transform.SetParent(GetLayer(layerType), false);
                PanelInfo<T> panelInfo = PanelDic[panelName] as PanelInfo<T>;

                //此时证明加载完后后悔需要删除
                if (panelInfo.WillDestroy)
                {
                    PanelDic.Remove(panelName);
                    return;
                }

                panelInfo.Panel = panelObject.GetComponent<T>();
                if (panelInfo.Panel == null)
                {
                    Debug.LogError("该对象上不存在UI脚本");
                    return;
                }

                panelInfo.Panel.ShowMe();
                panelInfo.PanelAction?.Invoke(panelInfo.Panel);
                panelInfo.PanelAction = null;
                panelInfo.WillDestroy = false;
            }

            public void HidePanel(BasePanel basePanel, bool isFade = true)
            {
                if (isFade)
                {
                    basePanel.HideMe(() => FadePanel(basePanel));
                }
                else
                {
                    FadePanel(basePanel);
                }
            }

            public void HidePanel<T>(bool isFade = true, bool isDestroy = false) where T : BasePanel
            {
                string name = typeof(T).Name;
                if (PanelDic.TryGetValue(name, out IPanelInfo basePanelInfo))
                {
                    PanelInfo<T> panelInfo = basePanelInfo as PanelInfo<T>;

                    UnityAction hideCallback;

                    if (panelInfo.Panel == null)
                    {
                        panelInfo.WillDestroy = true;
                        panelInfo.PanelAction = null;
                    }
                    else
                    {
                        hideCallback = isDestroy ? () => DestroyPanel(panelInfo.Panel, name) : () => FadePanel(panelInfo.Panel);

                        if (isFade)
                        {
                            panelInfo.Panel.HideMe(hideCallback);
                        }
                        else
                        {
                            hideCallback.Invoke();
                        }
                    }
                }
            }

            public void DestroyPanel(BasePanel panel, string name)
            {
                GameObject.Destroy(panel.gameObject);
                PanelDic.Remove(name);
            }

            public void FadePanel(BasePanel panel)
            {
                panel.gameObject.SetActive(false);
            }

            public bool IsCantainPanel<T>() where T : BasePanel
            {
                string panelName = typeof(T).Name;
                if (PanelDic.TryGetValue(panelName, out IPanelInfo basePanelInfo))
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
                if (PanelDic.TryGetValue(panelName, out IPanelInfo basePanelInfo))
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

            public Vector3 ScreenToWorldPoint(Vector3 worldPoint) 
            {
                return UICamera.ScreenToWorldPoint(worldPoint);
            }
        }
    }
}

