using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ZincFramework
{
    namespace TreeExtension
    {
        namespace TreeNode
        {
            public abstract class BaseNode : ScriptableObject
            {
                public enum E_Node_State
                {
                    None = 0,
                    Success = 2,
                    Excuting = 4,
                    Failed = 6,
                }

                public E_Node_State State { get; set; }

                public string Guid => _guid;

                [SerializeField]
                private string _guid;

#if UNITY_EDITOR
                public Vector2 graphPostion;
#endif
                public abstract void Excute();

                public abstract List<BaseNode> GetChildren();


                public abstract void SetChild(BaseNode baseNode);


                public abstract void RemoveChild(BaseNode baseNode);
            }
        }
    }
}
