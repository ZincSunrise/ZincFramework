using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using ZincFramework.Load.Editor;
using System.IO;



namespace ZincFramework
{
    namespace UI
    {
        namespace Main
        {
            public class MainDataWindow : EditorWindow
            {
                [MenuItem("FrameworkConsole/MainDataWindow")]
                public static void ShowExample()
                {
                    MainDataWindow wnd = GetWindow<MainDataWindow>();
                    wnd.titleContent = new GUIContent("MainDataWindow");
                }

                public void CreateGUI()
                {
                    VisualTreeAsset asset = AssetDataManager.LoadAssetAtPath<VisualTreeAsset>(Path.Combine(AssetDataManager.ScriptLoadPath, "MainData", nameof(MainDataWindow)));
                    asset.CloneTree(rootVisualElement);

                    StyleSheet styleSheet = AssetDataManager.LoadAssetAtPath<StyleSheet>(Path.Combine(AssetDataManager.ScriptLoadPath, "MainData", nameof(MainDataWindow)));
                    rootVisualElement.styleSheets.Add(styleSheet);
                }
            }
        }
    }
}

