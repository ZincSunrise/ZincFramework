using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ZincFramework
{
    namespace Localization
    {
        public class LocalizationInfo
        {
            private readonly Dictionary<int, string> _localStrings = new Dictionary<int, string>();

            public LocalizationInfo() { }

            public void AddLocalString(int wordKey, string word)
            {
                _localStrings.Add(wordKey, word);
            }

            public void RemoveLocalString(int wordKey, string word)
            {
                _localStrings.Remove(wordKey);
            }
        }
    }
}
