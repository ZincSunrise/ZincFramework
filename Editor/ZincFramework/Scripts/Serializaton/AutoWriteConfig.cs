using UnityEngine;


namespace ZincFramework
{
    namespace Serialization
    {
        public class AutoWriteConfig : ScriptableObject
        {
            //������
            public int nameLine = 0;

            //������
            public int typeLine = 1;

            //����
            public int keyLine = 2;

            //����˳������
            public int numberLine = 4;

            //���л���ʼ��
            public int startLine = 6;

            public string[] usingNamespaces;

            public string[] savePath;
 
        }
    }
}