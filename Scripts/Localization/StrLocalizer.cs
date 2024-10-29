using System.Collections.Generic;
using UnityEngine;


namespace ZincFramework.Localization
{
    public class StrLocalizer : ILocalizer<string>
    {
        public string CultureName { get; private set; } 

        private Dictionary<string, string> _localizationKeyValuePairs = new Dictionary<string, string>();

        public void ReloadLocalStr(LocalizationInfo localizationInfo)
        {
            _localizationKeyValuePairs = localizationInfo.LocalizationStr;
        }

        public string GetLocalization(string key)
        {
            if(_localizationKeyValuePairs.TryGetValue(key, out var value))
            {
                return value;
            }

            Debug.LogWarning($"�����ڼ�ֵΪ{key}�ĵ�ǰ�����ַ�����");
            return null;
        }

        public void AddOrUpdateLocalization(string key, string value)
        {
            _localizationKeyValuePairs[key] = value;
        }

        public void RemoveLocalization(string key)
        {
            _localizationKeyValuePairs.Remove(key);
        }
    }
}