using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.Load.Editor;


namespace ZincFramework
{
    namespace UI
    {
        [CreateAssetMenu(fileName = "UIConfig", menuName = "GameTool/UI/UIConfig")]
        public class UIConfig : ScriptableObject
        {
            public static UIConfig Default { get; private set; }


            private void OnEnable()
            {
                Default = Default != null ? Default : AssetDataManager.LoadAssetAtPath<UIConfig>("Editor/ZincFramework/Scripts/UITool/Configs/UIConfig");
            }

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