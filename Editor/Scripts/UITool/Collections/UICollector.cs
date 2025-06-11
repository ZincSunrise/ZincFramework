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

            // ��ȡ��������
            string typeName = type.Name;

            // ��ȡ�㼶·��
            Transform transform = uiBehaviour.transform;
            StringBuilder path = new StringBuilder();

            while (transform.parent != null && transform.parent.name != parentName)
            {
                path.Insert(0, '/' + transform.name);
                transform = transform.parent;
            }

            // ��������岻��Ҫб�ܣ�ֱ�Ӳ�����ڵ������
            path.Insert(0, transform.name);

            // ����UIWriteInfo����
            return new UIWriteInfo(TextUtility.UpperFirstChar(uiBehaviour.name), path.ToString(), typeName);
        }
    }
}