using UnityEngine;
using System.Collections.Generic;
using System.IO;


namespace ZincFramework
{
    namespace UI
    {
        [CreateAssetMenu(fileName = "UIConfig", menuName = "GameTool/UI/UIConfig")]
        public class UIConfig : ScriptableObject
        {
            public static string UIPath { get; } = Path.Combine(Application.dataPath, "Scripts", "UI");

            public static string ViewPath { get; } = Path.Combine(Application.dataPath, "Scripts", "View");


            public const string UILoadPath = "Assets/Editor/Zincframework/UITool/Configs/UIConfig.asset";


            public const string ViewLoadPath = "Assets/Editor/Zincframework/UITool/Configs/ViewConfig.asset";

            public List<string> Namespaces => _namespaces;


            [SerializeField]
            private List<string> _namespaces = new List<string>();

            public string ClassNamespaces => _classNamespaces;

            [SerializeField]
            private string _classNamespaces;

            public string[] Parents => _parents;

            [SerializeField]
            private string[] _parents;
        }
    }
}