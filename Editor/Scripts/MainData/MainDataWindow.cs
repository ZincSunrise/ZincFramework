using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using ZincFramework.LoadServices.Editor;
using System.IO;



namespace ZincFramework.UI.Main
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
            VisualTreeAsset asset = AssetDataManager.LoadAssetAtPath<VisualTreeAsset>(Path.Combine(AssetDataManager.FrameworkLoadPath, "MainData", nameof(MainDataWindow)));
            asset.CloneTree(rootVisualElement);

            StyleSheet styleSheet = AssetDataManager.LoadAssetAtPath<StyleSheet>(Path.Combine(AssetDataManager.FrameworkLoadPath, "MainData", nameof(MainDataWindow)));
            rootVisualElement.styleSheets.Add(styleSheet);
        }
    }
}

