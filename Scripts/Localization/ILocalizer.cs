namespace ZincFramework.Localization
{
    public interface ILocalizer<T>
    {
        T GetLocalization(string key);

        void AddOrUpdateLocalization(string key, T value);

        void RemoveLocalization(string key);
    }
}
