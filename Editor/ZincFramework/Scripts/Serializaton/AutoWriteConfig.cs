using UnityEngine;


namespace ZincFramework
{
    namespace Serialization
    {
        public class AutoWriteConfig : ScriptableObject
        {
            //名字行
            public int nameLine = 0;

            //类型行
            public int typeLine = 1;

            //键行
            public int keyLine = 2;

            //序列顺序码行
            public int numberLine = 4;

            //序列化开始行
            public int startLine = 6;

            public string[] usingNamespaces;

            public string[] savePath;
 
        }
    }
}