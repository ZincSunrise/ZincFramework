using UnityEngine.UI;

namespace ZincFramework
{
    namespace Localization
    {
        public static class LocalizationFactroy
        {
            private static LocalizationInfo _localizationInfo;

            public static string GetString(string text)
            {
                if(_localizationInfo == null)
                {
                    return text;
                }

                return string.Empty;
            }
        }
    }
}

