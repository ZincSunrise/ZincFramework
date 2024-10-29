using UnityEngine;
using System.Collections.Generic;
using ZincFramework.UI.Switchable;


namespace ZincFramework.UI
{
    public class UISwitchGroup : MonoBehaviour
    {
        [SerializeField]
        private List<ISwitchable> _switchables = new List<ISwitchable>();

        public void AddSwitch(ISwitchable switchable)
        {
            _switchables.Add(switchable);
        }

        public void RemoveSwitch(ISwitchable switchable)
        {
            _switchables.Remove(switchable);
        }

        public void HideElements(ISwitchable switchable)
        {
            for (int i = 0; i < _switchables.Count; i++) 
            {
                if (_switchables[i] != switchable)
                {
                    _switchables[i].SwitchOff();
                }
            }
        }
    }
}