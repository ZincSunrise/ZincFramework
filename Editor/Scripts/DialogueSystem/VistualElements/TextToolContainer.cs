using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using ZincFramework.Events;
using ZincFramework.LoadServices.Editor;



namespace ZincFramework.DialogueSystem.GraphView
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

        public ToolbarButton ButtonRefresh { get; set; }

        private string _nowLoadPath;

        private string _nowSavePath;

        public void Initial(ZincAction<TextTree[]> onTreeValidate, Action refresh)
        {
            TextToolBarMenu = this.Query<ToolbarMenu>();
            ButtonCreateNewTree = this.Q<ToolbarButton>(nameof(ButtonCreateNewTree));
            ButtonExcelToTree = this.Q<ToolbarButton>(nameof(ButtonExcelToTree));
            ButtonTreeToExcel = this.Q<ToolbarButton>(nameof(ButtonTreeToExcel));
            ButtonSave = this.Q<ToolbarButton>(nameof(ButtonSave));
            ButtonRefresh = this.Q<ToolbarButton>(nameof(ButtonRefresh));

            OnTreeValidate += onTreeValidate;
            ButtonCreateNewTree.clicked += ShowCreatePanel;
            ButtonExcelToTree.clicked += ShowExcelToTreePanel;
            ButtonTreeToExcel.clicked += TreeToExcel;
            ButtonRefresh.clicked += refresh;
        }

        [Obsolete]
        private void TreeToExcel()
        {
/*            if (Selection.activeObject is TextTree)
            {
                TextTree[] textTrees = Array.ConvertAll(Selection.objects, x => x as TextTree);
                string savePath = EditorUtility.OpenFolderPanel("选择保存的文件夹", Application.dataPath, "DialogueExcel");
                if (!string.IsNullOrEmpty(savePath))
                {
                    TreeToExcelConverter.WriteExcel(textTrees, savePath);
                }
            }*/
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
                OnTreeValidate?.Invoke(new TextTree[] { textTree });
            }
        }

        [Obsolete]
        private void ShowExcelToTreePanel()
        {
/*            _nowLoadPath = EditorUtility.OpenFilePanel("选择Excel文件", "Assets/DialogueExcel", "xlsx");

            if (!string.IsNullOrEmpty(_nowLoadPath))
            {
                using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(_nowLoadPath, false);

                var collection = ExcelReader.GenerateExcelBook(spreadsheetDocument).ExcelSheets;
                _nowSavePath = EditorUtility.SaveFilePanel("保存转换后的树", "Assets/AddressableAssets/StaticData/DialogueTree", collection[0].TableName, "asset");

                if (!string.IsNullOrEmpty(_nowSavePath))
                {
                    var textTrees = ExcelToTreeConverter.ExcelToTextTree(collection, _nowSavePath);
                    AssetDatabase.SaveAssets();
                    OnTreeValidate.Invoke(textTrees);
                }
            }

            _nowSavePath = null;
            _nowLoadPath = null;*/
        }
    }
}