using ZincFramework.LoadServices.Editor;
using ZincFramework.UI;


namespace ZincFramework
{
    public static class ConfigUtility
    {
        private static UIConfig Default { get; set; }

        public static UIConfig LoadUIConfig()
        {
            Default = Default == null ? AssetDataManager.LoadAssetAtPath<UIConfig>("Editor/ZincFramework/Scripts/UITool/Configs/UIConfig") : Default;

            return Default;
        }
    }
}