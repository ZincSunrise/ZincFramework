using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.UIElements;
using ZincFramework.LoadServices.Editor;



namespace ZincFramework.DialogueSystem.GraphView
{
    public class TextTreeWindow : EditorWindow
    {
        public TextGraphView TextTreeGraph { get; private set; }

        public TextInspector TextInspector { get; private set; }

        public TextToolContainer TextToolBar { get; private set; }

        private TextTree _nowTextTree;

        public GenericDropdownMenu GenericDropdownMenu { get; private set; }


        [MenuItem("GameTool/TreeGraphView/OpenTextTree")]
        public static void ShowExample()
        {
            TextTreeWindow textTreeWindow = EditorWindow.GetWindow<TextTreeWindow>();
            textTreeWindow.Show();

            if(Selection.activeObject is not TextTree)
            {
                textTreeWindow.TextInspector.ClickShowFile();
            }           
        }

        [OnOpenAsset]
        public static bool ClickAsset(int instanceId, int id)
        {
            if (Selection.activeObject is TextTree)
            {
                EditorWindow.GetWindow<TextTreeWindow>().Show();
                return true;
            }

            return false;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= TextTreeGraph.OnUndoPerformed;
        }

        public void CreateGUI()
        {
            string loadPath = Path.Combine(AssetDataManager.FrameworkLoadPath, "DialogueSystem", "TextTreeWindow");
            VisualTreeAsset visualTreeAsset = AssetDataManager.LoadAssetAtPath<VisualTreeAsset>(loadPath);
            visualTreeAsset.CloneTree(rootVisualElement);

            StyleSheet styleSheet = AssetDataManager.LoadAssetAtPath<StyleSheet>(loadPath);
            rootVisualElement.styleSheets.Add(styleSheet);

            TextTreeGraph = rootVisualElement.Q<TextGraphView>();
            TextInspector = rootVisualElement.Q<TextInspector>();

            TextToolBar = rootVisualElement.Q<TextToolContainer>();
            TextToolBar.Initial(OnTreeValidate, FindPlayingTree);

            TextToolBar.ButtonSave.clicked += SaveTree;
            TextToolBar.AddMenuListener("显示所有存在的树", (a) => TextInspector.ClickShowFile());
            TextToolBar.AddMenuListener("显示节点详细信息", (a) => TextInspector.ClickShowNode());

            TextTreeGraph.OnSelectEdge += TextInspector.OnSelectedEdge;
            TextTreeGraph.OnSelectNode += TextInspector.OnSelectNode;
            TextInspector.OnSelectedButton += LoadAsset;


            Undo.undoRedoPerformed -= TextTreeGraph.OnUndoPerformed;
            Undo.undoRedoPerformed += TextTreeGraph.OnUndoPerformed;
            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            if (!EditorApplication.isPlaying)
            {
                if (Selection.activeObject is TextTree textTree)
                {
                    SaveTree();
                    ShowTree(textTree);
                    if (TextInspector.NowTextTree != textTree)
                    {
                        TextInspector.FindSelection(textTree);
                    }
                }
            }
        }

        private void FindPlayingTree()
        {
            if (EditorApplication.isPlaying)
            {
                ShowTree(TextTreeRunner.Instance.MainTextTree);
            }
        }


        private void ShowTree(TextTree textTree)
        {
            _nowTextTree = textTree;
            TextTreeGraph.PaintNodes(textTree);
        }

        private void LoadAsset(TextTree textTree)
        {
            Selection.activeObject = textTree;
        }

        private void OnTreeValidate(TextTree[] textTrees)
        {
            TextInspector.Repaint();
            Selection.activeObject = textTrees[0];
        }

        private void SaveTree()
        {
            _nowTextTree?.SaveTree();
        }
    }
}
