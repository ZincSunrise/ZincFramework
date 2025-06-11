using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZincFramework.UI.Complex;


namespace ZincFramework.UI.Collections
{
    public class UICollector
    {
        public UIWriteInfo GetUIWriteInfos(string parentName, UIBehaviour uiBehaviour)
        {
            Type type = uiBehaviour.TryGetComponent<ComplexUI>(out var complexUI) ? complexUI.GetType() : uiBehaviour.GetType();

            // 获取类型名称
            string typeName = type.Name;

            // 获取层级路径
            Transform transform = uiBehaviour.transform;
            StringBuilder path = new StringBuilder();

            while (transform.parent != null && transform.parent.name != parentName)
            {
                path.Insert(0, '/' + transform.name);
                transform = transform.parent;
            }

            // 最外层物体不需要斜杠，直接插入根节点的名称
            path.Insert(0, transform.name);

            // 返回UIWriteInfo对象
            return new UIWriteInfo(TextUtility.UpperFirstChar(uiBehaviour.name), path.ToString(), typeName);
        }
    }
}