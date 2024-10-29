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

            // ��ȡ��������
            string typeName = type.Name;

            // ��ȡ�㼶·��
            Transform transform = selectable.transform;
            StringBuilder path = new StringBuilder();

            while (transform.parent != null && transform.parent.name != parentName)
            {
                path.Insert(0, '/' + transform.name);
                transform = transform.parent;
            }

            // ��������岻��Ҫб�ܣ�ֱ�Ӳ�����ڵ������
            path.Insert(0, transform.name);

            // ����UIWriteInfo����
            return new UIWriteInfo(TextUtility.UpperFirstChar(selectable.name), path.ToString(), typeName);
        }
    }
}