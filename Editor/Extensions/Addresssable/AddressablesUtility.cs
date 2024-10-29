using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;


namespace ZincFramework.LoadServices.Addressable
{
    public static class AddressablesUtility
    {
        public static void AddObjectInGroup(string groupName, string loadPath)
        {
            AddressableAssetSettings addressableAssetSettings = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetGroup assetGroup = addressableAssetSettings.FindGroup(groupName);
            assetGroup ??= CreateNewGroup(groupName);

            var entry = addressableAssetSettings.CreateOrMoveEntry(AssetDatabase.GUIDFromAssetPath(loadPath).ToString(), assetGroup);
            entry.SetAddress(Path.GetFileNameWithoutExtension(loadPath));
        }

        public static void AddLabelnGroup(string label, string loadPath)
        {
            AddressableAssetSettings addressableAssetSettings = AddressableAssetSettingsDefaultObject.Settings;
            var entry = addressableAssetSettings.FindAssetEntry(AssetDatabase.GUIDFromAssetPath(loadPath).ToString());
            entry.SetLabel(label, true, true);
        }

        public static void AddObjectInGroup(string groupName, Object asset)
        {
            AddressableAssetSettings addressableAssetSettings = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetGroup assetGroup = addressableAssetSettings.FindGroup(groupName);

            var entry = addressableAssetSettings.CreateOrMoveEntry(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(asset)).ToString(), assetGroup);
            entry.SetAddress(asset.name);
        }

        public static void AddLabelnGroup(string label, Object asset)
        {
            AddressableAssetSettings addressableAssetSettings = AddressableAssetSettingsDefaultObject.Settings;
            var entry = addressableAssetSettings.FindAssetEntry(AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(asset)).ToString());

            entry.SetLabel(label, true, true);
        }

        private static AddressableAssetGroup CreateNewGroup(string groupName)
        {
            AddressableAssetSettings addressableAssetSettings = AddressableAssetSettingsDefaultObject.Settings;
            return addressableAssetSettings.CreateGroup(groupName, false, false, false, null);
        }
    }
}
