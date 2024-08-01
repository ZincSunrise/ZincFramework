using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using ZincFramework.Load.Editor;


namespace ZincFramework
{
    namespace UI
    {
        namespace TreeExtension
        {
            public class TreeWindow : EditorWindow
            {
                [MenuItem("GameTool/Tree/TreeWindow")]
                public static void ShowExample()
                {
                    TreeWindow wnd = GetWindow<TreeWindow>();
                    wnd.titleContent = new GUIContent("TreeWindow");
                    wnd.Show();
                }

                public void CreateGUI()
                {
                    VisualElement root = rootVisualElement;

                    StyleSheet styleSheet = AssetDataManager.LoadAssetAtPath<StyleSheet>("Editor/ZincFramework/Scripts/Extension/TreeGraph/TreeWindow");
                    root.styleSheets.Add(styleSheet);

                    VisualTreeAsset visualTreeAsset = AssetDataManager.LoadAssetAtPath<VisualTreeAsset>("Editor/ZincFramework/Scripts/Extension/TreeGraph/TreeWindow");
                    visualTreeAsset.CloneTree(root);
                }
            }

        }
    }
}
