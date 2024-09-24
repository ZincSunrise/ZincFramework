using DocumentFormat.OpenXml.Packaging;
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using ZincFramework.Events;
using ZincFramework.Excel;
using ZincFramework.Load.Editor;



namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public class TextToolContainer : VisualElement
            {
                public event ZincAction<TextTree[]> OnTreeValidate;

                public class UxmlFactroy : UxmlFactory<TextToolContainer, VisualElement.UxmlTraits> { }

                public ToolbarMenu TextToolBarMenu { get; set; }

                public ToolbarButton ButtonCreateNewTree { get; set; }

                public ToolbarButton ButtonExcelToTree { get; set; }

                public ToolbarButton ButtonTreeToExcel { get; set; }

                public ToolbarButton ButtonSave { get; set; }


                private string _nowLoadPath;

                private string _nowSavePath;

                public void Initial(ZincAction<TextTree[]> onTreeValidate)
                {
                    TextToolBarMenu = this.Query<ToolbarMenu>();
                    ButtonCreateNewTree = this.Q<ToolbarButton>(nameof(ButtonCreateNewTree));
                    ButtonExcelToTree = this.Q<ToolbarButton>(nameof(ButtonExcelToTree));
                    ButtonTreeToExcel = this.Q<ToolbarButton>(nameof(ButtonTreeToExcel));
                    ButtonSave = this.Q<ToolbarButton>(nameof(ButtonSave));

                    OnTreeValidate += onTreeValidate;
                    ButtonCreateNewTree.clicked += ShowCreatePanel;
                    ButtonExcelToTree.clicked += ShowExcelToTreePanel;
                }


                public void AddMenuListener(string actionName, Action<DropdownMenuAction> action, DropdownMenuAction.Status status = DropdownMenuAction.Status.Normal)
                {
                    TextToolBarMenu.menu.AppendAction(actionName, action, status);
                }


                private void ShowCreatePanel()
                {
                    TextTree textTree = ScriptableObject.CreateInstance<TextTree>();
                    if (AssetDataManager.SaveAssetInPanel<TextTree>(textTree, "创建新文本树", "Assets/AddressableAssets/StaticData/DialogueTree", "TextTree"))
                    {
                        OnTreeValidate?.Invoke(new TextTree[] { textTree});
                    }
                }

                private void ShowExcelToTreePanel()
                {
                    _nowLoadPath = EditorUtility.OpenFilePanel("选择Excel文件", "Assets/DialogueExcel", "xlsx");

                    if (!string.IsNullOrEmpty(_nowLoadPath))
                    {
                        using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(_nowLoadPath, false);

                        var collection = ExcelReader.GetExcelBook(spreadsheetDocument).ExcelSheets;
                        _nowSavePath = EditorUtility.SaveFilePanel("保存转换后的树", "Assets/AddressableAssets/StaticData/DialogueTree", collection[0].TableName, "asset");

                        if (!string.IsNullOrEmpty(_nowSavePath))
                        {
                            var textTrees = TreeExcelConverter.ExcelToTextTree(collection, _nowSavePath);
                            AssetDatabase.SaveAssets();
                            OnTreeValidate.Invoke(textTrees);
                        }  
                    }

                    _nowSavePath = null;
                    _nowLoadPath = null;
                }
            }
        }
    }
}