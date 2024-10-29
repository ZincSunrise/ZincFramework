using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using ZincFramework.UI.Complex;


namespace ZincFramework.UI.Collections
{
    public class UICollector
    {
        public UIWriteInfo GetUIWriteInfos(string parentName, Selectable selectable)
        {
            Type type = selectable.TryGetComponent<ComplexUI>(out var complexUI) ? complexUI.GetType() : selectable.GetType();

            // 获取类型名称
            string typeName = type.Name;

            // 获取层级路径
            Transform transform = selectable.transform;
            StringBuilder path = new StringBuilder();

            while (transform.parent != null && transform.parent.name != parentName)
            {
                path.Insert(0, '/' + transform.name);
                transform = transform.parent;
            }

            // 最外层物体不需要斜杠，直接插入根节点的名称
            path.Insert(0, transform.name);

            // 返回UIWriteInfo对象
            return new UIWriteInfo(TextUtility.UpperFirstChar(selectable.name), path.ToString(), typeName);
        }
    }
}