using UnityEngine;
using System.Text;
using System.Collections.Generic;
using ZincFramework.Binary.Serialization;
using ZincFramework.LoadServices.Addressable;
using ZincFramework.Localization.Serialization;
using System.Threading.Tasks;



namespace ZincFramework.Localization
{
    public class LocalizationManager : BaseSafeSingleton<LocalizationManager>
    {
        public SerializerOption LocalSerializerOption 
        {
            get 
            {
                return _localSerializerOption ??= new SerializerOption()
                {
                    Converters = new[]
                    {
                        new LocalizationConverter()
                    },

                    Encoding = Encoding.UTF8,
                };
            }
        }

        private ILocalizer<string> _strLocalizer;

        private SerializerOption _localSerializerOption;

        private LocalizationManager() { }

        public async Task LoadAllLocalizerAsync(string cultureName)
        {
            _strLocalizer ??= new StrLocalizer();
            IList<TextAsset> textAssets = await AddressablesManager.Instance.LoadAssetsAsync<TextAsset>(cultureName);

            for (int i = 0; i < textAssets.Count; i++) 
            {
                LocalizationInfo localizationInfo = BinarySerializer.Deserialize<LocalizationInfo>(textAssets[i].bytes, LocalSerializerOption);
                foreach(var data in localizationInfo.LocalizationStr)
                {
                    _strLocalizer.AddOrUpdateLocalization(data.Key, data.Value);
                }
            }
        }

        public string GetLocalzationStr(string key)
        {
            return _strLocalizer.GetLocalization(key);
        }
    }
}