using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace ZincFramework.MVC.Editor
{
    public class MVCWindow : EditorWindow
    {
        private Button _buttonGenerate;

        private Button _buttonChoose;

        private Label _textPath;

        private TextField _textField;

        private EnumField _typeField;


        [MenuItem("Assets/Create/MVC/GenerateMVC")]
        public static void ShowExample()
        {
            MVCWindow wnd = GetWindow<MVCWindow>();
            wnd.titleContent = new GUIContent("MVCWindow");
        }

        private void OnEnable()
        {
            if(_buttonChoose == null)
            {
                VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ZincFramework/MVC/MVCWindow.uxml");
                visualTreeAsset.CloneTree(rootVisualElement);

                StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/ZincFramework/MVC/MVCWindow.uss");
                rootVisualElement.styleSheets.Add(styleSheet);

                _buttonChoose = rootVisualElement.Q<Button>("ButtonChoosePath");
                _buttonGenerate = rootVisualElement.Q<Button>("ButtonGenerate");
                _textPath = rootVisualElement.Query<Label>();
                _textField = rootVisualElement.Q<TextField>();
                _typeField = rootVisualElement.Q<EnumField>();

                _buttonChoose.clicked += ChoosePath;
                _buttonGenerate.clicked += GenerateCode;
            }

            if(Selection.activeObject != null)
            {
                string path = Path.Combine(Application.dataPath, AssetDatabase.GetAssetPath(Selection.activeObject)[7..]);
                _textPath.text = path;
            }
        }


        private void ChoosePath()
        {
            string path;
            if (string.IsNullOrEmpty(_textPath.text))
            {
                path = EditorUtility.OpenFolderPanel("选择创建路径", Application.dataPath, string.Empty);
            }
            else
            {
                path = EditorUtility.OpenFolderPanel("选择创建路径", Path.GetDirectoryName(_textPath.text), string.Empty);
            }
            
            if (!string.IsNullOrEmpty(path))
            {
                _textPath.text = path;
            }
        }

        private void GenerateCode()
        {
            MVCGenerator.GenerateMVC(_textPath.text, _textField.text, (MVCGenerateType)_typeField.value);
        }
    }
}
