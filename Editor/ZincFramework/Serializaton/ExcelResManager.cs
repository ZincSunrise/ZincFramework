using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZincFramework.Excel;
using ZincFramework.Load.Editor;

namespace ZincFramework
{
    namespace Serialization
    {
        public class ExcelResManager : BaseSafeSingleton<ExcelResManager>
        {
            public AutoWriteConfig ExcelDefault { get; private set; }

            public AutoWriteConfig ProtocolDefault { get; private set; }

            public ExcelSytleData DefaultStyleData { get; private set; }

            private ExcelResManager()
            {
                string path = Path.Combine(AssetDataManager.FrameworkLoadPath, "Excel", "Temp");
                ExcelDefault = AssetDataManager.LoadAssetAtPath<AutoWriteConfig>(Path.Combine(path, "ExcelConfig"));
                DefaultStyleData = AssetDataManager.LoadAssetAtPath<ExcelSytleData>(Path.Combine(path, "DefaultStyleData"));
                ProtocolDefault = AssetDataManager.LoadAssetAtPath<AutoWriteConfig>(Path.Combine(AssetDataManager.FrameworkLoadPath, "ProtocolTool", "Temp", "ProtocolConfig"));
            }
        }
    }
}