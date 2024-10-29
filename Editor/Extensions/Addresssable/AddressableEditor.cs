using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System.Linq;
using System.Collections.Generic;



namespace ZincFramework.LoadServices.Addressable
{
    public static class AddressableEditor
    {
        private static Dictionary<string, Object> _cachedAssets = new Dictionary<string, Object>();

        public static T LoadAsset<T>(this AddressablesManager addressablesManager, string name) where T : Object
        {
            if (!_cachedAssets.TryGetValue(name, out var obj))
            {
                AddressableAssetSettings addressableAssetSettings = AddressableAssetSettingsDefaultObject.Settings;
                foreach (var group in addressableAssetSettings.groups)
                {                  
                    var asset = group.entries.FirstOrDefault(x => x.address == name);

                    if(asset != null)
                    {
                        obj = AssetDatabase.LoadAssetAtPath<T>(asset.AssetPath);
                        _cachedAssets.Add(name, obj);
                        return obj as T;
                    }
                }
            }

            return obj as T;
        }
    }
}
