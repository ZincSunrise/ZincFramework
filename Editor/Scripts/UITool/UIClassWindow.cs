using System;
using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;
using ZincFramework.UI.EditorUI;
using ZincFramework.ScriptWriter;
using SystemAssembly = System.Reflection.Assembly;
using ZincFramework.UI.ScriptWriter;



namespace ZincFramework
{
    namespace UI
    {
        public class UIClassWindow : EditorWindow
        {
            [MenuItem("GameObject/UI/CreateUIScript")]
            [MenuItem("GameTool/UI/OpenUICreateWindow")]
            private static void OpenWindow()
            {
                EditorWindow.GetWindow(typeof(UIClassWindow)).Show();
            }

            [MenuItem("GameTool/UI/CloseUIScript")]
            private static void CloseWindow()
            {
                DestroyImmediate(EditorWindow.GetWindow(typeof(UIClassWindow)));
            }


            private readonly EditorScrollView _baseScrollView = new EditorScrollView();
            private readonly EditorLabel _baseLabel = new EditorLabel();

            private readonly ObjectField<UIConfig> _uiConfig = new ObjectField<UIConfig>(null, "配置文件种类", false);
            private readonly EditorButton _buttonOpenSavePanel = new EditorButton("保存面板");

            private readonly EditorCollection _editorUIBases;

            private string _savePath;
            private string _selectingName;

            private Selectable[] Selectables { get; set; }

            public static string UIPath { get; } = Path.Combine(Application.dataPath, "Scripts", "UI");

            public UIClassWindow()
            {
                _baseScrollView.Add(_baseLabel);

                _buttonOpenSavePanel.OnClick += SavePanel;
                _editorUIBases = new EditorCollection(this);
            }

            private void OnEnable()
            {
                _uiConfig.Value = AssetDatabase.LoadAssetAtPath<UIConfig>(UIConfig.DefaultLoadPath);
                OnSelectionChange();
            }

            private void OnDisable()
            {
                CompilationPipeline.assemblyCompilationFinished -= AssemblyCompilationFinished;
            }


            private void AssemblyCompilationFinished(string arg1, CompilerMessage[] arg2)
            {
                if (arg1.Contains("Assembly-CSharp.dll"))
                {
                    for (int i = 0; i < arg2.Length; i++)
                    {
                        if (arg2[i].type == CompilerMessageType.Error)
                        {
                            return;
                        }
                    }

                    SystemAssembly assembly = SystemAssembly.LoadFrom(Path.Combine(Path.GetDirectoryName(Application.dataPath), arg1));

                    string typeString;
                    if (string.IsNullOrEmpty(_uiConfig.Value.ClassNamespaces))
                    {
                        typeString = _selectingName;
                    }
                    else
                    {
                        typeString = _uiConfig.Value.ClassNamespaces + '.' + _selectingName;
                    }

                    GameObject gameObject = Selection.activeGameObject;
                    Type type = assembly.GetType(typeString, false, true);

                    if (type != null && !gameObject.GetComponent(type))
                    {
                        gameObject.AddComponent(type);
                    }

                    CompilationPipeline.assemblyCompilationFinished -= AssemblyCompilationFinished;
                }
            }

            private void OnGUI()
            {
                _editorUIBases.ForEach(ExcuteGUI);
            }

            private void ExcuteGUI(IEditorUI editorUI)
            {
                editorUI.OnGUI();
            }

            private void SavePanel()
            {
                CompilationPipeline.assemblyCompilationFinished -= AssemblyCompilationFinished;
                CompilationPipeline.assemblyCompilationFinished += AssemblyCompilationFinished;

                if (!Directory.Exists(UIPath))
                {
                    Directory.CreateDirectory(UIPath);
                }

                _savePath = EditorUtility.SaveFilePanel("保存你的文件", UIPath, _selectingName + "Base", "cs");

                if (!string.IsNullOrEmpty(_savePath))
                {
                    UIClassWriter.WriteClass(Selectables, _selectingName, _savePath, _uiConfig.Value);   
                }
                else
                {
                    CompilationPipeline.assemblyCompilationFinished -= AssemblyCompilationFinished;
                }
            }

            private void OnSelectionChange()
            {
                if (Selection.activeGameObject != null)
                {
                    GameObject gameObject = Selection.activeGameObject;

                    if (!gameObject.transform.IsChildOf(GameObject.Find("Canvas").transform) || !gameObject.name.Contains("Panel"))
                    {
                        return;
                    }
                    _selectingName = gameObject.name;

                    Selectables = gameObject.GetComponentsInChildren<Selectable>();
                    CSharpWriter cSharpWriter = CSharpWriter.RentWriter();
                    string[] strings = UIClassWriter.GetClassStrings(cSharpWriter, _selectingName, Selectables, _uiConfig.Value);
                    _baseLabel.Title = string.Join(Environment.NewLine, strings);
                }
            }
        }
    }
}