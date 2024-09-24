using System.IO;
using UnityEditor;
using UnityEngine;



namespace ZincFramework
{
    namespace Localization
    {
        public static class LocalizationTool
        {
            public static string localizationExcelPath = Application.dataPath + "/ArtRes/Localization/";

            [MenuItem("GameTool/Localization/GenerateLocalFile")]
            public static void GenerateLocalFile()
            {
                if (!Directory.Exists(localizationExcelPath))
                {
                    Directory.CreateDirectory(localizationExcelPath);
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(localizationExcelPath);

                FileInfo[] fileInfos = directoryInfo.GetFiles();
                foreach (FileInfo fileInfo in fileInfos)
                {
                    if (fileInfo.Extension != ".xlsx" && fileInfo.Extension != ".xls")
                    {
                        continue;
                    }

                    using (FileStream fileStream = fileInfo.OpenRead())
                    {

                    }
                }
            }
        }
    }
}
