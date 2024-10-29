using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using ZincFramework.Events;
using ZincFramework.LoadServices;



namespace ZincFramework
{
    namespace UI
    {
        namespace UIElements
        {
            public class UIElementManager : BaseSafeSingleton<UIElementManager>
            {
                public Dictionary<string, BaseUIElementInfo> ElementMap => _elementMap;

                public GameObject Container => _container;

                public PanelSettings PanelSettings { get; }

                private readonly Dictionary<string, BaseUIElementInfo> _elementMap = new Dictionary<string, BaseUIElementInfo>();

                private readonly GameObject _container;


                private UIElementManager()
                {
                    PanelSettings = ResourcesManager.Instance.Load<PanelSettings>("UI/DefaultSettings");
                    _container = new GameObject("UIContainer");
                    GameObject.DontDestroyOnLoad(_container);
                }

                public class BaseUIElementInfo
                {
                    public UIDocument UIDocument { get; set; }

                    public int Layer { get; set; }

                    public bool IsLoading { get; set; } = true;

                    public void SetLayer(int layer)
                    {
                        Layer = layer;
                        UIDocument.sortingOrder = Layer;
                    }
                }

                public class UIElementInfo<T> : BaseUIElementInfo where T : BaseView
                {
                    public T View { get; set; }

                    public ZincAction<T> PanelAction { get; set; }


                    public void Init(T view, UIDocument uIDocument, int layer)
                    {
                        View = view;
                        UIDocument = uIDocument;
                        View.Initialize();
                        SetLayer(layer);
                    }

                    public UIElementInfo(ZincAction<T> panelAction)
                    {
                        PanelAction = panelAction;
                    }
                }


                public void ShowViewAsync<T>(int layer, ZincAction<T> callback = null) where T : BaseView, new()
                {
                    string name = typeof(T).Name;

                    UIElementInfo<T> info;
                    if (_elementMap.TryGetValue(name, out var value))
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
                    _elementMap.Add(name, info);

                    AssetBundleManager.Instance.LoadAssetAsync<VisualTreeAsset>("UI", name, (asset) =>
                    {
                        GameObject gameObject = new GameObject(name);
                        gameObject.transform.SetParent(_container.transform, true);

                        UIDocument uIDocument = gameObject.AddComponent<UIDocument>();

                        uIDocument.visualTreeAsset = asset;
                        uIDocument.panelSettings = PanelSettings;

                        var root = uIDocument.rootVisualElement;

                        T view = new T()
                        {
                            RootElement = root,
                        };

                        info.Init(view, uIDocument, layer);
                        info.PanelAction?.Invoke(info.View);
                    });
                }

                public void HideView<T>() where T : BaseView
                {
                    string name = typeof(T).Name;

                    if (_elementMap.TryGetValue(name, out var value))
                    {
                        if (value.UIDocument.gameObject.activeSelf)
                        {
                            value.UIDocument.gameObject.SetActive(false);
                        }     
                    }
                    else
                    {
                        Debug.LogWarning($"该面板{name}不存在");
                    }
                }

                public void SetLayer<T>(int layer) where T : BaseView
                {
                    string name = typeof(T).Name;

                    if (_elementMap.TryGetValue(name, out var value))
                    {
                        value.SetLayer(layer);
                    }
                    else
                    {
                        Debug.LogWarning($"该面板{name}不存在");
                    }
                }
            }
        }
    }
}
