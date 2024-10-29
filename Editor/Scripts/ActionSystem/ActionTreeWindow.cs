using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using ZincFramework.LoadServices.Editor;


namespace ZincFramework.ActionSystem
{
    public class ActionTreeWindow : EditorWindow
    {
        [MenuItem("GameTool/TreeGraphView/OpenActionTree")]
        public static void ShowExample()
        {
            ActionTreeWindow wnd = GetWindow<ActionTreeWindow>();
            wnd.titleContent = new GUIContent("ActionTreeWindow");
        }

        public void CreateGUI()
        {
            string loadPath = Path.Combine(AssetDataManager.FrameworkLoadPath, "TreeGraphView", "ActionGraphView", "ActionTreeWindow");
            VisualTreeAsset visualTreeAsset = AssetDataManager.LoadAssetAtPath<VisualTreeAsset>(loadPath);
            visualTreeAsset.CloneTree(rootVisualElement);

            StyleSheet styleSheet = AssetDataManager.LoadAssetAtPath<StyleSheet>(loadPath);
            rootVisualElement.styleSheets.Add(styleSheet);
        }
    }
}

