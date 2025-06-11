using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZincFramework.DialogueSystem.TextData;
using ZincFramework.Excel;
using CharacterInfo = ZincFramework.DialogueSystem.TextData.CharacterInfo;



namespace ZincFramework.DialogueSystem
{
    public static class TextNodeUtility
    {
        private readonly static Dictionary<string, FieldInfo> _setMap = new Dictionary<string, FieldInfo>();
        public static CharacterData CharacterData { get; }

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
            
            CharacterData = ExcelBinaryReader.LoadDictionaryData<CharacterData, int, CharacterInfo>("Characters");
        }


        public static void SetNodeName(BaseTextNode baseTextNode, string staffName)
        {
            FieldInfo fieldInfo = _setMap["_staffName"];
            fieldInfo.SetValue(baseTextNode, staffName);
        }

        public static void SetNodeStatement(BaseTextNode baseTextNode, string statement)
        {
            FieldInfo fieldInfo = _setMap["_dialogueText"];
            fieldInfo.SetValue(baseTextNode, statement);
        }


        public static void SetChoiceText(ChoiceInfo choiceInfo, string text)
        {
            FieldInfo fieldInfo = _setMap["_choiceText"];
            fieldInfo.SetValue(choiceInfo, text);
        }

        public static void SetVisiblable(BaseTextNode baseTextNode, VisibleState[] visibleStates)
        {
            _setMap["_visibleStates"].SetValue(baseTextNode, visibleStates);
        }

        public static void RemoveVisible(BaseTextNode baseTextNode, IEnumerable<int> indies)
        {
            List<VisibleState> visibleStates = new List<VisibleState>(baseTextNode.VisibleStates);
            List<VisibleState> willDelete = new List<VisibleState>();

            foreach(int index in indies)
            {
                if(index < visibleStates.Count)
                {
                    willDelete.Add(visibleStates[index]);
                }
            }

            visibleStates.RemoveAll(x => willDelete.Contains(x));
            _setMap["_visibleStates"].SetValue(baseTextNode, visibleStates.ToArray());
        }

        public static string GetSpriteName(in VisibleState visiblePair)
        {
            if(visiblePair.VisableId == 0 && visiblePair.Differential == 0)
            {
                return string.Empty;
            }

            int id = visiblePair.VisableId;
            var characterName = CharacterData.CharacterInfos.First(x => x.Value.CharaceterId == id).Value.CharacterName;
            return $"{characterName}_{visiblePair.Differential}";
        }

        public static void GetSpriteId(string name, out int visableId, out int differential)
        {
            if (string.IsNullOrEmpty(name))
            {
                visableId = 0;
                differential = 0;
                return;
            }

            int index = name.IndexOf('_');
            differential = int.Parse(name.AsSpan()[(index + 1)..]);

            string charaName = name[..index];
            visableId = CharacterData.CharacterInfos.First(x => x.Value.CharacterName == charaName).Value.CharaceterId;
        }

        public static string GetCharacterNameFormId(int id)
        {
            return CharacterData.CharacterInfos.First(x => x.Value.CharaceterId == id).Value.CharacterName;
        }
    }
}
