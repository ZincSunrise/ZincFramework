using UnityEngine;
using System.Collections.Generic;


namespace ZincFramework
{
    namespace UI
    {
        [CreateAssetMenu(fileName = "UIConfig", menuName = "GameTool/UI/UIConfig")]
        public class UIConfig : ScriptableObject
        {
            public const string DefaultLoadPath = "Assets/Editor/Zincframework/UITool/Configs/UIConfig.asset";
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