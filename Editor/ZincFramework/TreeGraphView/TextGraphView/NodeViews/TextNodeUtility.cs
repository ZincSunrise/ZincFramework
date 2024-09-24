using GameData.StaticData;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UIElements;
using ZincFramework.Excel;
using static ZincFramework.TreeGraphView.TextTree.ChoiceNode;



namespace ZincFramework.TreeGraphView.TextTree
{
    public static class TextNodeUtility
    {
        private static Dictionary<string, FieldInfo> _setMap = new Dictionary<string, FieldInfo>();

        public static StaffData StaffData { get; } = ExcelBinaryReader.LoadDictionaryData<StaffData, int, StaffInfo>(nameof(StaffData));

        static TextNodeUtility()
        {
            FieldInfo[] fieldInfos = typeof(BaseTextNode).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo fieldInfo in fieldInfos) 
            {
                _setMap.Add(fieldInfo.Name, fieldInfo);
            }

            fieldInfos = typeof(ChoiceInfo).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                _setMap.Add(fieldInfo.Name, fieldInfo);
            }

        }

        public static void SetNodeName(BaseTextNode baseTextNode, string staffName)
        {
            FieldInfo fieldInfo = _setMap["_staffName"];
            fieldInfo.SetValue(baseTextNode, staffName);
        }

        public static void SetNodeDifferential(BaseTextNode baseTextNode, int differential) 
        {
            FieldInfo fieldInfo = _setMap["_differential"];
            fieldInfo.SetValue(baseTextNode, differential);
        }

        public static void SetNodeStatement(BaseTextNode baseTextNode, string statement)
        {
            FieldInfo fieldInfo = _setMap["_statement"];
            fieldInfo.SetValue(baseTextNode, statement);
        }

        public static void AddStaffItem(DropdownField dropdownField)
        {
            foreach(var data in StaffData.StaffInfos.Values)
            {
                dropdownField.choices.Add(data.Name);
            }
        }

        public static void SetChoiceText(ChoiceInfo choiceInfo, string text)
        {
            FieldInfo fieldInfo = _setMap["_choiceText"];
            fieldInfo.SetValue(choiceInfo, text);
        }
    }
}
