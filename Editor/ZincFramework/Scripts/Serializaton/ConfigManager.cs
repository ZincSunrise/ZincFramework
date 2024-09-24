using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZincFramework.Load.Editor;

namespace ZincFramework
{
    namespace Serialization
    {
        public class ConfigManager : BaseSafeSingleton<ConfigManager>
        {
            public AutoWriteConfig ExcelDefault { get; private set; }

            public AutoWriteConfig ProtocolDefault { get; private set; }

            private ConfigManager()
            {
                ExcelDefault = AssetDataManager.LoadAssetAtPath<AutoWriteConfig>(Path.Combine(AssetDataManager.ScriptLoadPath, "Excel", "Temp", "ExcelConfig"));
                ProtocolDefault = AssetDataManager.LoadAssetAtPath<AutoWriteConfig>(Path.Combine(AssetDataManager.ScriptLoadPath, "ProtocolTool", "Temp", "ProtocolConfig"));
            }
        }
    }
}