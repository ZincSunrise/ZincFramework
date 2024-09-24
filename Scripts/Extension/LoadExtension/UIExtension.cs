using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using ZincFramework.Events;
using ZincFramework.Load.Extension;
using ZincFramework.UI.UIElements;
using static ZincFramework.UI.UIElements.UIElementManager;



namespace ZincFramework
{
    namespace UI
    {
        namespace Extension
        {
            public static class UIExtension
            {
                /// <summary>
                /// 拓展方法函数声明在前
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="manager"></param>
                /// <param name="callback"></param>
                /// <param name="layerType"></param>
                public static void ShowPanelAsync<T>(this UIManager manager, E_UILayerType layerType = E_UILayerType.Middle, UnityAction<T> callback = null) where T : BasePanel
                {
                    string panelName = typeof(T).Name;
                    if (manager.PanelDic.TryGetValue(panelName, out IPanelInfo basePanelInfo))
                    {
                        PanelInfo<T> panelInfo = basePanelInfo as PanelInfo<T>;
                        if (panelInfo.Panel != null)
                        {
                            manager.ShowPanel(panelInfo.Panel, layerType);
                            callback?.Invoke(panelInfo.Panel);
                        }
                        else
                        {
                            panelInfo.PanelAction += callback;
                        }

                        panelInfo.WillDestroy = false;
                        return;
                    }

                    manager.PanelDic.Add(panelName, new PanelInfo<T>(callback));
                    AddressablesManager.Instance.LoadAssetAsync<GameObject>(panelName, (obj) => manager.InitializePanel<T>(obj, layerType, panelName));
                }

                /// <summary>
                /// 拓展方法函数声明在前
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="manager"></param>
                /// <param name="callback"></param>
                /// <param name="type"></param>
                public static async Task<T> ShowPanelAsync<T>(this UIManager manager, E_UILayerType layerType) where T : BasePanel
                {
                    string panelName = typeof(T).Name;
                    PanelInfo<T> panelInfo;

                    if (manager.PanelDic.TryGetValue(panelName, out IPanelInfo basePanelInfo))
                    {
                        panelInfo = basePanelInfo as PanelInfo<T>;
                        if (panelInfo.Panel != null)
                        {
                            manager.ShowPanel(panelInfo.Panel, layerType);
                        }

                        panelInfo.WillDestroy = false;
                    }
                    else
                    {
                        panelInfo = new PanelInfo<T>();
                        manager.PanelDic.Add(panelName, panelInfo);
                        GameObject prefab = await AddressablesManager.Instance.LoadAssetAsync<GameObject>(panelName);

                        manager.InitializePanel<T>(prefab, layerType, panelName);
                    }

                    return panelInfo.Panel;
                }

                /// <summary>
                /// 不会显示面板，仅仅加载面板
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="manager"></param>
                /// <param name="type"></param>
                /// <param name="callback"></param>
                public async static Task<T> LoadPanelAsync<T>(this UIManager manager) where T : BasePanel
                {
                    string name = typeof(T).Name;
                    PanelInfo<T> panelInfo;

                    if (manager.PanelDic.TryGetValue(name, out IPanelInfo basePanelInfo))
                    {
                        panelInfo = basePanelInfo as PanelInfo<T>;
                        panelInfo.Panel.transform.SetParent(manager.GetLayer(E_UILayerType.Buttom));
                        return panelInfo.Panel;
                    }

                    panelInfo = new PanelInfo<T>();
                    manager.PanelDic.Add(name, panelInfo);

                    GameObject prefab = await AddressablesManager.Instance.LoadAssetAsync<GameObject>(name);
                    GameObject panelObject = GameObject.Instantiate(prefab, manager.GetLayer(E_UILayerType.Buttom));

                    if (!panelObject.TryGetComponent<T>(out var panel))
                    {
                        throw new ArgumentNullException($"该{panelObject.name}对象上不存在UI脚本");

                    }

                    panel.HideMe();
                    panel.gameObject.SetActive(false);
                    panelInfo.Panel = panel;

                    return panel;
                }

                public static void ShowViewAsync<T>(this UIElementManager manager, int layer = 0, ZincAction<T> callback = null) where T : BaseView, new()
                {
                    string name = typeof(T).Name;

                    UIElementInfo<T> info;
                    if (manager.ElementMap.TryGetValue(name, out var value))
                    {
                        info = value as UIElementInfo<T>;

                        if (info.IsLoading)
                        {
                            info.PanelAction += callback;
                        }
                        else
                        {
                            callback?.Invoke(info.View);
                        }

                        if (!info.UIDocument.gameObject.activeSelf)
                        {
                            info.UIDocument.gameObject.SetActive(true);
                        }

                        return;
                    }

                    info = new UIElementInfo<T>(callback);
                    manager.ElementMap.Add(name, info);

                    AddressablesManager.Instance.LoadAssetAsync<VisualTreeAsset>(name, (asset) =>
                    {
                        GameObject gameObject = new GameObject(name);
                        gameObject.transform.SetParent(manager.Container.transform, true);

                        UIDocument uIDocument = gameObject.AddComponent<UIDocument>();

                        uIDocument.panelSettings = manager.PanelSettings;
                        uIDocument.visualTreeAsset = asset;

                        var root = uIDocument.rootVisualElement;

                        T view = new T()
                        {
                            RootElement = root,
                        };


                        info.Init(view, uIDocument, layer);
                        info.PanelAction?.Invoke(info.View);
                    });
                }
            }
        }
    }
}

