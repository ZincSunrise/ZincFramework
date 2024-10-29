using System.Collections.Generic;
using UnityEngine;



namespace ZincFramework.DialogueSystem.Analysis
{
    public class DialoguePreserver
    {
        private readonly GameObject _dialogueUser;

        private readonly Dictionary<System.Type, Object> _userComponents = new Dictionary<System.Type, Object>();

        public DialoguePreserver(GameObject dialogueUser)
        {
            _dialogueUser = dialogueUser;
        }

        public T GetUserComponent<T>() where T : Object
        {
            var type = typeof(T);
            if (!_userComponents.TryGetValue(type, out var value))
            {
                value = _dialogueUser.GetComponent<T>();
                _userComponents.Add(type, value);
            }

            return value as T ?? throw new System.ArgumentException($"该对象{_dialogueUser.name}不存在{type}的组件");
        }
    }
}