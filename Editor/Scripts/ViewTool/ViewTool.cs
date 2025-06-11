using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace ZincFramework.UI.ViewWriter
{
    public static class ViewTool
    {
        [MenuItem("Assets/Create/UI Toolkit/Create Script")]
        [MenuItem("GameTool/UI Toolkit/Create Script")]
        [MenuItem("GameObject/UI Toolkit/Create Script")]
        private static void CreateViewScript()
        {
            VisualElement container = Selection.activeObject switch
            {
                VisualTreeAsset treeAsset => treeAsset.CloneTree(),
                UIDocument uIDocument => uIDocument.visualTreeAsset?.CloneTree(),
                _ => null
            };

            if(container == null)
            {
                Debug.LogWarning("����UXML�ļ�����UIDocument�в����и�����,��ѡ��һ��UXML�ļ�����UIDocument����");
                return;
            }

            string savePath = EditorUtility.SaveFilePanel("��������ļ�", UIConfig.ViewPath, Selection.activeObject.name + "Base", "cs");
            var config = AssetDatabase.LoadAssetAtPath<UIConfig>(UIConfig.ViewLoadPath);

            if (!string.IsNullOrEmpty(savePath))
            {
                ViewWriter.WriteViewScript(container, Selection.activeObject.name, savePath, config);
            }
        }
    }
}